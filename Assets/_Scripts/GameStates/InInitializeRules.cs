using System;
using UnityEngine;
using YG;
using YG.Utils.LB;
using Zenject;
using Random = UnityEngine.Random;

public class InInitializeRules : IState
{
    [Inject]
    private Player player;
    [Inject]
    private SettingsPanel settings;

    public void Enter()
    {
        //LoadPlayerName();
        LoadSoundSettings();
        //LoadLanguage();
    }

    private void LoadPlayerName()
    {
        if (!PlayerPrefs.HasKey(PrefsKey.PlayerName))
            PlayerPrefs.SetString(PrefsKey.PlayerName, "player" + (int)(Random.value*1000));

        //scorePanel.PlayerNameInputField.text = PlayerPrefs.GetString(PrefsKey.PlayerName);
    }

    private void LoadLanguage()
    {
        if (!PlayerPrefs.HasKey(PrefsKey.Language))
            PlayerPrefs.SetString(PrefsKey.Language, "en");
    }

    private void LoadSoundSettings()
    {
        if (!PlayerPrefs.HasKey(PrefsKey.Sound))
            PlayerPrefs.SetFloat(PrefsKey.Sound, .5f);
        settings.Initialize();
        settings.SetEffectsVolume(PlayerPrefs.GetFloat(PrefsKey.Sound), isNeedSaveVolume: false);
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        
    }
}