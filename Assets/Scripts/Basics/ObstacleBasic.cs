using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObstacleBasic : ObjectBasic {


    public Rigidbody2D rb2d;
    [Range(0, 20)]
    public float lifeTime = 0.5f;
    protected float lifeTimeTick = 0;

    // Use this for initialization
    public void Start () {
        objectTag = (int)ObjectTag.Circle;
        objectTag += (int)ObjectTag.Damage;
        objectTag += (int)ObjectTag.AttachAble;

        rb2d = GetComponent<Rigidbody2D>();
    }

}
