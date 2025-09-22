using UnityEngine;

namespace FirAnimations
{
    public class FirRotate : MonoBehaviour
    {
        [SerializeField] private float speed;

        void Update()
        {
            transform.rotation *= Quaternion.Euler(0,0,speed);
        }
    }
}