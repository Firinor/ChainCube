using UnityEngine;

public class Ghost : CubeCollideEffect
{
    private Cubid cubid;
    private CubeFactoryWithPool factory;

    public Ghost(Cubid cubid, CubeFactoryWithPool factory, SceneEvents events) : base(events)
    {
        this.factory = factory;
        this.cubid = cubid;
    }
    public override void OnTriggerEnter(Cube cube, Collider other)
    {
        if (other.tag != "EndWall")
            return;
        
        cube.Rigidbody.velocity = Vector3.zero;
        cube.Collider.enabled = true;
        cube.Score = (int)cubid.Score;
        factory.RefreshView(cube);
        cube.Trigger.enabled = false;
        cube.CollideEffect = new NormalCube(events);
        cube.Trigger.enabled = true;
    }
}