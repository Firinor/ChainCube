using UnityEngine;

public class Rainbow : CubeCollideEffect
{
    public override void OnTriggerEnter(Cube cube, Collider other)
    {
        //if (!cube.IsInGame) return;

        if (other.tag == "Cube" || other.tag == "EndWall")
        {
            Cube otherCube = other.GetComponent<Cube>();
            if (otherCube != null) 
            {
                otherCube.Score *= 2;
                otherCube.CheckView();
                otherCube.AddUpForce();
                GlobalEvents.OnMerge?.Invoke(cube, otherCube);
            }
            
            cube.gameObject.SetActive(false);
        }
    }
}
