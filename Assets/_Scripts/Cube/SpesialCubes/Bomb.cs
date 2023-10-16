using UnityEngine;

public class Bomb : CubeCollideEffect
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
                otherCube.IsInGame = false;
                otherCube.gameObject.SetActive(false);
                Cube.OnMerge?.Invoke(otherCube.Score);
            }

            //TODO: Create boom effect
            cube.IsInGame = false;
            cube.gameObject.SetActive(false);
        }
    }
}
