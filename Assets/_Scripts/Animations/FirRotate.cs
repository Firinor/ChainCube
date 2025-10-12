using UnityEngine;

namespace FirAnimations
{
    [ExecuteInEditMode]
    public class FirRotate : MonoBehaviour, IFirAnimation
    {
        [SerializeField] private float speed;

        void Update()
        {
            transform.rotation *= Quaternion.Euler(0,0,speed*Time.deltaTime);
        }
        
        public void Play() => enabled = true;
        public void Stop() => enabled = false;

        public void ToStartPoint()
        {
            transform.rotation = default;
        }

        public void ToEndPoint()
        {
            transform.rotation = default;
        }
    }
}