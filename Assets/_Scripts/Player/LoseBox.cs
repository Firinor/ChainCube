using UnityEngine;

public class LoseBox : MonoBehaviour
{
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floor;
    
    private void OnTriggerEnter(Collider other)
    {
        floor.material = loseMaterial;
        GlobalEvents.OnLose?.Invoke();//GAME OVER
    }
}
