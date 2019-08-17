using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BoxController : RaycastController
{
    [HideInInspector]
    public int horizontalRayCount;
    [HideInInspector]
    public int verticalRayCount;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;


    [HideInInspector]
    public BoxCollider2D boxCollider;

    public RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;

    public override void Awake()
    {
        base.Awake();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public override void Start()
    {
        base.Start();
        collisions.faceDir = 1;
        CalculateRaySpacing();
    }

    //public Vector3 vel; // 디버그 위함
    //private void Update()
    //{
    //    Vector3 velocity = vel * Time.deltaTime;
    //    UpdateBoxController(velocity);
    //    CollisionCheckAll(velocity, collisionMask);
    //    CollisionCheckAll(velocity, collisionMask, false);
    //}

    public void UpdateBoxController(Vector2 moveAmount)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.moveAmountOld = moveAmount;

        if (moveAmount.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(moveAmount.x);
        }

    }

    public override void UpdateRaycastOrigins()
    {
        Vector2 center = transform.position;
        Vector2 size = boxCollider.bounds.size / 2;
        Vector2 scale = transform.localScale;
        size.x -= skinWidth;
        size.y -= skinWidth;

        raycastOrigins.bottomLeft
    = raycastOrigins.bottomRight
    = raycastOrigins.topLeft
    = raycastOrigins.topRight = boxCollider.bounds.center;

        Vector2 bl = new Vector2(- size.x, - size.y);
        Vector2 br = new Vector2(+ size.x, - size.y);
        Vector2 tl = new Vector2(- size.x, + size.y);
        Vector2 tr = new Vector2(+ size.x, + size.y);

        raycastOrigins.bottomLeft
            += bl.RotateByTwoVectorsAngle(Vector2.up, transform.up);

        raycastOrigins.bottomRight
            += br.RotateByTwoVectorsAngle(Vector2.up, transform.up);

        raycastOrigins.topLeft
            += tl.RotateByTwoVectorsAngle(Vector2.up, transform.up);

        raycastOrigins.topRight
            += tr.RotateByTwoVectorsAngle(Vector2.up, transform.up);

    }

    public override void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRays);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public RaycastHit2D CollisionCheck(Vector2 moveAmount, int index, LayerMask layerMask, bool isHorizontal = true)
    {
        Vector2 dir;
        float rayLength;
        Vector2 rayOrigin;

        if (isHorizontal)
        {
            if (collisions.faceDir == 1)
            {
                rayOrigin = raycastOrigins.bottomRight;
                dir = transform.right;

            }
            else
            {
                rayOrigin = raycastOrigins.bottomLeft;
                dir = -transform.right;
            }

            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

            Vector2 up = transform.up;
            rayOrigin += (up * horizontalRaySpacing) * index;
        }
        else
        {
            int yDir = (int)Mathf.Sign(moveAmount.y);
            if (yDir == 1)
            {
                rayOrigin = raycastOrigins.topLeft;
                dir = transform.up;

            }
            else
            {
                rayOrigin = raycastOrigins.bottomLeft;
                dir = -transform.up;
            }

            rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

            Vector2 right = transform.right;
            rayOrigin += right * (verticalRaySpacing * index + moveAmount.x);
        }

        Color col = Color.red;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, dir, rayLength, layerMask);

        if (hit)
            col = Color.green;

        Debug.DrawRay(rayOrigin, dir * rayLength, col);

        return hit;
    }

    public RaycastHit2D[] CollisionCheckAll(Vector2 moveAmount, LayerMask layerMask, bool isHorizontal = true)
    {


        int rayCount = 0;

        if (isHorizontal)
            rayCount = horizontalRayCount;
        else
            rayCount = verticalRayCount;

        RaycastHit2D[] hit = new RaycastHit2D[rayCount];

        for (int i = 0; i < rayCount; i++)
        {
            hit[i] = CollisionCheck(moveAmount, i, layerMask, isHorizontal);
        }

        return hit;
    }

    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    [System.Serializable]
    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public bool slidingDownMaxSlope;

        public float slopeAngle, slopeAngleOld;
        public Vector2 slopeNormal;
        public Vector2 moveAmountOld;
        public int faceDir;
        public bool fallingThroughPlatform;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slidingDownMaxSlope = false;
            slopeNormal = Vector2.zero;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;

        }

    }

}
