using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("# ========== UI Canvases ==========")]
    [Space(10)]
    [SerializeField] GameObject[] gameplayCanvases;
    [SerializeField] GameObject settingsCanvas;
    [SerializeField] DOTweenAnimation settingsPanelTweener;
    [SerializeField] GameObject[] backgrounds;
    [SerializeField] GameObject completedCanvas;
    [SerializeField] GameObject penProgressCanvas;
    [SerializeField] GameObject rateCanvas;
    [SerializeField] DOTweenAnimation ratePanelTweener;

    [Header("# ========== UI Elements ==========")]
    [Space(10)]
    [SerializeField] Text levelText;
    [SerializeField] DOTweenAnimation frameTweener;

    [SerializeField] Image accuracyProgressBar;
    [SerializeField] Text accuracyProgressText;

    [SerializeField] Image[] newPenImagesBack;
    [SerializeField] Image[] newPenImagesFore;
    [SerializeField] Sprite[] newPenSpritesWhite;
    [SerializeField] Sprite[] newPenSpritesShadow;
    [SerializeField] Color[] penWhiteForeColors;
    [SerializeField] Image penProgressBar;
    [SerializeField] Text penProgressText;
    [SerializeField] GameObject[] forAllUnlockedState;

    float tmpPenProgress;
    bool isNewPenUnlockable;

    public static UIManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance == this) return;
            Destroy(Instance.gameObject);
            Instance = this;
        }
    }

    void Start()
    {
        SetLevelText(PlayerData.CurrentLevel);
    }

    #region UI Interactions

    public void OpenSettings()
    {
        AudioManager.Instance.Click();
        //gameplayCanvases[1].SetActive(false);
        settingsCanvas.SetActive(true);
        settingsPanelTweener.DOPlayForward();
        GameManager.Instance.Pen.SetActive(false);
    }

    public void CloseSettings()
    {
        AudioManager.Instance.Click();
        settingsPanelTweener.DOPlayBackwards();
        StartCoroutine(OnDelayDisableSettingsCanvas());
    }

    IEnumerator OnDelayDisableSettingsCanvas()
    {
        yield return new WaitForSeconds(0.3f);
        settingsCanvas.SetActive(false);
        //gameplayCanvases[1].SetActive(true);
        GameManager.Instance.Pen.SetActive(true);
    }

    public void OpenRateCanvas()
    {
        AudioManager.Instance.Click();
        rateCanvas.SetActive(true);
        ratePanelTweener.DOPlayForward();
        GameManager.Instance.Pen.SetActive(false);
    }

    public void CloseRateCanvas()
    {
        AudioManager.Instance.Click();
        ratePanelTweener.DOPlayBackwards();
        StartCoroutine(OnDelayDisableRateCanvas());
    }

    IEnumerator OnDelayDisableRateCanvas()
    {
        yield return new WaitForSeconds(0.3f);
        rateCanvas.SetActive(false);
        GameManager.Instance.Pen.SetActive(true);
    }

    public void OpenCompletedCanvas(float accuracyRate)
    {
        PlayerData.HasPlayed = 1;
        AudioManager.Instance.StopRustleLoop();

        accuracyProgressBar.fillAmount = 0;
        accuracyProgressBar.DOFillAmount(accuracyRate, 1f).OnPlay(() =>
        {
            AudioManager.Instance.Fill();
        }).OnUpdate(() =>
        {
            accuracyProgressText.text = "Accuracy " + (accuracyProgressBar.fillAmount * 100).ToString("F0") + "%";
        });

        if (accuracyRate >= 0.6f) tmpPenProgress = 0.9f;
        else if (accuracyRate >= 0.3f) tmpPenProgress = 0.2f;
        else tmpPenProgress = 0.1f;
        //mask.SetActive(true);
        backgrounds[0].SetActive(false);
        backgrounds[1].SetActive(true);
        gameplayCanvases[0].SetActive(false);
        gameplayCanvases[1].SetActive(false);
        completedCanvas.SetActive(true);
        frameTweener.DOPlay();
    }

    void OnUnlockPen()
    {
        penProgressBar.fillAmount = 0;
        newPenImagesFore[0].fillAmount = 0;
        newPenImagesFore[1].fillAmount = 0;
        isNewPenUnlockable = false;

        PlayerData.CurrentPenProgress = 0;
        PlayerData.TotalPenUnlocked++;
        
        if (PlayerData.TotalPenUnlocked == 5)
        {
            PlayerData.OpenLastPen = 1;
            forAllUnlockedState[0].SetActive(true);
            forAllUnlockedState[1].SetActive(false);
            forAllUnlockedState[2].SetActive(false);
            return ;
        }

        newPenImagesBack[0].sprite = newPenSpritesWhite[PlayerData.TotalPenUnlocked];
        newPenImagesBack[1].sprite = newPenSpritesShadow[PlayerData.TotalPenUnlocked];
        newPenImagesFore[0].sprite = newPenSpritesWhite[PlayerData.TotalPenUnlocked];
        newPenImagesFore[1].sprite = newPenSpritesShadow[PlayerData.TotalPenUnlocked];
    }

    public void OnNextCompletedCanvas()
    {
        if (PlayerData.CurrentLevel == PlayerData.HighestLevel && PlayerData.HighestLevel < 30) PlayerData.HighestLevel++;
        PlayerData.CurrentLevel++;
        if (PlayerData.CurrentLevel > 30)
        {
            PlayerData.CurrentLevel = 1;
        }
        AudioManager.Instance.Click();
        AudioManager.Instance.StopRustleLoop();

        if (PlayerData.OpenLastPen == 0)
        {
            newPenImagesBack[0].sprite = newPenSpritesWhite[PlayerData.TotalPenUnlocked];
            newPenImagesBack[1].sprite = newPenSpritesShadow[PlayerData.TotalPenUnlocked];
            newPenImagesFore[0].sprite = newPenSpritesWhite[PlayerData.TotalPenUnlocked];
            newPenImagesFore[1].sprite = newPenSpritesShadow[PlayerData.TotalPenUnlocked];
            Debug.Log("PlayerData.TotalPenUnlocked " + PlayerData.TotalPenUnlocked);

            float progress = PlayerData.CurrentPenProgress;
            penProgressBar.fillAmount = progress;

            progress += tmpPenProgress;
            if (progress > 1) progress = 1;
            PlayerData.CurrentPenProgress = progress;
            newPenImagesFore[0].DOFillAmount(progress, 1f);
            newPenImagesFore[1].DOFillAmount(progress, 1f);
            newPenImagesFore[0].color = penWhiteForeColors[PlayerData.TotalPenUnlocked];
            penProgressBar.DOFillAmount(progress, 1f).OnPlay(() =>
            {
                AudioManager.Instance.Fill();
            }).OnUpdate(() =>
            {
                penProgressText.text = "Progress " + (penProgressBar.fillAmount * 100).ToString("F0") + "%";
            });
            if (progress == 1) isNewPenUnlockable = true;

            tmpPenProgress = 0;
        }
        else
        {
            forAllUnlockedState[0].SetActive(true);
            forAllUnlockedState[1].SetActive(false);
            forAllUnlockedState[2].SetActive(false);
        }

        completedCanvas.SetActive(false);
        penProgressCanvas.SetActive(true);
        GameManager.Instance.CameraControllerMoveToOriginTarget();

        GameManager.Instance.DestroyCurrentLevel();
        
    }

    public void OnNextPenProgressCanvas()
    {
        AudioManager.Instance.Click();
        AudioManager.Instance.StopRustleLoop();

        if (isNewPenUnlockable) OnUnlockPen();

        backgrounds[0].SetActive(true);
        backgrounds[1].SetActive(false);
        penProgressCanvas.SetActive(false);
        gameplayCanvases[0].SetActive(true);
        gameplayCanvases[1].SetActive(true);
        GameManager.Instance.LoadLevel();
        SetLevelText(PlayerData.CurrentLevel);
    
    }

    public void OnReplay()
    {
        float progress = PlayerData.CurrentPenProgress;
        progress += tmpPenProgress;
        if (progress > 1) progress = 1;
        PlayerData.CurrentPenProgress = progress;

        //PlayerData.CurrentLevel--;
        AudioManager.Instance.Click();
        backgrounds[0].SetActive(true);
        backgrounds[1].SetActive(false);
        completedCanvas.SetActive(false);
        gameplayCanvases[0].SetActive(true);
        gameplayCanvases[1].SetActive(true);

        GameManager.Instance.LoadLevel();
        SetLevelText(PlayerData.CurrentLevel);
        //GameManager.Instance.CameraControllerMoveToOriginTarget();
   
    }

    #endregion

    public void SetLevelText(int level)
    {
        levelText.text = "LEVEL " + level;
    }
}
