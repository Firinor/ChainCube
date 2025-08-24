using System.Collections;
using FirAnimations;
using UnityEngine;
using UnityEngine.Events;

namespace YG
{
    public class TimerBeforeAdsYG : MonoBehaviour
    {
        [Tooltip("The timer object before the ad is shown. It will activate and deactivate at the right time.")]
        [SerializeField]
        private GameObject secondsPanelObject;
        [Tooltip("An array of objects that will be displayed in turn in a second. How many objects you put in the array will be reported for as many seconds before the ad is shown.\n\nFor example, put three objects in the array: the left with the text '3', the second with the text '2', the third with the text '1'.\nIn this case, a three-second report will occur showing objects with numbers before advertising.")]
        [SerializeField]
        private InterfaceReference<IFirAnimation>[] seconds;

        [Space(20)]
        [SerializeField] private UnityEvent onShowTimer;
        [SerializeField] private UnityEvent onHideTimer;

        private int objSecCounter;
        private Coroutine checkTimerAdCoroutine, timerAdShowCoroutine, backupTimerClosureCoroutine;

        private void OnEnable()
        {
            YG2.onOpenAnyAdv += RestartTimer;
            RestartTimer();
        }

        private void OnDisable()
        {
            YG2.onOpenAnyAdv -= RestartTimer;
        }

        IEnumerator CheckTimerAd()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                if (YG2.isTimerAdvCompleted && !YG2.nowAdsShow)
                {
                    onShowTimer?.Invoke();
                    objSecCounter = 0;

                    if (secondsPanelObject)
                        secondsPanelObject.SetActive(true);

                    timerAdShowCoroutine = StartCoroutine(TimerAdShow());
                    checkTimerAdCoroutine = null;
                    yield break;
                }
            }
        }

        IEnumerator TimerAdShow()
        {
            foreach (var obj in seconds)
                obj.Value.Initialize();
            
            while (true)
            {
                seconds[0].Value.Play();
                yield return new WaitForSecondsRealtime(1.0f);
                YG2.PauseGame(true);
                seconds[1].Value.Play();
                yield return new WaitForSecondsRealtime(1.0f);
                seconds[2].Value.Play();
                yield return new WaitForSecondsRealtime(1.0f);
                YG2.InterstitialAdvShow();
                backupTimerClosureCoroutine = StartCoroutine(BackupTimerClosure());
                
                while (!YG2.nowInterAdv)
                    yield return null;
                
                RestartTimer();
                yield break;
            }
        }

        IEnumerator BackupTimerClosure()
        {
            yield return new WaitForSecondsRealtime(2f);

            if (objSecCounter != 0)
            {
                RestartTimer();
                YG2.PauseGame(false);
            }

            backupTimerClosureCoroutine = null;
        }

        private void RestartTimer()
        {
            secondsPanelObject.SetActive(false);
            foreach (var obj in seconds)
                obj.Value.ToStartPoint();

            onHideTimer?.Invoke();
            objSecCounter = 0;

            if (checkTimerAdCoroutine == null)
            {
                if (seconds.Length > 0)
                    checkTimerAdCoroutine = StartCoroutine(CheckTimerAd());
                else
                    Debug.LogError("Fill in the array 'secondObjects'");
            }

            if (timerAdShowCoroutine != null)
            {
                StopCoroutine(timerAdShowCoroutine);
                timerAdShowCoroutine = null;
            }

            if (backupTimerClosureCoroutine != null)
            {
                StopCoroutine(backupTimerClosureCoroutine);
                backupTimerClosureCoroutine = null;
            }
        }
    }
}
