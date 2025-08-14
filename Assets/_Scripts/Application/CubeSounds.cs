using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class CubeSounds : MonoBehaviour
{
    public AudioClip[] pop;

    [SerializeField]
    private AudioSource source;

    [Inject]
    private void Instantiate()
    {
        GlobalEvents.OnMerge += MoveSourse;
        GlobalEvents.OnSoundChange += ChangeVolume;
    }

    private void ChangeVolume(float volume)
    {
        source.volume = volume;
    }

    private void MoveSourse(Cube c1, Cube c2)
    {
        transform.position = c1.transform.position;
        Play();
    }
    
    public void Play()
    {
        int random = Random.Range(0, pop.Length);
        source.PlayOneShot(pop[random]);
    }
    
    private void OnDestroy()
    {
        GlobalEvents.OnMerge -= MoveSourse;
        GlobalEvents.OnSoundChange -= ChangeVolume;
    }
}
