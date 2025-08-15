using System;
using UnityEngine;

public class Bomb : CubeCollideEffect
{
    private float explosionRadius = 2.6f;
    public Action<Vector3> BoomAction;

    public Bomb(SceneEvents events) : base(events)
    {
        BoomAction += Explode;
    }
    public override void OnTriggerEnter(Cube cube, Collider other)
    {
        if (!cube.IsInGame) return;

        if (other.CompareTag("Cube") 
            || other.CompareTag("EndWall"))
        {
            Cube otherCube = other.GetComponent<Cube>();
            if (otherCube != null)
                otherCube.RemoveCube();

            BoomAction?.Invoke(cube.transform.position + cube.GetComponent<SphereCollider>().center);
            cube.RemoveCube();
        }
    }

    void Explode(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Cube other = nearbyObject.GetComponent<Cube>();
            if (other is not null)
                other.AddUpForce();
        }
    }
}
