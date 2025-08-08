using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public Button EffectsSoundButton;
    public Image EffectsSoundImage;
    public Slider EffectsSoundSlider;

    [SerializeField] private Sprite _EffectsOn;
    [SerializeField] private Sprite _EffectsOff;

    private void Awake()
    {
        EffectsSoundSlider.onValueChanged.AddListener(SerEffectsVolume);
        EffectsSoundButton.onClick.AddListener(SwitchEffectsSound);
    }

    public void SwitchEffectsSound()
    {
        if (EffectsSoundImage.sprite == _EffectsOn)
            EffectsSoundImage.sprite = _EffectsOff;
        else
            EffectsSoundImage.sprite = _EffectsOn;
    }

    public void EffectsSoundOn()
    {
        EffectsSoundImage.sprite = _EffectsOn;
    }

    public void EffectsSoundOff()
    {
        EffectsSoundImage.sprite = _EffectsOff;
    }

    public void SerEffectsVolume(float volume)
    {
        EffectsSoundSlider.value = volume;
        if (volume == 0)
            EffectsSoundOff();
        else
            EffectsSoundOn();
        GlobalEvents.OnSoundChange?.Invoke(volume);
    }
}
