using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTrackAndShoot : MonoBehaviour
{
    #region Variables
    
    [Header("Floats")]
    [SerializeField] float rotSpeed = 150f;
    [SerializeField] float range = 11f;
    readonly float shootForce = 15f;
    [SerializeField] float firerate = 0.1f;
    [SerializeField] float cooldown = 1f;
    [SerializeField] float viewAngle = 135f;

    [Header("Bools")]
    bool shooting;
    bool tracking;
    [SerializeField] bool isLookingLeft = true;
    [SerializeField] bool isOmniDirectional = false;
    [SerializeField] bool obstructed;
    [SerializeField] bool inRange;
    [SerializeField] bool inAngle;

    [Header("Vector3s")]
    Vector3 FoVPointA;
    Vector3 FoVPointB;
    Vector3 topAngle;
    Vector3 bottomAngle;
    static Vector3 DirFromAngle(float angleIndDegrees)
    {
        return new Vector3(Mathf.Sin((angleIndDegrees - 90f) * Mathf.Deg2Rad), Mathf.Cos((angleIndDegrees - 90f) * Mathf.Deg2Rad), 0);
    }

    [Header("Transforms")]
    Transform target;
    Transform player;
    [SerializeField] Transform firePoint;

    [Header("GameObjects")]
    [SerializeField] GameObject bulletPrefab;
    GameObject BulletPrefabs;

    [Header("LayerMasks")]
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask obstructionMask;

    [Header("AudioClips")]
    [SerializeField] AudioClip shootAudio;

    [Header("Components")]
    AudioManager audioManager;

    #endregion

    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform.Find("Target");
        BulletPrefabs = GameObject.FindWithTag("BulletPrefabFolder");

        audioManager = GameObject.FindWithTag("Managers").GetComponent<AudioManager>();

        if (isLookingLeft)
        {
            topAngle = DirFromAngle(-viewAngle / 2);
            bottomAngle = DirFromAngle(viewAngle / 2);
        }
        else
        {
            topAngle = DirFromAngle((-viewAngle + 360) / 2);
            bottomAngle = DirFromAngle((viewAngle + 360) / 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Fov Triangle
        FoVPointA = transform.position + topAngle * (range + range * 1.6f);
        FoVPointB = transform.position + bottomAngle * (range + range * 1.6f);

        obstructed = IsObstructed();
        inRange = TargetInRange();
        inAngle = TargetWithinAngle();

        if (!target && !shooting)
        {
            RotateToDefault();
            FindTarget();
            return;
        }

        if (IsObstructed() || !TargetInRange() || !TargetWithinAngle())
        {
            target = null;
            StopCoroutine(nameof(Shooting));
        }
        
        if (!shooting)
        {
            RotateToTarget();
        }

        if (target && tracking)
        {
            StartCoroutine(nameof(Shooting));
        }
    }

    #endregion

    #region FindPlayerMethods

    void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, (Vector2) transform.position, 0f, playerMask);

        if ((hits.Length > 0) && TargetInRange() && TargetWithinAngle() && !IsObstructed())
        {
            tracking = true;
            if (hits[0].transform.CompareTag("Player"))
            {
                target = player;
            }
        }
        else
        {
            target = null;
            tracking = false;
        }
    }

    bool IsObstructed()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, obstructionMask);

        if (hit)
        {
            // print("Target obstructed");
            return true;
        }
        // print("Target not obstructed");
        return false;
    }

    bool TargetInRange()
    {
        if (Vector2.Distance(player.position, transform.position) <= range)
        {
            return true;
        }
        return false;
    }

    bool TargetWithinAngle()
    {
        if (!isOmniDirectional)
        {
            Vector3 d, e;
            Vector3 a = FoVPointA;
            Vector3 b = FoVPointB;
            Vector3 c = transform.position;
            Vector3 p = player.position;

            float w1, w2;

            d = b - a;
            e = c - a;

            w1 = (e.x * (a.y - p.y) + e.y * (p.x - a.x)) / (d.x * e.y - d.y * e.x);
            w2 = (p.y - a.y - w1 * d.y) / e.y;

            return (w1 >= 0.0) && (w2 >= 0.0) && ((w1 + w2) <= 1.0);
        }
        else
        {
            return true;
        }
    }

    void RotateToTarget()
    {
        if (target)
        {
            float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg + 180;

            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }
    }

    void RotateToDefault()
    {
        Quaternion targetRotation;
        if (isLookingLeft)
        {
            targetRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }
        else
        {
            targetRotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }

    #endregion

    #region ShootingMethods

    IEnumerator Shooting()
    {
        tracking = false;
        // print("Preparing burst");
        
        yield return new WaitForSeconds(cooldown);

        shooting = true;
        StartCoroutine(ShootBurst());
        // print("Burst started");
    }

    IEnumerator ShootBurst()
    {
        WaitForSeconds wait = new(firerate);

        Shoot();
        yield return wait;

        Shoot();
        yield return wait;

        Shoot();
        yield return wait;

        Shoot();
        yield return wait;

        Shoot();
        yield return wait;

        shooting = false;
        target = null;
        // print("Burst ended");
    }

    void Shoot()
    {
        GameObject bulletCopy = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity, BulletPrefabs.transform);
        audioManager.PlayClip(shootAudio, "sfx");
        
        if (!isOmniDirectional)
        {
            bulletCopy.GetComponent<Rigidbody2D>().AddForce(-firePoint.right * shootForce, ForceMode2D.Impulse);
        }
        else
        {
            bulletCopy.GetComponent<Rigidbody2D>().AddForce(firePoint.up * shootForce, ForceMode2D.Impulse);
        }

        bulletCopy.transform.rotation = transform.rotation * Quaternion.Euler(new Vector3(0,0,90));
        bulletCopy.transform.localPosition += new Vector3(0f, 0f, -4f);

        Destroy(bulletCopy, 3f);
    }

    #endregion
    
    #region Gizmos:

    void OnDrawGizmosSelected()
    {
        if (!isOmniDirectional)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + topAngle * range);
            Gizmos.DrawLine(transform.position, transform.position + bottomAngle * range);
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);

        if (player)
        {
            Gizmos.DrawLine(transform.position, player.position);
        }

        if (target)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);

            if (!isOmniDirectional)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, FoVPointA);
                Gizmos.DrawLine(transform.position, FoVPointB);
                Gizmos.DrawLine(FoVPointA, FoVPointB);
            }
        }
    }

    #endregion
}
