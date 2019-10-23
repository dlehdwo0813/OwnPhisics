using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BoxObject : BoxController2D
{

    [HideInInspector]
    public Rigidbody2D rb2D;
    public float gravityScale = 1.0f;


    public override void Awake()
    {
        base.Awake();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public override void Start()
    {
        base.Start();
        rb2D = GetComponent<Rigidbody2D>();
    }


}
