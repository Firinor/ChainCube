using UnityEngine;

public class NormalCube : CubeCollideEffect
{
    public override void OnTriggerEnter(Cube cube, Collider other)
    {
        if (!cube.IsInGame) return;

        if (!other.CompareTag("Cube"))
            return;
        
        Cube otherCube = other.GetComponent<Cube>();
        if (otherCube.Score != cube.Score)
            return;

        cube.OnDestroy?.Invoke(cube);

        otherCube.Score *= 2;
        if(otherCube.Score <= 4096)
            otherCube.RefreshMaterial();

        //CheckNeighbors
        otherCube.Trigger.enabled = false;
        otherCube.Trigger.enabled = true;
        otherCube.AddUpForce();

        Cube.OnMerge?.Invoke(cube.Score);
    }
}
