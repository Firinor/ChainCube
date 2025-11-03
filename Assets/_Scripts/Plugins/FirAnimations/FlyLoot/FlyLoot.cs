using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class FlyLoot : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] 
    private Image image;
    private Vector3 offset;
    [SerializeField] 
    private AnimationCurve sizeCurve;
    [SerializeField] 
    private AnimationCurve moveCurve;
    [SerializeField] 
    private AnimationCurve YCurve;
    [SerializeField]
    private float lyingDuration;
    
    private Vector3 startPosition;
    
    private float elapsedTime;
    private float dropDuration;
    private float flyDuration;

    public event Action OnPointerEnterAction;
    private FlyLootFilling filling;
    private bool secondStep;
    
    public void SetDestination(FlyLootFilling filling, Vector2 offset = default)
    {
        this.filling = filling;
        image.sprite = filling.Sprite;
        this.offset = offset;
        startPosition = transform.position;
        transform.position = filling.EndPosition.position;
        image.transform.position = startPosition;
        
        image.transform.localScale = Vector3.one * sizeCurve.keys[0].value;
        
        flyDuration = moveCurve.keys.Last().time;
        dropDuration = YCurve.keys.Last().time;
    }

    private void Update()
    {
        if (secondStep)
            Retraction();
        else
            Prolapse();
    }

    private void Prolapse()
    {
        elapsedTime += Time.deltaTime;

        float YValue = YCurve.Evaluate(elapsedTime);

        image.transform.position = Vector3.Lerp(
            startPosition, 
            startPosition+offset, 
            elapsedTime / dropDuration
        ) + Vector3.up * YValue;
        
        if (elapsedTime > lyingDuration)
            FlyToEnd();
    }

    private void Retraction()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / flyDuration;
        if (t >= 1)
        {
            filling.OnEndCallback?.Invoke();
            Destroy(gameObject);
            return;
        }
        
        float moveValue = moveCurve.Evaluate(t);
        
        image.transform.position = Vector3.LerpUnclamped(
            startPosition+offset, 
            transform.position, 
            moveValue
        );
        
        float sizeValue = sizeCurve.Evaluate(t);
        image.transform.localScale = Vector3.one*sizeValue;
    }

    public void FlyToEnd()
    {
        if(secondStep) 
            return;

        image.raycastTarget = false;
        elapsedTime = 0;
        secondStep = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(secondStep) 
            return;
        
        OnPointerEnterAction?.Invoke();
    }
}

[Serializable]
public struct FlyLootFilling
{
    public Sprite Sprite;
    public Transform StartPosition;
    public Transform EndPosition;
    public Action OnEndCallback;
}