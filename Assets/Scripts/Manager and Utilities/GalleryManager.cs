using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using System.Text.RegularExpressions;

public class GalleryManager : MonoBehaviour
{
    [SerializeField] List<LevelItem> levelItems;
    string directoryPath = "";
    private RawImage rawImage;
    void Awake()
    {
        //for (int i = 0; i < levelItems.Count; i++)
        //{
        //    rawImage = levelItems[i].transform.GetChild(0).GetComponent<RawImage>();
        //    MaintainAspect(rawImage);

        //}


   

        directoryPath = Application.persistentDataPath + "/";
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        //Debug.Log("direct " + directoryPath);
        if (PlayerData.HasPlayed == 0) return;

        int tmpHighestLevel = PlayerData.HighestLevel - 1;
        if (PlayerData.HighestLevel == 20)
            tmpHighestLevel++;
        Debug.Log("tmp " + tmpHighestLevel);
        for (int i = 0; i < tmpHighestLevel; i++)
        {
            levelItems[i].LoadImage();
        }

   

    }
  

    void MaintainAspect(RawImage rawImage)
    {
        Texture texture = rawImage.texture;
        if (texture == null)
            return;

        RectTransform rectTransform = rawImage.GetComponent<RectTransform>();

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

        void OnEnable()
    {
        if (PlayerData.HasPlayed == 0) return;

        int tmpHighestLevel = PlayerData.HighestLevel - 1;
        if (PlayerData.HighestLevel == 20)
            tmpHighestLevel++;
        //Debug.Log("FFF " + tmpHighestLevel);

        for (int i = 0; i < tmpHighestLevel; i++)
        {
            levelItems[i]. LoadImage();
            levelItems[i].GetComponent<Button>().interactable = true;
            levelItems[i].gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
