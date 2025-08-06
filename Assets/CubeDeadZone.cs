using UnityEngine;

public class CubeDeadZone : MonoBehaviour
{
    [SerializeField]
    private Collider collider;
    private void OnTriggerExit(Collider other)
    {
        Cube cube = other.GetComponent<Cube>();
        
        if(cube is null) return;
        
        if(collider.bounds.Contains(other.transform.position)) return;
         
        cube.RemoveCube();
    }
}
