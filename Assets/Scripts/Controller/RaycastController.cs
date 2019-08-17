using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : ObjectBasic {

    public LayerMask collisionMask;


    public float skinWidth = .015f;
    public float dstBetweenRays = .1f;


    public virtual void Awake()
    {
    }

    public virtual void Start()
    {
        CalculateRaySpacing();
    }

    public virtual void UpdateRaycastOrigins()
    {
    }

    public virtual void CalculateRaySpacing()
    {
    }

}
