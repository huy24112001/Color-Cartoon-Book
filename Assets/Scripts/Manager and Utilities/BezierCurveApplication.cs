using UnityEngine;

public static class BezierCurveApplication
{
    public static Vector3 CubicBezier(Vector3 Start, Vector3 P1, Vector3 P2, Vector3 end, float t)
    {
        return (1f - t) * QuadraticBezier(Start, P1, P2, t) + t * QuadraticBezier(P1, P2, end, t);
    }

    public static Vector3 QuadraticBezier(Vector3 start, Vector3 P1, Vector3 end, float t)
    {
        return (1f - t) * LinearBezier(start, P1, t) + t * LinearBezier(P1, end, t);
    }

    public static Vector3 LinearBezier(Vector3 start, Vector3 end, float t)
    {
        return (1f - t) * start + t * end;
    }
}
