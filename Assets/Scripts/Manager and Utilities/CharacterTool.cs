using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using Manager_and_Utilities;
using PaintCore;
using PaintIn2D;
using Sirenix.OdinInspector;
using TraceCurve;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

#if UNITY_EDITOR
public class CharacterTool : MonoBehaviour
{
    public List<Sprite> sprites;
    public List<string> textAssetPaths;
    public Transform Parts;
    [HideInInspector] public GameObject tracePrefab;
    public List<GameObject> TraceList;
    public DefaultAsset folder;
    [HideInInspector] public LevelController LevelController;
    [Button("Create & Load all references")]
    public void Create()
    {
        tracePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Trace.prefab");
        LevelController = Parts.GetComponent<LevelController>();
        //#====================================Initialization===========================================#
        sprites.Clear();
        textAssetPaths.Clear();
        LevelController.traceFillers.Clear();
        LevelController.traceFillerGameObjects.Clear();
        LevelController.traceProgressSpeeds.Clear();
        LevelController.fullLinesImages.Clear();
        LevelController.endDots.Clear();
        LevelController.hintTweeners.Clear();
        LevelController.hintSprites.Clear();
        LevelController.cwChangeCounters.Clear();
        LevelController.colorOptions.Clear();
        LevelController.fullColorParts.Clear();
        LevelController.fullColorSprites.Clear();
        LevelController.paintableSprites.Clear();
        LevelController.cameraLooksAtTargets.Clear();
        LevelController.tracePainters.Clear();
        LevelController.traceInputs.Clear();
        if (folder == null) return;
        //#===================================Generate All FolderType===================================#
        string folderPath = AssetDatabase.GetAssetPath(folder);// GetLevelFilePath;
        DirectoryInfo fileLevel = new DirectoryInfo(folderPath);
        foreach (DirectoryInfo folderInfo in fileLevel.GetDirectories())
        {
            if (Enum.TryParse(folderInfo.Name, out FolderType folderType))
            {
                string pathInside = folderPath + '/' + folderType.ToString();
                GenerateFile(folderType, pathInside);
            }
            else
            {
                Debug.LogWarning($"No FouderType has name: {folderInfo.Name} exist");
            }
        }
    }

