using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class BoomSound : MonoBehaviour
{
    public AudioClip boom;

    [SerializeField]
    private AudioSource source;

    [Inject]
    private void Instantiate()
    {
        GlobalEvents.OnSoundChange += ChangeVolume;
    }

    private void ChangeVolume(float volume)
    {
        source.volume = volume;
    }

    public void Play()
    {
        source.PlayOneShot(boom);
    }

    private void OnDestroy()
    {
        GlobalEvents.OnSoundChange -= ChangeVolume;
    }
}
