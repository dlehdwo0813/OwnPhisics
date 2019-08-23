using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCalculator : MonoBehaviour {

    public List<Vector3> localWaypoints;
    List<Vector3> globalWaypoints;
    public Transform objToMove;

    public float speed;
    public bool cyclic;
    public float waitTime;
    [Range(0, 2)]
    public float easeAmount;

    public int fromWaypointIndex;
    float percentBetweenWaypoints;
    float nextMoveTime;

    private bool isIndexChanged = false;
    public bool Get_b_isIndexChanged()
    {
        return isIndexChanged;
    }


    // Use this for initialization
    void Start () {
        globalWaypoints = new List<Vector3>();
        SetWaypoints();
    }

    void SetWaypoints()
    {
        if (globalWaypoints.Count != localWaypoints.Count)
        {
            for (int i = globalWaypoints.Count; i < localWaypoints.Count; i++)
            {
                globalWaypoints.Add(localWaypoints[i] + transform.position);
            }
        }

        isIndexChanged = false;
    }
    public Vector3 CalculatePlatformMovement()
    {
        SetWaypoints();

        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }
        if (globalWaypoints.Count == 0)
        {
            return Vector3.zero;
        }

        fromWaypointIndex %= globalWaypoints.Count;
        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Count;

        Vector3 newPos = globalWaypoints[fromWaypointIndex].EaseTranslate(globalWaypoints[toWaypointIndex], speed, ref percentBetweenWaypoints, easeAmount);

        if (percentBetweenWaypoints >= 1)
        {
            isIndexChanged = true;
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;

            if (!cyclic)
            {
                if (fromWaypointIndex >= globalWaypoints.Count - 1)
                {
                    fromWaypointIndex = 0;
                    globalWaypoints.Reverse();
                }
            }
            nextMoveTime = Time.time + waitTime;
        }

        return newPos - transform.position;
    }

    public Vector3 CalculatePlatformMovement(Transform fromTransform, Transform toTransform)
    {
        SetWaypoints();

        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }
        if (globalWaypoints.Count == 0)
        {
            return Vector3.zero;
        }

        int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Count;

        Vector3 newPos = fromTransform.position.EaseTranslate(toTransform.position, speed, ref percentBetweenWaypoints, easeAmount);

        if (percentBetweenWaypoints >= 1)
        {
            isIndexChanged = true;
            percentBetweenWaypoints = 0;

            nextMoveTime = Time.time + waitTime;
        }

        return newPos;
    }

    void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = .3f;

            for (int i = 0; i < localWaypoints.Count; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }



}
