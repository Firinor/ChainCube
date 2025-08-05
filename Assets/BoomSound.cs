using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BoomSound : MonoBehaviour
{
    public AudioClip boom;

    [SerializeField]
    private AudioSource source;

    public void Play()
    {
        source.PlayOneShot(boom);
    }
}
