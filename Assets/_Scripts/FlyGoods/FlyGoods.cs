using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FlyGoods : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] 
    private Image image;
    [SerializeField] 
    private Vector3 offset;
    [SerializeField] 
    private AnimationCurve moveCurve;
    [SerializeField] 
    private AnimationCurve YCurve;
    [SerializeField]
    private float lyingDuration;
    
    private Vector3 startPosition;
    
    private float elapsedTime;
    private float flyDuration;

    private bool secondStep;
    
    public void SetDestination(Sprite goods, Transform endPoint, Vector3 offset = default)
    {
        image.sprite = goods;
        this.offset = offset;
        startPosition = transform.position;
        transform.position = endPoint.position;
        image.transform.position = startPosition;

        flyDuration = moveCurve.keys.Last().time;
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
            elapsedTime
        ) + Vector3.up * YValue;
        
        if (elapsedTime > lyingDuration)
            OnPointerEnter(null);
    }

    private void Retraction()
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / flyDuration;
        if (t >= 1)
        {
            Destroy(gameObject);
            return;
        }
        
        float moveValue = moveCurve.Evaluate(t);
        
        image.transform.position = Vector3.LerpUnclamped(
            startPosition+offset, 
            transform.position, 
            moveValue
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!secondStep)
            elapsedTime = 0;
        secondStep = true;
    }
}