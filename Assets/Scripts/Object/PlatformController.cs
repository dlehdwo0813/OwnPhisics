using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MovementCalculator))]
public class PlatformController : BoxController2D
{
    MovementCalculator movementCalculator;


    public override void Start()
    {
        base.Start();
        movementCalculator = GetComponent<MovementCalculator>();

        objectTag += (int)ObjectTag.Platform;
    }

    private void FixedUpdate()
    {

        UpdateRaycastOrigins();

        velocity = movementCalculator.CalculatePlatformMovement();

        Move(velocity, false);

        velocity /= Time.deltaTime;
    }




}
