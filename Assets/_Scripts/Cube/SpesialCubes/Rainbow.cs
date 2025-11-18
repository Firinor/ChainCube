using UnityEngine;

public class Rainbow : CubeCollideEffect
{
    public Rainbow(SceneEvents events) : base(events) { }
    
    public override void OnTriggerEnter(Cube cube, Collider other)
    {
        if (!cube.IsInGame) 
            return;

        if (!(other.CompareTag("Cube") 
              || other.CompareTag("EndWall"))) 
            return;
    
        Cube otherCube = other.GetComponent<Cube>();
        
        if (otherCube is null) 
            return;
        
        if(otherCube.Score <= 0) 
            return;
        
        cube.RemoveCube();
        events.OnMerge?.Invoke(otherCube, cube);
        
        if(otherCube.Score < 4096)
            otherCube.Score *= 2; 
        
        otherCube.CheckView();
        otherCube.AddUpForce();
        
    }
}
