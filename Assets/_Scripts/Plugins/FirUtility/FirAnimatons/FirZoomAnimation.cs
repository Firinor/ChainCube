using System;
using UnityEngine;

namespace FirAnimations
{
    [RequireComponent(typeof(RectTransform))]
    public class FirZoomAnimation : MonoBehaviour, IFirAnimation
    {
        [Range(0, 1)]
        public float Time;
        public Vector3 StartZoom;
        public Vector3 EndZoom;
        public AnimationCurve Curve = AnimationCurve.EaseInOut(0,0,1,1);
        
        public Action OnComplete;

        private float _endTime;
        private Vector3 delta;

        private RectTransform rectTransform;
        private RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }

                return rectTransform;
            }
        }

        private void OnValidate()
        {
            MoveByDelta();
        }
        
        public void Initialize()
        {
            Stop();
            delta = EndZoom - StartZoom;
            _endTime = Curve.keys[Curve.length-1].time;
            ToStartPoint();
        }
        
        public void Play() => enabled = true;
        public void Stop()
        {
            ToEndPoint();
            enabled = false;
        }

        [ContextMenu("ToStartPoint")]
        public void ToStartPoint()
        {
            RectTransform.localScale = StartZoom;
            Time = 0;
        }
        [ContextMenu("ToEndPoint")]
        public void ToEndPoint()
        {
            Time = 1;
            MoveByDelta();
        }
        
        public void Update()
        {
            if (Time >= 1)
            {
                Time = 1;
                enabled = false;
                OnComplete?.Invoke();
                return;
            }
            
            Time += UnityEngine.Time.unscaledDeltaTime/_endTime;
            MoveByDelta();
        }

        private void MoveByDelta()
        {
            float curveValue = Curve.Evaluate(Time*_endTime);
            RectTransform.localScale = StartZoom + (delta * curveValue);
        }
    }
}