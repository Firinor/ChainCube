using TMPro;
using UnityEngine;

namespace FirAnimations
{
    public static class FirAnimationsExtensions
    {
        public static void PlayFirTextAnimation(this TextMeshProUGUI textMesh, FirTextAnimationData data)
        {
            FirTextFontSizeAnimation animation = textMesh.gameObject.GetComponent<FirTextFontSizeAnimation>();
            if(animation is null) 
                animation = textMesh.gameObject.AddComponent<FirTextFontSizeAnimation>();
            animation.OnComplete = null;
                
            //animation.text = textMesh;
            textMesh.text = data.Text;
            animation.Curve = data.LifeLine;
            animation.EndPosition = data.MaxFontSize;
            animation.Initialize();
            animation.OnComplete += () =>
            {
                textMesh.enabled = false;
            };
            if(data.OnEnd is not null)
                animation.OnComplete += data.OnEnd;
            textMesh.enabled = true;
            animation.Play();
        }
    }
}