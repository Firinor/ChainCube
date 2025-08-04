using System;
using UnityEngine;

public class LoseBox : MonoBehaviour
{
    public Action OnLose;

    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floor;
    
    private void OnTriggerEnter(Collider other)
    {
        floor.material = loseMaterial;
        OnLose?.Invoke();//GAME OVER
    }
}
