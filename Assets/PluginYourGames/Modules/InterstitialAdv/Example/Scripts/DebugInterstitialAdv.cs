using UnityEngine;
using UnityEngine.UI;

namespace YG.Example
{
    public class DebugInterstitialAdv : MonoBehaviour
    {
        public Text timerText;

        private void Update()
        {
            string translate = "Timer before adv: ";
            timerText.text = translate + YG2.timerInterAdv.ToString("00.0");
        }
    }
}
