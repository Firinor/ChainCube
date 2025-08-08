
using System;

public static class GlobalEvents
{
    public static Action OnLose;
    public static Action<Cube, Cube> OnMerge;
    public static Action<string> OnLanguageChange;
    public static Action<float> OnSoundChange;
}
