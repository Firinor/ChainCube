using System.Collections.Generic;
using UnityEngine;

public class LoseBox : MonoBehaviour
{
    public bool IsLose => cubes.Count > 0;

    private List<Cube> cubes = new();

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Cube cube))
        {
            cubes.Add(cube);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Cube cube))
        {
            cubes.Remove(cube);
        }
    }
}
