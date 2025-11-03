using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlyLootManager : MonoBehaviour
{
    public static FlyLootManager instance;

    [SerializeField] 
    private Transform parent;
    [SerializeField] 
    private FlyLoot prefab;
    [SerializeField] 
    private float spawnRadius;
    [SerializeField] 
    private float spawnTotalTime = 1f;

    [Header("Test")]
    [SerializeField] 
    private Sprite testSprite;
    [SerializeField] 
    private Transform testStartpoint;
    [SerializeField] 
    private Transform testEndpoint;
    [SerializeField] 
    private int testCount;

    private List<FlyLoot> lootPool = new();
    
    private void Awake()
    {
        instance = this;
    }

    public void AnimateGoods(List<FlyLootFilling> listLoot, Action lootCallback = null)
    {
        StartCoroutine(AnimateGoodsCoroutine(listLoot, lootCallback));
    }

    private IEnumerator AnimateGoodsCoroutine(List<FlyLootFilling> listLoot, Action lootCallback = null)
    {
        bool isOffset = listLoot.Count > 1;
        
        float timer = 0;
        float yieldDelay = spawnTotalTime/listLoot.Count;

        for (int i = 0; i < listLoot.Count; i++)
        {
            timer -= yieldDelay;
            while (timer < 0 && listLoot.Count > 1)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;

            Transform startPoint = listLoot[i].StartPosition;
            FlyLoot newGoods = Instantiate(prefab, startPoint.position , startPoint.rotation, parent);
            lootPool.Add(newGoods);
            if (isOffset)
            {
                newGoods.SetDestination(listLoot[i], randomOffset);
            }
            else
                newGoods.SetDestination(listLoot[i]);

            newGoods.OnPointerEnterAction += FlyToEnd;
        }
    }

    public void FlyToEnd()
    {
        foreach (FlyLoot loot in lootPool)
        {
            loot.FlyToEnd();
            loot.OnPointerEnterAction -= FlyToEnd;
        }
    }
    
    /*[ContextMenu(nameof(Test))]
    private void Test()
    {
        StartCoroutine(AnimateGoodsCoroutine(testSprite, testStartpoint, testEndpoint, testCount));
    }*/
}