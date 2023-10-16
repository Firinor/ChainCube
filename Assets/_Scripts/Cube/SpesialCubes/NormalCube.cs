using UnityEngine;

public class NormalCube : CubeCollideEffect
{
    public override void OnTriggerEnter(Cube cube, Collider other)
    {
        if (!cube.IsInGame)
            return;

        if (other.tag == "Cube")
        {
            Cube otherCube = other.GetComponent<Cube>();
            if (otherCube.Score == cube.Score)
            {
                otherCube.IsInGame = false;
                otherCube.gameObject.SetActive(false);
                cube.Score *= 2;
                if(cube.Score <= 4096)
                    cube.RefreshMaterial();
                cube.AddUpForce();
                Cube.OnMerge?.Invoke(cube.Score);
            }
        }
    }
}
