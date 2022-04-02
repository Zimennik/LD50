using UnityEngine;

//Objects with this script will be affected by black hole force
//When object is inside black hole radius (by default it's 0.5f), it will be affected by black hole force
//object will be pulled towards black hole center
//the closer to black hole radius, the more force will be applied
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public abstract class AffectedByBlackHole : MonoBehaviour
{
    protected BlackHoleController blackHoleController;
    protected float pullDistance = 3;
    protected float force;

    protected Rigidbody rb;
    protected Collider collider;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        blackHoleController = GameManager.Instance.blackHoleController;
    }

    protected virtual void FixedUpdate()
    {
        ApplyForce();
    }

    //if distance multiplied by black hole size is less than pull distance, pull object
    //the less distance, the more force will be applied
    protected virtual void ApplyForce()
    {
        if (Vector3.Distance(transform.position, blackHoleController.transform.position) *
            blackHoleController.currentSize < pullDistance)
        {
            force = blackHoleController.currentSize * blackHoleController.currentSize /
                    (Vector3.Distance(transform.position, blackHoleController.transform.position) *
                     Vector3.Distance(transform.position, blackHoleController.transform.position));
            rb.AddForce((blackHoleController.transform.position - transform.position) * force);
        }
    }
}