using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform target;
    [SerializeField] float speed = 1f;
    Vector3 offset;
    [SerializeField] RectTransform screenArea;
    public Vector3 posBoundLevel = Vector3.zero;

    public float firstOrthographicSize = 0f;
    public Vector3 firstPositionCamera = Vector3.zero;
    public Rect? rectBoundingBox = null;
    public Transform penObject;
    public Transform Target { get => target; set => target = value; }

    void Start()
    {
        if (target != null)
        {
            firstPositionCamera = mainCamera.transform.position;
            offset = mainCamera.transform.position - target.position;
        }
    }

    public void LookAtNewTarget(Transform newTarget)
    {
        target = newTarget;

        Bounds bounds = new Bounds(newTarget.position, Vector3.zero);
        foreach (SpriteRenderer renderer in newTarget.GetComponentsInChildren<SpriteRenderer>())
        {
            bounds.Encapsulate(renderer.bounds);
        }

        float newSize = mainCamera.orthographicSize * CalculatorScaleOrthographicSize1(bounds);
        mainCamera.DOOrthoSize(newSize, speed);

        float sc = newSize / 17f;
        penObject.localScale = Vector3.one * 0.75f * sc;

        Vector3 pos = target.position;
        pos.z = -10f;
        mainCamera.transform.DOMove(pos, speed);
    }

    public void MoveToOriginTarget()
    {
        //Debug.Log("MoveToOriginTarget");
        penObject.localScale = Vector3.one * 0.75f * firstOrthographicSize / 17f ;
        mainCamera.transform.DOMove(firstPositionCamera, speed);
        mainCamera.DOOrthoSize(firstOrthographicSize, speed);
    }

    public void OnEndChangeOrthoSize(Bounds bounds)
    {
        posBoundLevel = bounds.center;
        float scaleOrthographicSize = CalculatorScaleOrthographicSize(bounds, screenArea);
        Vector3 p = bounds.center;
        p.x = 0f;
        p.z = -10f;
        p.y -= (screenArea.transform.position.y);

        //Debug.Log("screenArea center : " + screenArea.transform.position);
        //Debug.Log("screenArea center 1 : " + bounds.center);

        mainCamera.transform.DOMove(p, speed);
      
        mainCamera.DOOrthoSize(mainCamera.orthographicSize * scaleOrthographicSize , speed);
    }

    public float CalculatorScaleOrthographicSize1(Bounds bounds)
    {
        if (rectBoundingBox.HasValue)
        {
            Rect boundingBox = rectBoundingBox.Value;
            // Sử dụng boundingBox ở đây


            Vector3[] worldCorners = new Vector3[4];
            worldCorners[0] = bounds.min;
            worldCorners[1] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
            worldCorners[2] = bounds.max;
            worldCorners[3] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);

            // Chuyển đổi các điểm biên từ không gian thế giới sang không gian màn hình
            Vector3[] screenCorners = new Vector3[4];
            for (int i = 0; i < 4; i++)
            {
                screenCorners[i] = mainCamera.WorldToScreenPoint(worldCorners[i]);
            }

            // Tính toán kích thước trong không gian màn hình
            float width = screenCorners[2].x - screenCorners[0].x;
            float height = screenCorners[1].y - screenCorners[3].y;
            float scaleOld = width / height;
            //Debug.Log("width " + width);
            //Debug.Log("height " + height);
            //Tính toán kích thước của ô vuông UI



            float uiSquareWidth = boundingBox.width * 0.7f;
            float uiSquareHeight = boundingBox.height * 0.7f;

            //Debug.Log("width 1 " + rect.width);
            //Debug.Log("height 1 " + rect.height);

            float widthNew = uiSquareWidth;
            float heightNew = widthNew / scaleOld;

            if (heightNew > uiSquareHeight)
            {
                heightNew = uiSquareHeight;
                widthNew = heightNew * scaleOld;

            }

            return width / widthNew;
        }
        else
            return 1;
    }

    public float CalculatorScaleOrthographicSize(Bounds bounds, RectTransform rectTransform)
    {

        Vector3[] worldCorners = new Vector3[4];
        worldCorners[0] = bounds.min;
        worldCorners[1] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        worldCorners[2] = bounds.max;
        worldCorners[3] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);

        // Chuyển đổi các điểm biên từ không gian thế giới sang không gian màn hình
        Vector3[] screenCorners = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            screenCorners[i] = mainCamera.WorldToScreenPoint(worldCorners[i]);
        }

        // Tính toán kích thước trong không gian màn hình
        float width = screenCorners[2].x - screenCorners[0].x;
        float height = screenCorners[1].y - screenCorners[3].y;
        float scaleOld = width / height;
        //Debug.Log("width " + width);
        //Debug.Log("height " + height);
        //Tính toán kích thước của ô vuông UI

        Rect rect  = ConvertSizeRect(rectTransform);

        float uiSquareWidth = rect.width * 0.85f;
        float uiSquareHeight = rect.height * 0.85f;

        //Debug.Log("width 1 " + rect.width);
        //Debug.Log("height 1 " + rect.height);

        float widthNew = uiSquareWidth;
        float heightNew = widthNew / scaleOld;

        if (heightNew > uiSquareHeight)
        {
            heightNew = uiSquareHeight;
            widthNew = heightNew * scaleOld;

        }

        return width/widthNew;
    }

    public Rect ConvertSizeRect(RectTransform rect)
    {
        Vector3[] worldCorners = new Vector3[4];
        rect.GetWorldCorners(worldCorners);

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
        float screenWidthRatio = Screen.width / (float)mainCamera.pixelWidth;
        float screenHeightRatio = Screen.height / (float)mainCamera.pixelHeight;
        captureRect.x *= screenWidthRatio;
        captureRect.y *= screenHeightRatio;
        captureRect.width *= screenWidthRatio;
        captureRect.height *= screenHeightRatio;

        captureRect.width = Mathf.Abs(captureRect.width)*1.1f/3f;
        captureRect.height = Mathf.Abs(captureRect.height)*1.1f/3f;
        Debug.Log("screenArea center 2 : " + captureRect.center);

        return captureRect;

    }

}
