using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ChangeLocaleScript : MonoBehaviour
{
    private int localeIndex;
    private void Start()
    {
        localeIndex = PlayerPrefs.GetInt(PrefsKey.Language);
        ChangeLocale(localeIndex);
    }

    public void ChangeLocale(int index)
    {
        if (index == localeIndex) return;

        StopAllCoroutines();
        
        localeIndex = index;
        PlayerPrefs.SetInt(PrefsKey.Language, index);
        StartCoroutine(Change(index));
    }

    private IEnumerator Change(int index)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
