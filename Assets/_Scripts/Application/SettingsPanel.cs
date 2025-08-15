using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsPanel : MonoBehaviour
{
    public Button EffectsSoundButton;
    public Image EffectsSoundImage;
    public Slider EffectsSoundSlider;

    [SerializeField] private Sprite _EffectsOn;
    [SerializeField] private Sprite _EffectsOff;

    [Inject] 
    private SceneEvents events;

    public void Initialize()
    {
        EffectsSoundSlider.onValueChanged.AddListener(f=>SetEffectsVolume(f));
        EffectsSoundButton.onClick.AddListener(SwitchEffectsSound);
    }

    public void SwitchEffectsSound()
    {
        if (EffectsSoundImage.sprite == _EffectsOn)
        {
            EffectsSoundImage.sprite = _EffectsOff;
            SetEffectsVolume(0);
        }
        else
        {
            EffectsSoundImage.sprite = _EffectsOn;
            SetEffectsVolume(0.5f);
        }
    }

    private void EffectsSoundOn()
    {
        EffectsSoundImage.sprite = _EffectsOn;
    }

    private void EffectsSoundOff()
    {
        EffectsSoundImage.sprite = _EffectsOff;
    }

    public void SetEffectsVolume(float volume, bool isNeedSaveVolume = true)
    {
        EffectsSoundSlider.value = volume;
        if (volume == 0)
            EffectsSoundOff();
        else
            EffectsSoundOn();
        events.OnSoundChange?.Invoke(volume);
        
        if(isNeedSaveVolume)
            PlayerPrefs.SetFloat(PrefsKey.Sound, volume);
    }
}
