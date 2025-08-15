using UnityEngine;

public abstract class CubeCollideEffect
{
    public SceneEvents events;

    public CubeCollideEffect(SceneEvents events)
    {
        this.events = events;
    }
    public abstract void OnTriggerEnter(Cube cube, Collider collider);
}
