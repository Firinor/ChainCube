using UnityEngine;
using YG;
using YG.Insides;

public class YandexADS : MonoBehaviour
{
    [SerializeField] private float Timer;

    [ContextMenu("Pause")]
    void Pause()
    {
        YG2.PauseGame(!YG2.isPauseGame);   
    }
    
    [ContextMenu("ResetAdsTimer")]
    void ResetAdsTimer()
    {
        YGInsides.ResetTimerInterAdv();
    }

    private void Update()
    {
        Timer = YG2.timerInterAdv;
    }
}
