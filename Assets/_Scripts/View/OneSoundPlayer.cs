using UnityEngine;
using Zenject;

[RequireComponent(typeof(AudioSource))]
public class OneSoundPlayer : MonoBehaviour
{
    public AudioClip clip;

    [SerializeField]
    private AudioSource source;

    [Inject] private SceneEvents events;
    [Inject]
    private void Instantiate()
    {
        events.OnSoundChange += ChangeVolume;
        ChangeVolume(PlayerPrefs.GetFloat(PrefsKey.Sound));
    }

    private void ChangeVolume(float volume)
    {
        source.volume = volume;
    }

    public void Play()
    {
        source.PlayOneShot(clip);
    }

    private void OnDestroy()
    {
        events.OnSoundChange -= ChangeVolume;
    }
}