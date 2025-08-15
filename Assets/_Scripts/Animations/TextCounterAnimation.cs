using System;
using TMPro;
using UnityEngine;

namespace FirAnimations
{
    public class TextCounterAnimation : MonoBehaviour, IFirAnimation
    {
        public int StartNumber;
        public int EndNumber;
        public string Pattern = "{0}";
        
        public TextMeshProUGUI textComponent;
        public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public Action OnComplete;

        private float _time;
        private float _endTime;

        private int delta;

        public void Initialize()
        {
            Stop();
            OnComplete = null;
            delta = EndNumber - StartNumber;
            _endTime = Curve.keys[Curve.length - 1].time;
            ToStartPoint();
        }

        public void Play() => enabled = true;
        public void Stop() => enabled = false;

        [ContextMenu("ToStartPoint")]
        public void ToStartPoint()
        {
            textComponent.text = String.Format(Pattern, StartNumber); 
            _time = 0;
        }

        [ContextMenu("ToEndPoint")]
        public void ToEndPoint()
        {
            textComponent.text = String.Format(Pattern, EndNumber); 
            _time = _endTime;
        }

        public void Update()
        {
            _time += Time.deltaTime;
            float curveValue = Curve.Evaluate(_time);
            textComponent.text = String.Format(Pattern, StartNumber+(int)(delta*curveValue));

            if (_time >= _endTime)
            {
                enabled = false;
                OnComplete?.Invoke();
            }
        }
    }
}