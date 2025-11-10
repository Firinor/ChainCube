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
        
        private Coroutine checkTimerAdCoroutine, timerAdShowCoroutine;

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

        IEnumerator CheckTimerAd()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);
                
                if (YG2.isTimerAdvCompleted && !YG2.nowAdsShow)
                {
                    if (YG2.SkipIterAdv)
                    {
                        YG2.InterstitialAdvShow();
                        YGInsides.SetTimerInterAdv();
                        continue;
                    }
                    
                    onShowTimer?.Invoke();

                    /*if (secondsPanelObject)
                        secondsPanelObject.SetActive(true);*/

                    timerAdShowCoroutine = StartCoroutine(TimerAdShow());
                    StopCoroutine(checkTimerAdCoroutine);
                    checkTimerAdCoroutine = null;
                    yield break;
                }
            }
        }

        IEnumerator TimerAdShow()
        {
            /*foreach (var obj in seconds)
                obj.Initialize();
            
            seconds[0].Play();
            YG2.PauseGame(true);
            yield return new WaitForSecondsRealtime(1.0f);
            seconds[1].Play();
            yield return new WaitForSecondsRealtime(1.0f);
            seconds[2].Play();
            yield return new WaitForSecondsRealtime(1.0f);*/
            YG2.InterstitialAdvShow();

            while (!YG2.nowInterAdv)
                yield return null;
            
            YG2.PauseGame(false);
            RestartTimer();
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

            if (checkTimerAdCoroutine == null)
            {
                if (seconds.Count > 0)
                    checkTimerAdCoroutine = StartCoroutine(CheckTimerAd());
                else
                    Debug.LogError("Fill in the array 'secondObjects'");
            }
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}
