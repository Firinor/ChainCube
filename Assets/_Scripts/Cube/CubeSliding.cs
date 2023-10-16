using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Cube))]
public class CubeSliding : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SlidingOn();
    }

    public void SlidingOn()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionX 
            | RigidbodyConstraints.FreezePositionY 
            | RigidbodyConstraints.FreezeRotation;
    }
    private void SlidingOff()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.None;
    }

    private void OnCollisionEnter(Collision collision)
    {
        SlidingOff();
        Destroy(this);
    }
}
