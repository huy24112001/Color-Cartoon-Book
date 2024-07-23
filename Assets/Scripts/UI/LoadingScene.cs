using System.Collections;
using TraceCurve;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingScene : MonoBehaviour
{
    [SerializeField] string sceneNameToLoad;
    [SerializeField] TraceFiller loadingTraceProgress;
    float loadProgress = 0;
    [SerializeField] bool isStart = false, isDone = false;
    private bool isCheck = false;

    void Update()
    {
        if (!isStart) return;

        loadProgress += Time.deltaTime / 2.5f;
        if (loadProgress > 1) loadProgress = 1;
        loadingTraceProgress.UpdateProgress(loadProgress);

        if (loadProgress >= 0.5f && !isCheck)
        {
            isCheck = false;
            isStart = false;
            AppOpenAdManager.Instance.ShowAdIfAvailable(() =>
            {
                //SceneManager.LoadScene(sceneNameToLoad);
                GameController.Instance.admobAds.InitBannerAdmob();
                isStart = true;
                isCheck = true;
            });
           
           
        }

        if (loadProgress == 1 && !isDone)
        {
            isDone = true;
            SceneManager.LoadScene(sceneNameToLoad);
        }
    }




    public void StartLoading()
    {
        StartCoroutine(OnDelayLoadScene());
    }

    IEnumerator OnDelayLoadScene()
    {
        yield return new WaitForSeconds(1f);
        isStart = true;
    }
}
