using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


[ExecuteAlways]
public class UpdateRawImageInEditor : MonoBehaviour
{
    [SerializeField] List<LevelItem> levelItems; // Mảng chứa các đối tượng cha


    void OnEnable()
    {
        UpdateRawImages();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        UpdateRawImages();
    }
#endif

    void UpdateRawImages()
    {
        
        if (levelItems == null) return;

        foreach (LevelItem levelItem in levelItems)
        {
            if (levelItem != null && levelItem.transform.childCount > 0)
            {
                RawImage rawImage = levelItem.transform.GetChild(0).GetComponent<RawImage>();
                if (rawImage != null)
                {
                    MaintainAspect(rawImage);
                }
            }
        }
    }

    void MaintainAspect(RawImage rawImage)
    {
        Texture texture = rawImage.texture;
        if (texture == null)
            return;

        RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
        if (rectTransform == null)
            return;

        float textureRatio = (float)texture.width / texture.height;
        float rectRatio = rectTransform.rect.width / rectTransform.rect.height;

        if (textureRatio > rectRatio)
        {
            // Width is the limiting factor
            float height = rectTransform.rect.width / textureRatio;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
        else
        {
            // Height is the limiting factor
            float width = rectTransform.rect.height * textureRatio;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }
    }


}
