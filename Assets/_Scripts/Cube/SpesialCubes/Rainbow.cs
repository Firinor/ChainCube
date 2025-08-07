using UnityEngine;

public class Rainbow : CubeCollideEffect
{
    public override void OnTriggerEnter(Cube cube, Collider other)
    {
        if (!cube.IsInGame) return;

        if (!(other.CompareTag("Cube") 
              || other.CompareTag("EndWall"))) return;
    
        Cube otherCube = other.GetComponent<Cube>();
        
        if (otherCube is null) return;
        
        if(otherCube.Score <= 0) return;
        
        GlobalEvents.OnMerge?.Invoke(otherCube, cube);
        otherCube.Score *= 2;
        otherCube.CheckView();
        otherCube.AddUpForce();
        cube.RemoveCube();
    }
}
