using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathfTriangles
{
    public static bool IsPointInTriangleXY(Vector3 point, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return IsInside(point.x, point.y, p1.x, p1.y, p2.x, p2.y, p3.x, p3.y);
    }

    private static bool IsInside(float x, float y, float x1, float y1, float x2, float y2, float x3, float y3)
    {
        float l1 = (x - x1) * (y3 - y1) - (x3 - x1) * (y - y1),
          l2 = (x - x2) * (y1 - y2) - (x1 - x2) * (y - y2),
          l3 = (x - x3) * (y2 - y3) - (x2 - x3) * (y - y3);
        return (l1 > 0 && l2 > 0 && l3 > 0) || (l1 < 0 && l2 < 0 && l3 < 0);
    }
}
