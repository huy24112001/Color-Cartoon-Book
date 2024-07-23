using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TraceCurve;
using DG.Tweening;
using PaintCore;
using PaintIn2D;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class LevelController : MonoBehaviour
{
    [SerializeField] GameState gameState;
    [Header("# ========== CAMERA ==========")]
    [Space(10)]
    [SerializeField] public List<Transform> cameraLooksAtTargets; // positions to look at
    [SerializeField] public bool isDrawing, isDoneCurrentLine, isDrawable, isFullFilled;

    [Header("# ========== DRAW LINE ==========")]
    [Space(10)]
    [SerializeField] int currentTraceIndex;
    [SerializeField] public List<TraceFiller> traceFillers; // trace fillers to draw
    [SerializeField] public List<GameObject> traceFillerGameObjects; // game objects holding trace filler of each part
    [SerializeField] public List<float> traceProgressSpeeds; // trace speeds of each parts
    float progress;
    [SerializeField] public List<DOTweenAnimation> fullLinesImages; // full lines images to show after drawing trace fillers
    [SerializeField] public List<GameObject> endDots; // last point of each trace filler

    [Header("# ========== COLOR ==========")]
    [Space(10)]
    [SerializeField] int currentColorIndex;
    [SerializeField] public List<DOTweenAnimation>  hintTweeners; // show colorable areas after drawing trace fillers
    [SerializeField] public List<SpriteRenderer>  hintSprites; // turn off hints when start coloring
    [SerializeField] public List<CwChangeCounter>  cwChangeCounters; // change counters to check if can be full filled
    [SerializeField] public List<ColorOption>  colorOptions; // color options to change
    [SerializeField] public List<SpriteRenderer>  fullColorParts; // full color parts to show after done coloring
    [SerializeField] public List<FullColorPartOptions>  fullColorSprites; // full color sprites to swap to fullColorParts
    [SerializeField] int currentColorOption;
    [SerializeField] public List<CwPaintableSprite> paintableSprites; // turn off paintable sprites after coloring
    [SerializeField] float accuracy = 0;
    Vector3 tmpPosition;
    Vector3 colorPenPositionOffset;
    bool isCalculatedOffset;
    Vector3 positionForClamp;

    private List<int> listColorReward = new List<int> { 3, 1, 3, 3, 1, 1, 1, 1, 1, 1, 3, 1, 1, 1, 1, 1, 2, 1, 2, 1 };


    [Header("# ========== Instance's References ==========")]
    [Space(10)]
    [SerializeField] public List<TracePainter> tracePainters;
    [SerializeField] public List<TraceInput>  traceInputs;
    //private bool isInputBlocked = false;
    public List<GameObject>  TraceFillerGameObjects { get => traceFillerGameObjects; set => traceFillerGameObjects = value; }
    public GameState GameState { get => gameState; set => gameState = value; }


    

    void Start()
    {
        gameState = GameState.None;
        progress = 0;
        isDoneCurrentLine = false;
        isDrawable = true;
        isDrawing = false;
        currentColorIndex = 0;
        currentTraceIndex = 0;
        currentColorOption = 0;
        accuracy = 0;

        StartCoroutine(OnDelayChangeState());
    }

    IEnumerator OnDelayChangeState()
    {
        yield return new WaitForEndOfFrame();
        gameState = GameState.DrawLine;
    }

    public void SetUpInstanceReferences(Transform brushObject)
    {
        for (int i = 0; i < tracePainters.Count; i++)
        {
            tracePainters[i].BrushObject = brushObject;
            traceInputs[i].Camera = Camera.main;
        }
    }

    void Update()
    {
        
        if (!isDrawable) return;

        switch (gameState)
        {
            case GameState.DrawLine:
                if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    GameManager.Instance.TapToPlayText.SetActive(false);

                    if (!isDrawing)
                    {
                        GameManager.Instance.PenTweeners[0].tween.onComplete = ToggleDrawingTrue;
                        GameManager.Instance.PenTweeners[0].DOPlayForward();
                        GameManager.Instance.PenTweeners[1].DOPlay();
                    }
                }
                else
                {
                    if (isDrawing)
                    {
                        isDrawing = false;
                        AudioManager.Instance.StopRustleLoop();
                        GameManager.Instance.PenTweeners[0].DOPlayBackwards();
                        GameManager.Instance.PenTweeners[1].DOPause();
                    }
                }

                if (isDrawing)
                {
                    progress += Time.deltaTime * traceProgressSpeeds[currentTraceIndex];
                    if (progress > 1)
                    {
                        progress = 1;
                        if (!isDoneCurrentLine)
                        {
                            isDoneCurrentLine = true;
                            isDrawing = false;
                            UpdateTraceLineIndex();
                        }
                    }
                    else AudioManager.Instance.PlayRustleLoop();
                    traceFillers[currentTraceIndex].UpdateProgress(progress);
                }
                else AudioManager.Instance.StopRustleLoop();
                break;
            case GameState.Color:
                if (Input.touchCount > 0)
                {
                    //Debug.Log("huy day " + hintSprites[currentColorIndex].gameObject.activeSelf);

                    if (hintSprites[currentColorIndex].gameObject.activeSelf)
                    {
                        //Debug.Log("huy day " + GameState.Color);
                        //if (hintTweeners[currentColorIndex].isPlaying())
                        hintTweeners[currentColorIndex].DOPause();
                        hintSprites[currentColorIndex].gameObject.SetActive(false);
                        paintableSprites[currentColorIndex].gameObject.SetActive(true);
                        hintSprites[currentColorIndex].color = ColorDefine.White;
                    }
                }
                else
                {
                    CheckChangeCounterCanBeFullFilled();
                }
                break;
        }
    }

    void LateUpdate()
    {
        if (isDoneCurrentLine == true && !isDrawing)
        {
            Debug.Log("Run");
        }
        if (gameState == GameState.Color)
        {
            if (Input.touchCount > 0 && GameManager.Instance. CanDragPen)
            {
                tmpPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                if (!isCalculatedOffset)
                {
                    colorPenPositionOffset = tmpPosition - GameManager.Instance.PenSpriteGO.transform.position;
                    isCalculatedOffset = true;
                }
                positionForClamp = tmpPosition - colorPenPositionOffset;
                if (positionForClamp.x < GameManager.Instance.ColorPenClampPoints[1].position.x) positionForClamp.x = GameManager.Instance.ColorPenClampPoints[1].position.x;
                if (positionForClamp.x > GameManager.Instance.ColorPenClampPoints[0].position.x) positionForClamp.x = GameManager.Instance.ColorPenClampPoints[0].position.x;
                if (positionForClamp.y < GameManager.Instance.ColorPenClampPoints[1].position.y) positionForClamp.y = GameManager.Instance.ColorPenClampPoints[1].position.y;
                if (positionForClamp.y > GameManager.Instance.ColorPenClampPoints[0].position.y) positionForClamp.y = GameManager.Instance.ColorPenClampPoints[0].position.y;
                GameManager.Instance.PenSpriteGO.transform.position = new Vector3(positionForClamp.x, positionForClamp.y, 0);
                AudioManager.Instance.PlayRustleLoop();
            }
            else
            {
                isCalculatedOffset = false;
                AudioManager.Instance.StopRustleLoop();
            }
        }
    }

    void ToggleDrawingTrue()
    {
        isDrawing = true;
    }

    public void OnDrawLineDone()
    {
        GameManager.Instance.Pen.SetActive(false);
        AudioManager.Instance.StopRustleLoop();
        for (int i = 0; i < colorOptions[currentColorIndex].colors.Length; i++)
        {
            GameManager.Instance.SetColorButtons(i, colorOptions[currentColorIndex].colors[i]);
        }
        gameState = GameState.ChooseColor;
    }

    public void SetColor(int index)
    {
        //Debug.Log("huy day : " + hintSprites[currentColorIndex].gameObject.activeSelf);
        if (PlayerData.CurrentLevel >= 3 && currentColorIndex == colorOptions.Count - 1 && index == listColorReward[PlayerData.CurrentLevel - 1] - 1)
        {
            GameController.Instance.admobAds.ShowVideoReward(actionReward: () =>
            {
            
            },
            actionNotLoadedVideo: () =>
            {
                Debug.Log("Video Reawart Not Load !! ");
            },
            actionClose: () =>
            {
                ExecuteColorChangeActions(index);

            }, "Show Video Reward From The user has chosen the correct color", "Show Video Reward in Level " + PlayerData.CurrentLevel);
        }
        else
        {
            ExecuteColorChangeActions(index);
        }
    }

    private void ExecuteColorChangeActions(int index)
    {
        GameManager.Instance.SetColor(colorOptions[currentColorIndex].colors[index]);
        GameManager.Instance.SetPenForColoring();
        isCalculatedOffset = false;
        gameState = GameState.Color;
        currentColorOption = index;
        if (index == 0) accuracy++;
        GameManager.Instance.DisableAllColorButtons();
    }

    void CheckChangeCounterCanBeFullFilled()
    {
        if (cwChangeCounters[currentColorIndex].Ratio >= 0.65f && !isFullFilled && !GameManager.Instance.FullFillButtonGO.activeSelf)
        {
            GameManager.Instance.FullFillButtonGO.SetActive(true);
            GameManager.Instance.FullFillButtonGO.GetComponent<DOTweenAnimation>().DORestart();
        }
    }

    public void OnClickFill()
    { 

        AudioManager.Instance.Done();
        AudioManager.Instance.StopRustleLoop();
        AudioManager.Instance.PlayHaptic();
        GameManager.Instance.Pen.SetActive(false);
        isFullFilled = true;
        fullColorParts[currentColorIndex].sprite = fullColorSprites[currentColorIndex].fullColoredSprites[currentColorOption];
        fullColorParts[currentColorIndex].gameObject.SetActive(true);
        paintableSprites[currentColorIndex].enabled = false;

        if (currentColorIndex < hintSprites.Count - 1)
        {
            currentColorIndex++;
            isFullFilled = false;
            hintSprites[currentColorIndex].gameObject.SetActive(true);
            //hintSprites[currentColorIndex].gameObject.transform.GetComponents<DOTweenAnima;

            for (int i = 0; i < colorOptions[currentColorIndex].colors.Length; i++)
            {
                GameManager.Instance.SetColorButtons(i, colorOptions[currentColorIndex].colors[i]);
            }
            gameState = GameState.ChooseColor;
        }
        else
        {
            UIManager.Instance.OpenCompletedCanvas(accuracy / hintSprites.Count);
            // Tính bounding box bao quanh tất cả các node con
            Bounds bounds = CalculateBounds(this.gameObject.transform);
            GameManager.Instance.CameraControllerOnEndChangeOrthoSize(bounds);
           
        }
    }

    public  Bounds CalculateBounds(Transform parent)
    {
        Bounds bounds = new Bounds(parent.position, Vector3.zero);

        foreach (SpriteRenderer renderer in parent.GetComponentsInChildren<SpriteRenderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }

    void UpdateTraceLineIndex()
    {
        isDrawable = false;
        GameManager.Instance.PenTweeners[0].DOPlayBackwards();
        GameManager.Instance.PenTweeners[1].DOPause();
        fullLinesImages[currentTraceIndex].DOPlay();
        GameManager.Instance.Pen.SetActive(false);
        endDots[currentTraceIndex].SetActive(false);
        GameManager.Instance.ShowFloatingText(endDots[currentTraceIndex].transform);

        AudioManager.Instance.Done();
        AudioManager.Instance.PlayHaptic();

        if (currentTraceIndex < traceFillers.Count - 1)
        {
            
            currentTraceIndex++;
            traceFillerGameObjects[currentTraceIndex].SetActive(true);

            progress = 0;
            GameManager.Instance.Pen.SetActive(true);
            //if (PlayerData.CurrentLevel != 5 && currentTraceIndex != 1) 
                GameManager.Instance.CameraControllerLooksAtNewTarget(cameraLooksAtTargets[currentTraceIndex]);
        }
        else
        {
            Debug.Log("OnDrawLineDone");
            OnDrawLineDone();
            currentTraceIndex = traceFillers.Count - 1;
            GameManager.Instance.CameraControllerMoveToOriginTarget();
        }
        StartCoroutine(OnDelayDraw());

    }

    IEnumerator OnDelayDraw()
    {
        yield return new WaitForSeconds(1f);
        isDrawable = true;
        isDoneCurrentLine = false;
        Debug.Log("OnDelayDraw " + isDrawable);

    }
}

[Serializable]
public class ColorOption
{
    public Color[] colors = new Color[3];
}

[Serializable]
public class FullColorPartOptions
{
    public Sprite[] fullColoredSprites = new Sprite[3];
}
