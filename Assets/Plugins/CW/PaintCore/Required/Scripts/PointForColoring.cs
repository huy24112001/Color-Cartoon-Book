using UnityEngine;

public class PointForColoring : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform point;

    public Vector3 PointPosition { get => mainCamera.WorldToScreenPoint(point.position); }

    public static PointForColoring Instance;

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
}

