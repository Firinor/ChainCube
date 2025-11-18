using System;
using System.Collections.Generic;
using UnityEngine;
using YG;
using YG.Insides;

public class YandexADS : MonoBehaviour
{
    [SerializeField] private float Timer;
    [SerializeField] private RewardView rewardView;
    [SerializeField] private LootBox lootBox;

    private void Awake()
    {
        YG2.onPurchaseSuccess += SuccessPurchased;
    }

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

    public void ShowRewarded()
    {
        Pause();
        YG2.RewardedAdvShow("randomBonus", RewardedBonus);
    }

    private void RewardedBonus()
    {
        YG2.SkipNextInterAdCall();
        SpecialCube loot = lootBox.GetRandomItem<SpecialCube>();
        rewardView.SetReward(loot);
    }
    
    private void SuccessPurchased(string ID)
    {
        switch (ID)
        {
            case "CubidsBonus":
            {
                List<SpecialCube> loot = lootBox.GetRandomItems<SpecialCube>(count: 10);
                rewardView.SetReward(loot);
                break;
            }
            default:
                throw new Exception("Unknown Purchase ID!");
        }
    }

    private void OnDestroy()
    {
        YG2.onPurchaseSuccess -= SuccessPurchased;
    }
}