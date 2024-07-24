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
    public string folderPath = "E:/Unity/Color-CartoonBook-Asmr/Assets/Resources/cartoon-book-level/line_full";


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
        //string[] fileEntries = Directory.GetFiles(folderPath)
        //                        .Where(file => file.EndsWith(".png") || file.EndsWith(".jpg") || file.EndsWith(".jpeg"))
        //                        .ToArray();

        //var sortedFileEntries = fileEntries.OrderBy(file => ExtractNumberFromFileName(file)).ToArray();

        //int index = 0;
        //foreach (string filePath in sortedFileEntries)
        //{
        //    Debug.Log(filePath);

        //    StartCoroutine(LoadImage1(filePath, levelItems[index].transform.GetChild(0).GetComponent<RawImage>()));
        //    //break; // Dừng lại sau khi gán hình ảnh đầu tiên, bỏ qua dòng này nếu muốn tiếp tục với nhiều hình ảnh
        //    index += 1;

        //}
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

    int ExtractNumberFromFileName(string fileName)
    {
        // Biểu thức chính quy để trích xuất số từ tên tệp
        Match match = Regex.Match(fileName, @"\d+");
        return match.Success ? int.Parse(match.Value) : 0;
    }

    System.Collections.IEnumerator LoadImage1(string filePath, RawImage imageComponent)
    {
        // Tải hình ảnh từ file
        byte[] byteArray = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(byteArray);

        // Tạo Sprite từ Texture2D
        //Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // Gán Sprite cho đối tượng Image
        imageComponent.texture = texture;

        yield return null;
    }
}
