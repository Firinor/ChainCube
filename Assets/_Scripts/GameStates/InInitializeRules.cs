using UnityEngine;
using Zenject;

public class InInitializeRules : IState
{
    [Inject]
    private Player player;
    [Inject]
    private SettingsPanel settings;
    [Inject]
    private NewBestScorePanel scorePanel;
    
    public void Enter()
    {
        LoadPlayerName();
        LoadSoundSettings();
        LoadLanguage();
        LoadHighscore();
    }

    private void LoadPlayerName()
    {
        if (!PlayerPrefs.HasKey(PrefsKey.PlayerName))
            PlayerPrefs.SetString(PrefsKey.PlayerName, "player" + Random.value*1000);
    }

    private void LoadHighscore()
    {
        if (!PlayerPrefs.HasKey(PrefsKey.PersonalBestScore))
            PlayerPrefs.SetInt(PrefsKey.PersonalBestScore, 2048);

        player.oldRecord = PlayerPrefs.GetInt(PrefsKey.PersonalBestScore);
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
        settings.SetEffectsVolume(PlayerPrefs.GetFloat(PrefsKey.Sound));
    }

    public void Exit()
    {
        
    }

    public void Tick()
    {
        
    }
}