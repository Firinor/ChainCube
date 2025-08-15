using System;
using System.Collections;
using UnityEngine;

namespace FirAnimations
{
    public class FirAnimationsManager : MonoBehaviour
    {
        [SerializeField] 
        private Animation[] animations;

        [Serializable]
        private class Animation
        {
            public InterfaceReference<IFirAnimation> animation;
            public float delay;
        }
        private float _time;
        
        public float _timeLimit;
        public Action OnEndAllAnimations;

        [ContextMenu("StartAnimations")]
        public void StartAnimations()
        {
            _time = 0;

            foreach (var animation in animations)
            {
                StartCoroutine(PlayAnimation(animation.animation.Value, animation.delay));
            }

            enabled = true;
        }

        private IEnumerator PlayAnimation(IFirAnimation animation, float delay = 0)
        {
            animation.Initialize();
            float time = 0;
            while (time < delay)
            {
                time += Time.deltaTime;
                yield return null;
            }

            animation.Play();
        }

        private void Update()
        {
            _time += Time.deltaTime;
            if (_time > _timeLimit)
            {
                enabled = false;
                foreach (var animation in animations)
                {
                    animation.animation.Value.Stop();
                }
                OnEndAllAnimations?.Invoke();
            }
        }
        [ContextMenu("ToStartPoint")]
        public void ToStartPoint()
        {
            foreach (var animation in animations)
            {
                animation.animation.Value.ToStartPoint();
            }
        }
        [ContextMenu("ToEndPoint")]
        public void ToEndPoint()
        {
            foreach (var animation in animations)
            {
                animation.animation.Value.ToEndPoint();
            }
        }
    }
}