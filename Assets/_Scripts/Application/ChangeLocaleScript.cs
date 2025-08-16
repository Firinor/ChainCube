using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ChangeLocaleScript : MonoBehaviour
{
    private int localeIndex;
    private IEnumerator Start()
    {
        yield return LocalizationSettings.InitializationOperation;
        localeIndex = PlayerPrefs.GetInt(PrefsKey.Language);
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
