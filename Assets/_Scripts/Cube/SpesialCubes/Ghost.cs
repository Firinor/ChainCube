using UnityEngine;

public class Ghost : CubeCollideEffect
{
    private Collider _collider;
    public Ghost(Collider collider)
    {
        _collider = collider;
        _collider.enabled = false;
    }
    public override void OnTriggerEnter(Cube cube, Collider other)
    {
        if (!cube.IsInGame)
            return;

        if (other.tag != "EndWall")
            return;
        
        cube.CollideEffect = new NormalCube();
        _collider.enabled = true;
    }
}