    private void GenerateFile(FolderType folderType, string folderPath)
    {
        //#===================================Get All Sprite of this Folder===================================#
        string[] guids = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });
        sprites.Clear();
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (sprite != null)
            {
                sprites.Add(sprite);
            }
        }
        sprites.Sort(new SpriteNameComparer());
            
        textAssetPaths = Directory.GetFiles(folderPath, "*.cor").ToList();
        textAssetPaths.Sort();
        //#===================================Generate File===================================#
        
        
        
        switch (folderType)
        {
            case FolderType.fillwhite: // PaintArea
            {
                for (int i = 0; i < textAssetPaths.Count; i++)
                {
                    GameObject objectItem = new GameObject("Paint Area " + i);
                    objectItem.transform.parent = Parts.transform;
                    //#================set component===================#
                    SpriteRenderer spriteRenderer = objectItem.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprites[i];
                    spriteRenderer.sortingOrder = 0;
                    
                    CwPaintableSprite paintableSprite = objectItem.AddComponent<CwPaintableSprite>();
                    CwPaintableSpriteTexture paintableSpriteTexture = objectItem.GetComponent<CwPaintableSpriteTexture>();
                    CwChangeCounter changes = objectItem.AddComponent<CwChangeCounter>();
                    changes.PaintableTexture = paintableSpriteTexture;
                    changes.Threshold = 0;
                    changes.WaitUntilNotPainting = true;
                    //#================locate item===================#
                    LocateItem(i,objectItem, spriteRenderer);
                    //#================reference all object to levelController================#
                    LevelController.cwChangeCounters.Add(changes);
                    LevelController.paintableSprites.Add(paintableSprite);
                }
                break;
            }
            case  FolderType.pattern: // HintPattern
            {
                for (int i = 0; i < textAssetPaths.Count; i++)
                {
                    GameObject objectItem = new GameObject("Hint Pattern " + i);
                    objectItem.transform.SetParent(Parts);
                    //#================set component===================#
                    SpriteRenderer spriteRenderer = objectItem.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprites[i];
                    spriteRenderer.color = new Vector4(1, 1, 1, 0.1f);
                    spriteRenderer.sortingOrder = 1;
                    
                    DOTweenAnimation doTweenAnimation = objectItem.AddComponent<DOTweenAnimation>();
                    doTweenAnimation.animationType = DOTweenAnimation.AnimationType.Color;
                    doTweenAnimation.easeType = Ease.InOutSine;
                    doTweenAnimation.duration = 0.35f;
                    doTweenAnimation.loopType = LoopType.Yoyo;
                    doTweenAnimation.loops = -1;
                    doTweenAnimation.autoPlay = true;
                    doTweenAnimation.autoKill = false;
                    
                    //#================locate item===================#
                    LocateItem(i,objectItem, spriteRenderer);
                    //#================reference all object to levelController================#
                    LevelController.hintTweeners.Add(doTweenAnimation);
                    LevelController.hintSprites.Add(spriteRenderer);
                }
                break;
            }
            case FolderType.line_full: // FullLine
            {
                for (int i = 0; i < textAssetPaths.Count; i++)
                {

                        GameObject objectItem = new GameObject("Full Line " + i);
                    objectItem.transform.SetParent(Parts);
                    //#================set component===================#
                    SpriteRenderer spriteRenderer = objectItem.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprites[i];
                    spriteRenderer.color = new Vector4(1, 1, 1, 0);
                    spriteRenderer.sortingOrder = 2;
                    
                    DOTweenAnimation doTweenAnimation = objectItem.AddComponent<DOTweenAnimation>();
                    doTweenAnimation.animationType = DOTweenAnimation.AnimationType.Color;
                    doTweenAnimation.easeType = Ease.InSine;
                    doTweenAnimation.duration = 0.75f;
                    doTweenAnimation.loops = 1;
                    doTweenAnimation.autoKill = false;
                    doTweenAnimation.autoPlay = false;
                    
                    if (i == textAssetPaths.Count - 1)
                        {                    
                            doTweenAnimation.hasOnComplete = true;       
                        }

                    //#================locate item===================#
                    LocateItem(i,objectItem, spriteRenderer);
                    //#================reference all object to levelController================#
                    LevelController.fullLinesImages.Add(doTweenAnimation);
                    
                }
                break;
            }
            case FolderType.color_1:
            {
                for (int i = 0; i < textAssetPaths.Count; i++)
                {
                    GameObject objectItem = new GameObject("Full Color " + i);
                    objectItem.transform.SetParent(Parts);
                    //#================set component===================#
                    SpriteRenderer spriteRenderer = objectItem.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprites[i];
                    spriteRenderer.sortingOrder = 1;
                    //#================locate item===================#
                    LocateItem(i,objectItem, spriteRenderer);
                    //#================reference all object to levelController================#
                    LevelController.fullColorParts.Add(spriteRenderer);
                    LevelController.colorOptions.Add(new ColorOption());
                    LevelController.fullColorSprites.Add(new FullColorPartOptions());
                }
                for (int i = 0; i < textAssetPaths.Count; i++)
                {
                    LevelController.colorOptions[i].colors[0] = GetMostFrequentColor(sprites[i]);
                    LevelController.fullColorSprites[i].fullColoredSprites[0] = sprites[i];
                }
                break;
            }
            case FolderType.color_2: 
            {
                
                for (int i = 0; i < textAssetPaths.Count; i++)
                {
                    //#================reference all object to levelController================#
                    LevelController.colorOptions[i].colors[1] = GetMostFrequentColor(sprites[i]);
                    LevelController.fullColorSprites[i].fullColoredSprites[1] = sprites[i];

                }
                break;
            }
            case FolderType.color_3: 
            {
                for (int i = 0; i < textAssetPaths.Count; i++)
                {
                    //#================reference all object to levelController================#
                    LevelController.colorOptions[i].colors[2] = GetMostFrequentColor(sprites[i]);
                    LevelController.fullColorSprites[i].fullColoredSprites[2] = sprites[i];


                }
                break;
            }
            case FolderType.hint: // Dots then next to TraceObject
            {
                TraceList.Clear();
                var genPath = Directory.GetFiles(folderPath, "*.txt").ToList();
                genPath.Sort();
                    for (int i = 0; i < textAssetPaths.Count; i++)
                {
                    Debug.Log(i);
                    GameObject objectItem = Instantiate(tracePrefab, Parts, true);
                    objectItem.name = "Trace " + i;
                    TraceList.Add(objectItem);
                    
                    //#================set component===================#
                    objectItem.TryGetComponent(out Trace trace);
                    GameObject dots;
                    SpriteRenderer spriteRenderer = (dots = trace.dots.gameObject).GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprites[i];

                        //#====================Gen Dots====================#



                        var bytes = File.ReadAllText(textAssetPaths[i]).Split(' ');

                        float.TryParse(bytes[0], out var tmpX); // .cor Rectifying

                        float.TryParse(bytes[1], out var tmpY);

                        var tmpVector = LocateItem(i, spriteRenderer: spriteRenderer);

                        Debug.Log(tmpVector);

                        var lines = File.ReadAllLines(genPath[i]);

                        for (int k = 0; k < lines.Length - 1; k++)

                        {

                            var parts = lines[k].Split(',');

                            var lineDraw = AddGameObjectWithLine(trace.tracePainter);

                            var geometry = trace.tracePainter.GetComponent<GeometryContainer>().Objects;

                            if (!float.TryParse(parts[0], out var x) || !float.TryParse(parts[1], out var y))

                            {

                                Debug.LogError("Can't parse line ");

                                continue;

                            }

                            lineDraw.Start = new Vector3(x + tmpVector.x, y + tmpVector.y, 0);

                            parts = lines[k + 1].Split(',');

                            if (!float.TryParse(parts[0], out x) || !float.TryParse(parts[1], out y))

                            {

                                Debug.LogError("Can't parse line ");

                                continue;

                            }

                            lineDraw.End = new Vector3(x + tmpVector.x, y + tmpVector.y, 0);

                            geometry.Add(lineDraw);

                        }

                        //#================locate item===================#
                        LocateItem(i,dots,spriteRenderer);
                    //#================reference all object to levelController================
                    LevelController.traceFillers.Add(trace.tracePainter.GetComponent<TraceFiller>());
                    // LevelController.traceFillerGameObjects.Add(trace.traceObject.gameObject);
                    LevelController.traceFillerGameObjects.Add(objectItem);
                    // LevelController.traceFillerGameObjects.Add(objectItem);
                    LevelController.traceProgressSpeeds.Add(0.3f);
                    LevelController.endDots.Add(trace.dots.GetChild(0).gameObject);
                    LevelController.tracePainters.Add(trace.tracePainter.GetComponent<TracePainter>());
                    LevelController.traceInputs.Add(trace.tracePainter.GetComponent<TraceInput>());
                    LevelController.cameraLooksAtTargets.Add(trace.traceObject);
                }
                break;
                
            }
            case FolderType.line: //TraceObject 
            {
                for (int i = 0; i < TraceList.Count; i++)
                {
                    GameObject objectItem = TraceList[i];
                    //#================set component===================#
                    objectItem.TryGetComponent(out Trace trace);
                    GameObject traceFullLine;
                    SpriteRenderer spriteRenderer = (traceFullLine = trace.traceObject.gameObject).GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = sprites[i];
                    
                    //#================locate item===================#
                    LocateItem(i,traceFullLine,spriteRenderer);
                    
                }
                break;
            }
            
            // default:
                // throw new ArgumentOutOfRangeException(nameof(folderType), folderType, null);
        }
    }

    private Vector2 LocateItem(int index, GameObject objectItem = null, SpriteRenderer spriteRenderer = null)

    {
        string[] bytes = File.ReadAllText(textAssetPaths[index]).Split(' ');

        int x = int.Parse(bytes[0]);

        int y = int.Parse(bytes[1]);

        Vector2 objectPos = new Vector2(x, y);

        var size = spriteRenderer.size;

        float width = size.x * 100;

        float height = size.y * 100;

        Vector3 resultPos = new Vector3(objectPos.x + width / 2 - 1920 / 2, 1080 / 2 - (objectPos.y + height / 2), 0f) / 100;

        if (objectItem != null)

            objectItem.transform.localPosition = resultPos;

        return resultPos;

    }

    Texture2D GetReadableTexture(Texture2D texture)
    {
        RenderTexture rt = RenderTexture.GetTemporary(
            texture.width,
            texture.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear);

        Graphics.Blit(texture, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D readableTexture = new Texture2D(texture.width, texture.height);
        readableTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return readableTexture;
    }

    Color GetMostFrequentColor(Sprite sprite)
    {
        Texture2D texture = GetReadableTexture(sprite.texture);
        Color[] pixels = texture.GetPixels();
        Dictionary<Color, int> colorCount = new Dictionary<Color, int>();
        
        foreach (Color color in pixels)
        {
            if (colorCount.ContainsKey(color))
            {
                colorCount[color]++;
            }
            else
            {
                colorCount[color] = 1;
            }
        }

        Color mostFrequentColor = Color.clear;
        int maxCount = 0;
        foreach (KeyValuePair<Color, int> pair in colorCount)
        {
            if (pair.Value > maxCount)
            {
                mostFrequentColor = pair.Key;
                maxCount = pair.Value;
            }
        }
        mostFrequentColor.a = 1;
        return mostFrequentColor;
    }

    private Line AddGameObjectWithLine(Transform container)

    {

        var gameObjectName = "Line";

        var obj = new GameObject(gameObjectName);

        obj.transform.parent = container;

        obj.transform.position = Vector3.zero;

        var geometry = obj.AddComponent<Line>();

        geometry.ShouldContinue = false;

        return geometry;

    }

}
#endif

public class SpriteNameComparer : IComparer<Sprite>
{
    public int Compare(Sprite sprite1, Sprite sprite2)
    {
        return sprite1.name.CompareTo(sprite2.name);
    }
}

public enum FolderType
{
    fillwhite,
    hint,
    line,
    line_full,
    pattern,
    color_1,
    color_2,
    color_3
}