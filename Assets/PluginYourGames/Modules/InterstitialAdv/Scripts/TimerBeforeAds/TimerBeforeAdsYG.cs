using System.Collections;
using System.Collections.Generic;
using FirAnimations;
using UnityEngine;
using UnityEngine.Events;
using YG.Insides;

namespace YG
{
    public class TimerBeforeAdsYG : MonoBehaviour
    {
        [Tooltip("The timer object before the ad is shown. It will activate and deactivate at the right time.")]
        [SerializeField]
        private GameObject secondsPanelObject;
        [Tooltip("An array of objects that will be displayed in turn in a second. How many objects you put in the array will be reported for as many seconds before the ad is shown.\n\nFor example, put three objects in the array: the left with the text '3', the second with the text '2', the third with the text '1'.\nIn this case, a three-second report will occur showing objects with numbers before advertising.")]
        [SerializeField]
        private List<FirAnimation> seconds;

        [Space(20)]
        [SerializeField] private UnityEvent onShowTimer;
        [SerializeField] private UnityEvent onHideTimer;
        
        private Coroutine timerAdShowCoroutine;

        private void OnEnable()
        {
            YG2.onOpenAnyAdv += RestartTimer;
            RestartTimer();
        }

        private void OnDisable()
        {
            YG2.onOpenAnyAdv -= RestartTimer;
            StopAllCoroutines();
        }

        public void CheckTimerAd()
        {
            if (!YG2.isTimerAdvCompleted || YG2.nowAdsShow) 
                return;
            
            if (YG2.SkipIterAdv)
            {
                YG2.InterstitialAdvShow();
                YGInsides.SetTimerInterAdv();
                return;
            }
                
            //onShowTimer?.Invoke();

            /*if (secondsPanelObject)
                    secondsPanelObject.SetActive(true);*/

            timerAdShowCoroutine = StartCoroutine(AdShow());
        }

        IEnumerator AdShow()
        {
            YG2.PauseGame(true);
            /*foreach (var obj in seconds)
                obj.Initialize();
            
            seconds[0].Play();
            yield return new WaitForSecondsRealtime(1.0f);
            seconds[1].Play();
            yield return new WaitForSecondsRealtime(1.0f);
            seconds[2].Play();
            yield return new WaitForSecondsRealtime(1.0f);*/
            YG2.InterstitialAdvShow();

            while (!YG2.nowInterAdv)
                yield return null;
            
            YG2.PauseGame(false);
        }

        private void RestartTimer()
        {
            if (timerAdShowCoroutine != null)
            {
                StopCoroutine(timerAdShowCoroutine);
                timerAdShowCoroutine = null;
            }
            
            secondsPanelObject.SetActive(false);
            foreach (var obj in seconds)
                obj.ToStartPoint();

            onHideTimer?.Invoke();
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
