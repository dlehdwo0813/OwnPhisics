using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extentions
{
    public static Vector2 RotateByTwoVectorsAngle(this Vector2 vec, Vector2 lhs, Vector2 rhs)
    {
        Vector2 vector2;

        float cross = Vector3.Cross(lhs, rhs).z;
        float cosT = Vector2.Dot(lhs, rhs) / (lhs.magnitude * rhs.magnitude);

        float theta = Mathf.Acos(cosT) * Mathf.Sign(cross);

        float cosA = Mathf.Cos(theta);
        float sinA = Mathf.Sin(theta);


        vector2
             = new Vector2(
                vec.x * cosA - vec.y * sinA,
                vec.x * sinA + vec.y * cosA);

        return vector2;
    }


    public static float Ease(this float x, float easeAmount = 0)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    public static Vector3 EaseTranslate(this Vector3 start, Vector3 destination, float speed, ref float percentBetweenWaypoints, float easeAmount = 0)
    {
        float distanceBetweenWaypoints = Vector3.Distance(start, destination);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
        percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
        float easedPercentBetweenWaypoints = percentBetweenWaypoints.Ease(easeAmount);

        Vector3 newPos = Vector3.Lerp(start, destination, easedPercentBetweenWaypoints)/* - start*/;

        return newPos;
    }

    public static int ObjectTagToInt(this ObjectTag objt)
    {
        return (int)objt;
    }

    public static bool isObjectTagIncluded(this uint i, ObjectTag objt)
    {
        return ((uint)objt & i) > 0 ? true : false;
    }

}