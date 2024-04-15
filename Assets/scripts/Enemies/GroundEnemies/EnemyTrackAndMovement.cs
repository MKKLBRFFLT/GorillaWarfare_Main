using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyTrackAndMovement : MonoBehaviour
{
    #region Variables

    [Header("Floats")]
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float range = 10f;

    [Header("Bools")]
    [SerializeField] bool isGoingRight = true;
    public bool targetInRange;
    public bool targetObstructed;

    [Header("GameObjects")]
    GameObject player;
    
    [Header("Transforms")]
    Transform target;

    [Header("LayerMasks")]
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask obstructionMask;

    [Header("Components")]
    Rigidbody2D rb;
    Robot robot;

    #endregion

    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform.Find("Target").gameObject;
        robot = GetComponent<Robot>();
    }

    void FixedUpdate()
    {
        if (target)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            if (direction.x > 0f)
            {
                robot.isFlipped = false;
                transform.Translate(moveSpeed * Time.deltaTime * transform.right);
            }
            else if (direction.x < 0f)
            {
                robot.isFlipped = true;
                transform.Translate(moveSpeed * Time.deltaTime * -transform.right);
            }
        }
        else
        {
            Vector3 directionTranslation = isGoingRight ? transform.right : -transform.right;
            directionTranslation *= Time.deltaTime * moveSpeed;

            transform.Translate(directionTranslation);
            robot.isFlipped = !isGoingRight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        targetObstructed = IsObstructed();
        targetInRange = TargetInRange();

        if (!target)
        {
            FindTarget();
            return;
        }

        if (!TargetInRange() || IsObstructed())
        {
            target = null;
        }
    }

    #endregion

    #region Methods

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Obstruction"))
        {
            isGoingRight = !isGoingRight;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Obstruction"))
        {
            StartCoroutine(CheckDirection());
        }
    }

    void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, (Vector2) transform.position, 0f, playerMask);

        if ((hits.Length > 0) && TargetInRange() && !IsObstructed())
        {
            if (hits[0].transform.CompareTag("Player"))
            {
                target = player.transform;
            }
        }
        else
        {
            target = null;
        }
    }

    bool TargetInRange()
    {
        if (Vector2.Distance(player.transform.position, transform.position) <= range)
        {
            return true;
        }
        return false;
    }

    bool IsObstructed()
    {
        RaycastHit2D hit = Physics2D.Linecast(transform.position, player.transform.position, obstructionMask);

        if (hit)
        {
            // print("Target obstructed");
            return true;
        }
        // print("Target not obstructed");
        return false;
    }

    IEnumerator CheckDirection()
    {
        yield return new WaitForSeconds(2);

        isGoingRight = !isGoingRight;
    }

    #endregion

    #region Gizmos:

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);

        if (player)
        {
            Gizmos.DrawLine(transform.position, player.transform.position);
            Gizmos.color = Color.red;
        }

        if (target)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }

    #endregion
}
