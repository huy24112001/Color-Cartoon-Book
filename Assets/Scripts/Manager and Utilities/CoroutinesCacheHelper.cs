using System;
using System.Collections;
using UnityEngine;

public class CoroutinesCacheHelper : MonoBehaviour
{
    public static void RunCoroutine(Action callback, float waitTime, bool isStopOnLoadScene = false)
    {
        Runner.CoroutineBridge.StartCoroutine(Run());

        IEnumerator Run()
        {
            yield return new WaitForSeconds(waitTime);
            if (isStopOnLoadScene)
            {
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == ObjectsTag.MAINMENU) yield break;
            }
            callback?.Invoke();
        }
    }

    public static void RunRealtimeCoroutine(Action callback, float waitTime, bool isStopOnLoadScene = false)
    {
        Runner.CoroutineBridge.StartCoroutine(Run());

        IEnumerator Run()
        {
            yield return new WaitForSecondsRealtime(waitTime);
            if (isStopOnLoadScene)
            {
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == ObjectsTag.MAINMENU) yield break;
            }
            callback?.Invoke();
        }
    }

    public static void RunCoroutine(Action callback, Func<bool> condition, bool isStopOnLoadScene = false)
    {
        Runner.CoroutineBridge.StartCoroutine(Run());

        IEnumerator Run()
        {
            yield return new WaitUntil(condition);
            if (isStopOnLoadScene)
            {
                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == ObjectsTag.MAINMENU) yield break;
            }
            callback?.Invoke();
        }
    }

    class Runner : MonoBehaviour
    {
        public static Runner CoroutineBridge;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            CoroutineBridge = new GameObject("[Coroutines Cache Helper]").AddComponent<Runner>();
            DontDestroyOnLoad(CoroutineBridge);
        }
    }
}