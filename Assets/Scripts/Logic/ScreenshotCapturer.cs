using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

public class ScreenshotCapturer : MonoBehaviour
{
    [SerializeField] Camera UICamera;
    [SerializeField] CameraController cameraController;
    Texture2D texture;
    [SerializeField] RectTransform captureArea;
    private byte[] imageBytes;
    private string fileName = "";
    private string path = "";

    [Button("CaptureScreenshot")]
    public void CaptureScreenshot()
    {
        if (cameraController.posBoundLevel != Vector3.zero)
        {
            Vector3 p = cameraController.posBoundLevel;
            p.x -= captureArea.transform.position.x;
            p.z = -10f;
            p.y -= (captureArea.transform.position.y);
            p += UICamera.transform.position;

            UICamera.transform.DOMove(p, 0.5f).OnComplete(() =>
            {
                StartCoroutine(TakeScreenshotAndSaveToArchive());
            });
        }
        else
        {
            StartCoroutine(TakeScreenshotAndSaveToArchive());
        }
    }

    IEnumerator TakeScreenshotAndSaveToArchive()
    {
        yield return new WaitForEndOfFrame();

        // Calculate the Rect area to capture based on the UI element
        Vector3[] worldCorners = new Vector3[4];
        captureArea.GetWorldCorners(worldCorners);
        //Debug.Log("screenArea center 2 : " + captureArea.transform.position);

        // Convert the corners to screen space
        Vector3 screenBottomLeft = Camera.main.WorldToScreenPoint(worldCorners[0]);
        Vector3 screenTopRight = Camera.main.WorldToScreenPoint(worldCorners[2]);

        // Calculate the captureRect relative to the bottom-left corner of the screen
        Rect captureRect = new Rect(
            screenBottomLeft.x,
            screenBottomLeft.y,
            screenTopRight.x - screenBottomLeft.x,
            screenTopRight.y - screenBottomLeft.y
        );

        // Adjust for screen resolution and aspect ratio
        float screenWidthRatio = Screen.width / (float)UICamera.pixelWidth;
        float screenHeightRatio = Screen.height / (float)UICamera.pixelHeight;
        captureRect.x *= screenWidthRatio;
        captureRect.y *= screenHeightRatio;
        captureRect.width *= screenWidthRatio;
        captureRect.height *= screenHeightRatio;

        // Ensure the width and height are positive
        captureRect.width = Mathf.Abs(captureRect.width);
        captureRect.height = Mathf.Abs(captureRect.height);
        //Debug.Log("here " + captureRect.width);
        //Debug.Log("here " + captureRect.height);

        texture = new Texture2D((int)captureRect.width, (int)captureRect.height, TextureFormat.RGB24, false);
        texture.ReadPixels(captureRect, 0, 0);
        texture.LoadRawTextureData(texture.GetRawTextureData());
        texture.Apply();
        yield return new WaitForEndOfFrame();

        //Convert to bytes
        imageBytes = texture.EncodeToJPG(128);
        fileName = "cacb_archive_pic" + (PlayerData.CurrentLevel) + ".png";
        path = Application.persistentDataPath + "/" + fileName;

        File.WriteAllBytes(path, imageBytes);
       
        GameManager.Instance.PlayCongratsVFX();
    }
}