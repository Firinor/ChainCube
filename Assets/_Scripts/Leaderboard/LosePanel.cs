using UnityEngine;

public class LosePanel : MonoBehaviour
{
    [SerializeField] 
    public OneSoundPlayer sound;
    private void OnEnable()
    {
        sound.Play();
    }
}