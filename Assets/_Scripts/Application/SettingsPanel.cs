using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public Button BackgroundSoundButton;
    public Image BackgroundSoundImage;
    public Slider BackgroundSoundSlider;
    public Button EffectsSoundButton;
    public Image EffectsSoundImage;
    public Slider EffectsSoundSlider;

    [SerializeField] private Sprite _BackgroundOn;
    [SerializeField] private Sprite _BackgroundOff;
    [SerializeField] private Sprite _EffectsOn;
    [SerializeField] private Sprite _EffectsOff;

    public void SwitchBackgroundSound()
    {
        if (BackgroundSoundImage.sprite == _BackgroundOn)
            BackgroundSoundImage.sprite = _BackgroundOff;
        else
            BackgroundSoundImage.sprite = _BackgroundOn;
    }
    public void BackgroundSoundOn()
    {
        BackgroundSoundImage.sprite = _BackgroundOn;
    }
    public void BackgroundSoundOff()
    {
        BackgroundSoundImage.sprite = _BackgroundOff;
    }
    public void SerBackgroundVolume(float volume)
    {
        BackgroundSoundSlider.value = volume;
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
    }
}
