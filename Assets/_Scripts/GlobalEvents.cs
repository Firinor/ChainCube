
using System;

public static class GlobalEvents
{
    public static Action OnLose;
    public static Action<Cube, Cube> OnMerge;
    public static Action<string> OnLanguageChange;
    public static Action<float> OnSoundChange;
}

public static class PrefsKey
{
    public static string Sound = "Sound";
    public static string Language = "Language";
    public static string PersonalBestScore = "Score";
    public static string PlayerName = "PlayerName";
}