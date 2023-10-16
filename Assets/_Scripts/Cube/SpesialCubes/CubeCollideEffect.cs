using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CubeCollideEffect
{
    public abstract void OnTriggerEnter(Cube cube, Collider collider);
}
