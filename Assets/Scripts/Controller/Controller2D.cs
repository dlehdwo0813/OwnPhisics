using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : BoxController
{
    public Vector2 velocity;
    protected bool isOnSomething = false;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        //collisions.rope = false;
    }

    virtual public void Move(Vector2 moveAmount, bool standingOnPlatform)
    {
    }

}
