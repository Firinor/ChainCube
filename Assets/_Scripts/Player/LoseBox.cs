using UnityEngine;
using Zenject;

public class LoseBox : MonoBehaviour
{
    [SerializeField] 
    private Material loseMaterial;
    [SerializeField] 
    private MeshRenderer floor;
    [Inject] 
    private SceneEvents events;
    
    private void OnTriggerEnter(Collider other)
    {
        floor.material = loseMaterial;
        events.OnLose?.Invoke();//GAME OVER
    }
}
