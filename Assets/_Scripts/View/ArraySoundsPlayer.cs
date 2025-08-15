using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class ArraySoundsPlayer : MonoBehaviour
{
    public AudioClip[] pop;

    [SerializeField]
    private AudioSource source;
    [Inject] 
    private SceneEvents events;

    [Inject]
    private void Instantiate()
    {
        events.OnMerge += MoveSourse;
        events.OnSoundChange += ChangeVolume;
        ChangeVolume(PlayerPrefs.GetFloat(PrefsKey.Sound));
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
        events.OnMerge -= MoveSourse;
        events.OnSoundChange -= ChangeVolume;
    }
}
