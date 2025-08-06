using UnityEngine;

public class NormalCube : CubeCollideEffect
{
    public override void OnTriggerEnter(Cube cube, Collider other)
    {
        if (!cube.IsInGame
            || !other.CompareTag("Cube"))
            return;
        
        Cube otherCube = other.GetComponent<Cube>();
        
        if(otherCube.Rigidbody.isKinematic
            || otherCube.Score != cube.Score)
            return;

        cube.RemoveCube();

        GlobalEvents.OnMerge?.Invoke(cube, otherCube);
        
        otherCube.Score *= 2;
        if(otherCube.Score <= 4096)
            otherCube.CheckView();

        //CheckNeighbors
        otherCube.Trigger.enabled = false;
        otherCube.Trigger.enabled = true;
        
        otherCube.AddUpForce();
    }
}
