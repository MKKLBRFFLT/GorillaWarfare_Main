using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    #region Variables

    [Header("Ints")]
    [SerializeField] int numFound;

    [Header("Floats")]
    readonly float radius = 0.5f;

    [Header("Bools")]
    public bool promtFound;

    [Header("Arrays")]
    public readonly Collider2D[] colliders = new Collider2D[3];

    [Header("LayerMasks")]
    [SerializeField] LayerMask interactableMask; // SerializeField is Important!

    #endregion

    #region StartUpdate

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        numFound = Physics2D.OverlapCircleNonAlloc(transform.position, radius, colliders, interactableMask);

        if (numFound > 0)
        {
            var interactable = colliders[0].GetComponent<IInteractable>();
            if (interactable != null && Input.GetKeyDown(KeyCode.E))
            {
                interactable.Interact(this);
            }

            promtFound = true;
        }
        else
        {
            promtFound = false;
        }
    }

    #endregion

    #region Gizmos

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    #endregion
}
