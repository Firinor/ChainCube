using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class CubeSounds : MonoBehaviour
{
    public AudioClip[] pop;

    [SerializeField]
    private AudioSource source;

    private void Awake()
    {
        GlobalEvents.OnMerge += (c1, c2) =>
        {
            transform.position = c1.transform.position;
            Play();
        };
    }

    public void Play()
    {
        int random = Random.Range(0, pop.Length);
        source.PlayOneShot(pop[random]);
    }
}
