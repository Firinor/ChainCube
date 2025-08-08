using UnityEngine;

public class CubeDeadZone : MonoBehaviour
{
    [SerializeField]
    private Collider deathCollider;
    private void OnTriggerExit(Collider other)
    {
        Cube cube = other.GetComponent<Cube>();
        
        if(cube is null) return;
        
        if(deathCollider.bounds.Contains(other.transform.position)) return;
         
        cube.RemoveCube();
    }
}
