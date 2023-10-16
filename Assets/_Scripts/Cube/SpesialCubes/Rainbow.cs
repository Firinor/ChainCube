using UnityEngine;

public class Rainbow : CubeCollideEffect
{
    public override void OnTriggerEnter(Cube cube, Collider other)
    {
        if (!cube.IsInGame)
            return;

        if (other.tag == "Cube" || other.tag == "EndWall")
        {
            Cube otherCube = other.GetComponent<Cube>();
            if (otherCube != null) 
            {
                otherCube.Score *= 2;
                otherCube.RefreshMaterial();
                otherCube.AddUpForce();
                Cube.OnMerge?.Invoke(otherCube.Score);
            }

            cube.IsInGame = false;
            cube.gameObject.SetActive(false);
        }
    }
}
