using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    [SerializeField] int id;
    [SerializeField] RawImage rawImage;
    string path = "";
    byte[] bytes;
    Texture2D texture;

    public int ID { get => id; set => id = value; }

    public void LoadImage()
    {
        path = Application.persistentDataPath + "/cacb_archive_pic" + (id + 1) + ".png";
        Debug.Log(path);
        bytes = File.ReadAllBytes(path);

        // Create a texture.
        texture = new Texture2D(2, 2);

        // Load the image
        texture.LoadImage(bytes);

        // Set the texture to the RawImage component
        rawImage.texture = texture;

        // adjust the size of the image
        rawImage.rectTransform.sizeDelta = new Vector2(330 , 330);
    }
}
