using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using YG;

public class ChangeLocaleScript : MonoBehaviour
{
    private int localeIndex;
    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;
        if (PlayerPrefs.HasKey(PrefsKey.Language))
        {
            localeIndex = PlayerPrefs.GetInt(PrefsKey.Language);
            Change(localeIndex);
            yield break;
        }

        switch (YG2.envir.language)
        {
            case "ru":
                localeIndex = 1;
                break;
            case "tr":
                localeIndex = 2;
                break;
            default:
                localeIndex = 0;
                break;
        }

        Change(localeIndex);
    }

    public void ChangeLocale(int index)
    {
        if (index == localeIndex) return;

        StopAllCoroutines();
        
        localeIndex = index;
        PlayerPrefs.SetInt(PrefsKey.Language, index);
        Change(index);
    }

    private void Change(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
