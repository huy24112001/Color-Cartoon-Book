using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GoogleMobileAds.Api;
using MoreMountains.Feedbacks;
using PaintIn2D;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{


    public Camera MainCamera;

    [Header("# ========== LEVELS ==========")]
    [Space(10)]
    [SerializeField] List<LevelController> levelPrefabs;
    [SerializeField] LevelController currentLevel;
    [SerializeField] int currentLevelIndex;

    [Header("# ========== GAMEPLAY RESOURCES ==========")]
    [Space(10)]
    [SerializeField] CameraController cameraController;
    [SerializeField] RectTransform canVas;
    [SerializeField] GameObject tapToPlayText;
    [SerializeField] DOTweenAnimation[] penTweeners;

    // =============== Pen ===============

    [SerializeField] GameObject pen;
    [SerializeField] SpriteRenderer penPoint;
    [SerializeField] SpriteRenderer penColor;
    [SerializeField] Transform penSpriteGO;
    [SerializeField] bool canDragPen;

    // ===================================

    [SerializeField] SpriteRenderer[] penSkinImages;
    [SerializeField] Sprite[] penSkinSpritesWhite;
    [SerializeField] Sprite[] penSkinSpritesShadow;
    [SerializeField] CwPaintDecal2D cwPaintDecal2DMain;
    [SerializeField] CwPaintDecal2D cwPaintDecal2DExpand;
    [SerializeField] GameObject expandPaintListener;
    [SerializeField] Image[] chooseColorImages;
    [SerializeField] Button[] colorButtons;
    [SerializeField] Button fullFillButton;

    // =====================================================

    [SerializeField] Image[] hintImage;
    [SerializeField] Sprite[] hintSprites;

    [Header("# ========== FLOATING TEXT ==========")]
    [Space(10)]
    [SerializeField] MMF_Player targetPlayer;
    [SerializeField] MMF_FloatingText floatingText;
    [SerializeField] ParticleSystem congratsVFX;

    [Header("# ========== PEN POSITION ==========")]
    [Space(10)]
    [SerializeField] Transform[] colorPenClampPoints;

    public GameObject TapToPlayText { get => tapToPlayText; set => tapToPlayText = value; }
    public DOTweenAnimation[] PenTweeners { get => penTweeners; set => penTweeners = value; }
    public GameObject Pen { get => pen; set => pen = value; }
    public GameObject FullFillButtonGO { get => fullFillButton.gameObject; }
    public bool CanDragPen { get => canDragPen; set => canDragPen = value; }
    public Transform PenSpriteGO { get => penSpriteGO; set => penSpriteGO = value; }
    public Transform[] ColorPenClampPoints { get => colorPenClampPoints; set => colorPenClampPoints = value; }

    public RectTransform topRight;

    public RectTransform bottomLeft;

    public RectTransform rectLevelObject;

    [SerializeField] DOTweenAnimation[] btnTweeners;




    [Button("get pos")]

    public void Position()
    {
        Vector3 pos = Vector3.zero;
        pos = topRight.transform.position;
        Debug.Log("pos : " + pos);
    }

    public static GameManager Instance;

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
        GameManager.Instance.ColorPenClampPoints[0].position = topRight.transform.position;
        GameManager.Instance.ColorPenClampPoints[1].position = bottomLeft.transform.position;

        //Debug.Log("here " + topRight.transform.position);

        //Vector2 sizeDelta = rectLevelObject.sizeDelta;
        //sizeDelta.x = Mathf.Abs(topRight.transform.position.x - bottomLeft.transform.position.x);
        //sizeDelta.y = Mathf.Abs(topRight.transform.position.y - bottomLeft.transform.position.y);
        //rectLevelObject.sizeDelta = sizeDelta;
        //Debug.Log(sizeDelta);


        LoadLevel();
    }



    #region Camera Controller

    public void CameraControllerLooksAtNewTarget(Transform target)
    {
        cameraController.LookAtNewTarget(target);
    }

    public void CameraControllerMoveToOriginTarget()
    {
        cameraController.MoveToOriginTarget();
    }

    public void CameraControllerOnEndChangeOrthoSize(Bounds bounds)
    {
        //if (PlayerData.CurrentLevel == 1) cameraController.OnEndChangeOrthoSize(1);
        //else cameraController.OnEndChangeOrthoSize(bounds);
        cameraController.OnEndChangeOrthoSize(bounds);
    }

    #endregion

    #region Set Colors

    public void SetPenForColoring()
    {
        pen.SetActive(true);
        penPoint.enabled = false;
        StartCoroutine(OnDelaySetPenTransform());
    }

    IEnumerator OnDelaySetPenTransform()
    {
        pen.transform.localRotation = Quaternion.identity;
        canDragPen = true;
        yield return new WaitForSeconds(0.5f);
        expandPaintListener.SetActive(false);
        expandPaintListener.SetActive(true);
    }

    public void SetColor(Color color)
    {
        GameManager.Instance.ColorPenClampPoints[0].position = topRight.transform.position;
        GameManager.Instance.ColorPenClampPoints[1].position = bottomLeft.transform.position;
        AudioManager.Instance.ChooseColor();
        cwPaintDecal2DMain.Color = color;
        cwPaintDecal2DExpand.Color = color;
        penColor.color = color;
    }

    public void SetColorButtons(int index, Color color)
    {
        chooseColorImages[index].color = color;
        chooseColorImages[index].transform.parent.gameObject.SetActive(true);
        colorButtons[index].GetComponent<DOTweenAnimation>().DORestart();
    }

    public void DisableAllColorButtons()
    {
        for (int i = 0; i < chooseColorImages.Length; i++)
        {
            chooseColorImages[i].transform.parent.gameObject.SetActive(false);
        }
    }



    #endregion

    #region Level Controller

    public float CalculatorScaleOrthographicSize(Bounds bounds, RectTransform rectTransform)
    {

        // Tính toán kích thước trong không gian màn hình
        float width = bounds.size.x;
        float height = bounds.size.y;
        float scaleOld = width / height;

        //Tính toán kích thước của ô vuông UI
        float uiSquareWidth = rectTransform.rect.width * 0.9f;
        float uiSquareHeight = rectTransform.rect.height * 0.9f;

        float widthNew = uiSquareWidth;
        float heightNew = widthNew / scaleOld;

        if (heightNew > uiSquareHeight)
        {
            heightNew = uiSquareHeight;
            widthNew = heightNew * scaleOld;

        }

        return width / widthNew;
    }
    public void LoadLevel()
    {
        if (currentLevel != null) Destroy(currentLevel.gameObject);

        currentLevelIndex = PlayerData.CurrentLevel - 1;



        // Vector3.zero
        currentLevel = Instantiate(levelPrefabs[currentLevelIndex].gameObject, levelPrefabs[currentLevelIndex].gameObject.transform.position, Quaternion.identity).GetComponent<LevelController>();
        currentLevel.SetUpInstanceReferences(pen.transform);

        if (currentLevelIndex == 10 || currentLevelIndex == 11 || currentLevelIndex == 13 || currentLevelIndex == 15 || currentLevelIndex == 17 || currentLevelIndex == 18)
        {
            MainCamera.orthographicSize = 20.5f;
            cameraController.firstOrthographicSize = MainCamera.orthographicSize;
        }
        else
        {
            MainCamera.orthographicSize = 17f;
            cameraController.firstOrthographicSize = MainCamera.orthographicSize;
        }



        //Bounds bounds = currentLevel.CalculateBounds(currentLevel.gameObject.transform);
        //////Debug.Log("bound " + bounds.size.x +" "+ bounds.size.y);
        //if (bounds.size.x > rectLevelObject.sizeDelta.x * 0.9f || bounds.size.y > rectLevelObject.sizeDelta.y * 0.9f)
        //{
        //    MainCamera.orthographicSize *= CalculatorScaleOrthographicSize(bounds, rectLevelObject);
        //    cameraController.firstOrthographicSize = MainCamera.orthographicSize;
        //}

        currentLevel.gameObject.SetActive(true);
        CameraControllerLooksAtNewTarget(currentLevel.cameraLooksAtTargets[0]);

        for (int i = 0; i < hintImage.Length; i++)
        {
            hintImage[i].sprite = hintSprites[PlayerData.CurrentLevel - 1];
            hintImage[i].SetNativeSize();
        }
        hintImage[0].transform.localScale = Vector3.one * 0.125f;
        hintImage[1].transform.localScale = Vector3.one * 0.08f;
        pen.SetActive(true);
        canDragPen = false;

        for (int i = 0; i < penTweeners.Length; i++)
        {
            penTweeners[i].DORewind();
        }

        cwPaintDecal2DMain.Color = ColorDefine.White;
        cwPaintDecal2DExpand.Color = ColorDefine.White;
        penColor.color = ColorDefine.DarkGray;
        expandPaintListener.SetActive(false);
        penPoint.enabled = true;

        fullFillButton.onClick.RemoveAllListeners();
        fullFillButton.onClick.AddListener(currentLevel.OnClickFill);
        fullFillButton.onClick.AddListener(DisableFullFillButton);
        colorButtons[0].onClick.RemoveAllListeners();
        colorButtons[0].onClick.AddListener(() => currentLevel.SetColor(0));
        colorButtons[1].onClick.RemoveAllListeners();
        colorButtons[1].onClick.AddListener(() => currentLevel.SetColor(1));
        colorButtons[2].onClick.RemoveAllListeners();
        colorButtons[2].onClick.AddListener(() => currentLevel.SetColor(2));

        cameraController.Target = currentLevel.TraceFillerGameObjects[0].transform;
        cameraController.enabled = true;
        GameManager.Instance.ColorPenClampPoints[0].position = topRight.transform.position;
        GameManager.Instance.ColorPenClampPoints[1].position = bottomLeft.transform.position;
        btnTweeners[0].GetComponent<RectTransform>().transform.localScale = Vector3.zero;
        btnTweeners[1].GetComponent<RectTransform>().transform.localScale = Vector3.zero;


    }

    public void LoadLevel(LevelItem levelItem)
    {
        PlayerData.CurrentLevel = levelItem.ID + 1;
        if (currentLevel != null) Destroy(currentLevel.gameObject);

        currentLevelIndex = levelItem.ID;
        currentLevel = Instantiate(levelPrefabs[currentLevelIndex].gameObject, levelPrefabs[currentLevelIndex].gameObject.transform.position, Quaternion.identity).GetComponent<LevelController>();
        currentLevel.SetUpInstanceReferences(pen.transform);

        if (currentLevelIndex == 10 || currentLevelIndex == 11 || currentLevelIndex == 13 || currentLevelIndex == 15 || currentLevelIndex == 17 || currentLevelIndex == 18)
        {
            MainCamera.orthographicSize = 20.5f;
            cameraController.firstOrthographicSize = MainCamera.orthographicSize;
        }
        else
        {
            MainCamera.orthographicSize = 15f;
            cameraController.firstOrthographicSize = MainCamera.orthographicSize;
        }
        MainCamera.transform.position = new Vector3(0, 0, -10);

        //Bounds bounds = currentLevel.CalculateBounds(currentLevel.gameObject.transform);
        ////Debug.Log("bound " + bounds.size.x +" "+ bounds.size.y);
        //if (bounds.size.x > rectLevelObject.sizeDelta.x * 0.9f || bounds.size.y > rectLevelObject.sizeDelta.y * 0.9f)
        //{
        //    MainCamera.orthographicSize *= CalculatorScaleOrthographicSize(bounds, rectLevelObject);
        //    cameraController.firstOrthographicSize = MainCamera.orthographicSize;
        //}

        currentLevel.gameObject.SetActive(true);
        CameraControllerLooksAtNewTarget(currentLevel.cameraLooksAtTargets[0]);

        for (int i = 0; i < hintImage.Length; i++)
        {
            hintImage[i].sprite = hintSprites[levelItem.ID];
            hintImage[i].SetNativeSize();
        }
        hintImage[0].transform.localScale = Vector3.one * 0.125f;
        hintImage[1].transform.localScale = Vector3.one * 0.08f;
        pen.SetActive(true);
        canDragPen = false;

        for (int i = 0; i < penTweeners.Length; i++)
        {
            penTweeners[i].DORewind();
        }

        cwPaintDecal2DMain.Color = ColorDefine.White;
        cwPaintDecal2DExpand.Color = ColorDefine.White;
        penColor.color = ColorDefine.DarkGray;
        expandPaintListener.SetActive(false);
        penPoint.enabled = true;

        fullFillButton.onClick.RemoveAllListeners();
        fullFillButton.onClick.AddListener(currentLevel.OnClickFill);
        fullFillButton.onClick.AddListener(DisableFullFillButton);
        colorButtons[0].onClick.RemoveAllListeners();
        colorButtons[0].onClick.AddListener(() => currentLevel.SetColor(0));
        colorButtons[1].onClick.RemoveAllListeners();
        colorButtons[1].onClick.AddListener(() => currentLevel.SetColor(1));
        colorButtons[2].onClick.RemoveAllListeners();
        colorButtons[2].onClick.AddListener(() => currentLevel.SetColor(2));

        cameraController.Target = currentLevel.TraceFillerGameObjects[0].transform;
        cameraController.enabled = true;
        GameManager.Instance.ColorPenClampPoints[0].position = topRight.transform.position;
        GameManager.Instance.ColorPenClampPoints[1].position = bottomLeft.transform.position;
        btnTweeners[0].GetComponent<RectTransform>().transform.localScale = Vector3.zero;
        btnTweeners[1].GetComponent<RectTransform>().transform.localScale = Vector3.zero;
    }

    public void DestroyCurrentLevel()
    {
        if (currentLevel != null) Destroy(currentLevel.gameObject);
        currentLevel = null;
        fullFillButton.gameObject.SetActive(false);
        for (int i = 0; i < colorButtons.Length; i++)
        {
            colorButtons[i].gameObject.SetActive(false);
        }
    }

    void DisableFullFillButton()
    {
        fullFillButton.gameObject.SetActive(false);
        cwPaintDecal2DExpand.Color = ColorDefine.White;
    }

    public void DisableCurrentLevel()
    {
        currentLevel.gameObject.SetActive(false);
    }

    public void EnableCurrentLevel()
    {
        currentLevel.gameObject.SetActive(true);
        if (currentLevel.GameState == GameState.ChooseColor || currentLevel.GameState == GameState.DrawLine)
        {
            currentLevel.isDrawable = true;
            currentLevel.isDoneCurrentLine = false;
        }
    }

    #endregion

    #region Pen Skins

    public void SetPenSkin(int index)
    {
        penSkinImages[0].sprite = penSkinSpritesWhite[index];
        penSkinImages[1].sprite = penSkinSpritesShadow[index];
    }

    public void MovePenByDotweenAni( Vector2 endPos)
    {
    //    pen.transform.DOMove(endPos, 0.5f)
    //        .SetEase(Ease.Linear) // Tùy chọn: chọn hiệu ứng easing
    //        .OnStart(() => Debug.Log("Animation Started")) // Tùy chọn: khi hoạt ảnh bắt đầu
    //        .OnComplete(() => Debug.Log("Animation Completed")); // Tùy chọn: khi hoạt ảnh hoàn tất
    }

    #endregion

    #region Floating Text

    public void ShowFloatingText(Transform targetTransform)
    {
        AudioManager.Instance.StopRustleLoop();
        int rnd = Random.Range(0, 3);
        string content = "";
        switch (rnd)
        {
            case 0:
                content = "Great!";
                break;
            case 1:
                content = "Awesome!";
                break;
            case 2:
                content = "Perfect!";
                break;
            case 3:
                content = "Excellent!";
                break;
            case 4:
                content = "Nice!";
                break;
        }

        floatingText = targetPlayer.GetFeedbackOfType<MMF_FloatingText>();
        floatingText.Value = content;
        targetPlayer.transform.position = targetTransform.position;
        targetPlayer.PlayFeedbacks(targetPlayer.transform.position);
    }

    public void PlayCongratsVFX()
    {
        //mask.SetActive(false);
        btnTweeners[0].DORestart();
        btnTweeners[1].DORestart();
        congratsVFX.Play(true);
        AudioManager.Instance.Congrats();
    }

    #endregion
}
