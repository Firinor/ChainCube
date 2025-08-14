using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    public Button EffectsSoundButton;
    public Image EffectsSoundImage;
    public Slider EffectsSoundSlider;

    [SerializeField] private Sprite _EffectsOn;
    [SerializeField] private Sprite _EffectsOff;

    public void Initialize()
    {
        EffectsSoundSlider.onValueChanged.AddListener(SetEffectsVolume);
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

    public void SetEffectsVolume(float volume)
    {
        EffectsSoundSlider.value = volume;
        if (volume == 0)
            EffectsSoundOff();
        else
            EffectsSoundOn();
        GlobalEvents.OnSoundChange?.Invoke(volume);
    }
}
