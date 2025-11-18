using System;
using System.Collections.Generic;
using FirAnimations;
using UnityEngine;
using UnityEngine.UI;
using YG;
using Zenject;

public class RewardView : MonoBehaviour
{
    [Inject] 
    private Player player;
    
    public FirAnimationsManager Animator;
    public Sprite ChestSprite;
    public Image Icon;

    public Transform RainbowTransform;
    public Transform BombTransform;
    public Transform GhostTransform;

    public OneSoundPlayer sound;

    private void Awake()
    {
        Animator.Initialize();
    }

    public void SetReward(List<SpecialCube> cubids)
    {
        Icon.sprite = ChestSprite;
        gameObject.SetActive(true);
        Animator.OnEndAllAnimations += () => FlyGoods(cubids, isPremium: true);
        sound.Play();
        Animator.StartAnimations();
    }

    public void SetReward(SpecialCube cubid)
    {
        Icon.sprite = cubid.Sprite;
        gameObject.SetActive(true);
        Animator.OnEndAllAnimations += () => FlyGoods(new List<SpecialCube>(){ cubid });
        sound.Play();
        Animator.StartAnimations();
    }

    public void PlayerClick()
    {
        if (Animator.enabled)
        {
            Animator.ToEndPoint();
            return;
        }
        Animator.OnEndAllAnimations = null;
        gameObject.SetActive(false);
    }

    private void FlyGoods(List<SpecialCube> cubids, bool isPremium = false)
    {
        List<FlyLootFilling> loots = new();
        foreach (var cubid in cubids)
        {
            FlyLootFilling lootFilling = new();
            lootFilling.Sprite = cubid.Sprite;
            lootFilling.StartPosition = Icon.transform;
            if (cubid.Index == 0)
            {
                lootFilling.EndPosition = RainbowTransform;
                lootFilling.OnEndCallback = () => player.RainbowCount.Value++;
                if(isPremium)
                    YG2.saves.RainbowBonus++;
            }
            else if (cubid.Index == 1)
            {
                lootFilling.EndPosition = BombTransform;
                lootFilling.OnEndCallback = () => player.BombCount.Value++;
                if(isPremium)
                    YG2.saves.BombBonus++;
            }
            else if (cubid.Index == 2)
            {
                lootFilling.EndPosition = GhostTransform;
                lootFilling.OnEndCallback = () => player.GhostCount.Value++;
                if(isPremium)
                    YG2.saves.GhostBonus++;
            }
            else
                throw new Exception("What Transform?");
            
            loots.Add(lootFilling);
        }
        
        if(isPremium)
            YG2.SaveProgress();
        
        Animator.OnEndAllAnimations = null;
        gameObject.SetActive(false);
        FlyLootManager.instance.AnimateGoods(loots);
    }
}
