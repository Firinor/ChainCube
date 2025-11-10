using TMPro;
using UnityEngine;

namespace FirAnimations
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FirTextNumeratorAnimation : FirAnimation
    {
        public int StartPosition;
        public int EndPosition;
        private int delta => EndPosition - StartPosition;

        private TextMeshProUGUI _text;
        private TextMeshProUGUI text
        {
            get
            {
                if (_text == null)
                {
                    _text = GetComponent<TextMeshProUGUI>();
                }

                return _text;
            }
        }
        private void Reset()
        {
            int.TryParse(text.text, out int StartPosition);
            EndPosition = StartPosition;
        }
        private void OnValidate()
        {
            MoveByDelta();
        }
        protected override void MoveByDelta()
        {
            float curveValue = Curve.Evaluate(Time*_endTime);
            text.text = ((int)(StartPosition + (delta * curveValue))).ToString();
        }
    }
}