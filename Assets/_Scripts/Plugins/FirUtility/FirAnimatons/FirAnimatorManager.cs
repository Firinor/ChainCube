using System;
using System.Collections;
using UnityEngine;

namespace FirAnimations
{
    public class FirAnimationsManager : MonoBehaviour
    {
        [SerializeField, Range(0, 1)]
        private float _time;
        public float _timeLimit;
        [SerializeField] 
        private Animation[] animations;
        
        public Action OnEndAllAnimations;

        
        public void Play()
        {
            StartAnimations();
        }

        public void Stop()
        {
            ToEndPoint();
        }
        
        [ContextMenu("StartAnimations")]
        public void StartAnimations()
        {
            _time = 0;

            /*foreach (var animation in animations)
            {
                StartCoroutine(PlayAnimation(animation.animation.Value, animation.delay));
            }*/

            enabled = true;
        }

        private IEnumerator PlayAnimation(FirAnimation animation, float delay = 0)
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

        public void Update()
        {
            _time += Time.deltaTime/_timeLimit;
            if (_time >= 1)
            {
                enabled = false;
                foreach (var animation in animations)
                {
                    //animation.animation.Value.Stop();
                }
                OnEndAllAnimations?.Invoke();
            }
        }

        [ContextMenu("ToStartPoint")]
        public void ToStartPoint()
        {
            foreach (var animation in animations)
            {
                //animation.animation.Value.ToStartPoint();
            }
            
            _time = 0;
        }
        [ContextMenu("ToEndPoint")]
        public void ToEndPoint()
        {
            foreach (var animation in animations)
            {
                //animation.animation.Value.ToEndPoint();
            }

            _time = 1;
        }
    }
}