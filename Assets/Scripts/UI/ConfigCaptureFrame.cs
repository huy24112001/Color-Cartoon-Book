using UnityEngine;

public class ConfigCaptureFrame : MonoBehaviour
{
    public RectTransform[] UIImageRectTransforms; // The RectTransform of the UI Image
    public Transform worldObject; // The Transform of the world space object
    public Camera worldCamera; // The camera rendering the world space (usually the main camera)
    public Canvas canvas; // The canvas containing the UI Image

    void OnEnable()
    {
        CenterUIElement();
    }

    void CenterUIElement()
    {
        // Convert world position to screen position
        Vector3 screenPosition = worldCamera.WorldToScreenPoint(worldObject.position);

        // Convert screen position to Canvas space
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPosition, canvas.worldCamera, out Vector2 canvasLocalPosition);

        // Set the position of the UI Image RectTransform
        // ratio = height of device / 1920
        float ratio = Screen.height / 1920f;
        UIImageRectTransforms[0].localPosition = canvasLocalPosition + new Vector2(0, 120f * ratio);
        UIImageRectTransforms[1].localPosition = canvasLocalPosition + new Vector2(0, 140f * ratio);
    }
}
