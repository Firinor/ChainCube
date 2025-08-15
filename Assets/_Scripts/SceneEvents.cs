
using System;
using UnityEngine;

public class SceneEvents : MonoBehaviour
{
    public Action OnLose;
    public Action<Cube, Cube> OnMerge;
    public Action<string> OnLanguageChange;
    public Action<float> OnSoundChange;
}