using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIGradient : MonoBehaviour 
{
    [SerializeField]
    private Color topColor = Color.white;
    [SerializeField]
    private Color bottomColor = Color.black;
    [SerializeField]
    private GradientDirection direction = GradientDirection.Vertical;
    [SerializeField]
    private Image image;
    
    private enum GradientDirection {
        Vertical,
        Horizontal
    }

    private void Awake() {
        image = GetComponent<Image>();
        UpdateGradient();
        Destroy(this);
    }

    private void UpdateGradient() {
        if (image == null) return;
        
        Texture2D texture = new Texture2D(1, 2);
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;
        
        if (direction == GradientDirection.Vertical) {
            texture.SetPixel(0, 0, topColor);
            texture.SetPixel(0, 1, bottomColor);
        } else {
            texture.SetPixel(0, 0, topColor);
            texture.SetPixel(1, 0, bottomColor);
        }
        
        texture.Apply();
        image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }

    private void OnValidate() {
        UpdateGradient();
    }
}