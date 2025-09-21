using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlyGoodsUIManager : MonoBehaviour
{
    public static FlyGoodsUIManager instance;

    [SerializeField] 
    private Transform parent;
    [SerializeField] 
    private FlyGoods prefab;
    [SerializeField] 
    private float spawnRadius;
    [SerializeField] 
    private float delayBetweenSpawn;

    [Header("Test")]
    [SerializeField] 
    private Sprite testSprite;
    [SerializeField] 
    private Transform testEndpoint;
    [SerializeField] 
    private int testCount;
    private void Awake()
    {
        instance = this;
    }

    public void AnimateGoods(Sprite goods, Transform startPoint, Transform endPoint, int count = 1)
    {
        StartCoroutine(AnimateGoodsCoroutine(goods, startPoint, endPoint, count));
    }

    private IEnumerator AnimateGoodsCoroutine(Sprite goods, Transform startPoint, Transform endPoint, int count = 1)
    {
        bool isOffset = count > 1;
        WaitForSeconds yieldDelay = new WaitForSeconds(delayBetweenSpawn);
        
        for (int i = 0; i < count; i++)
        {
            Vector3 randomOffset = new Vector3(
                            Random.Range(-spawnRadius, spawnRadius),
                            Random.Range(-spawnRadius, spawnRadius),
                            0);
            
            FlyGoods newGoods = Instantiate(prefab, startPoint.position , Quaternion.identity, parent);
            if (isOffset)
            {
                newGoods.SetDestination(goods, endPoint, randomOffset);
            }
            else
                newGoods.SetDestination(goods, endPoint);

            yield return yieldDelay;
        }
    }

    [ContextMenu(nameof(Test))]
    private void Test()
    {
        StartCoroutine(AnimateGoodsCoroutine(testSprite, transform, testEndpoint, testCount));
    }
}