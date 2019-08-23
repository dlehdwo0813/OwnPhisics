using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : ObstacleBasic {


    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        OnStateChanged();

        if (objectTag.isObjectTagIncluded(ObjectTag.Projectile))
        {
            lifeTimeTick += Time.deltaTime;
            if (lifeTimeTick > lifeTime)
            {
                lifeTimeTick = 0;
                objectTag -= (int)ObjectTag.Projectile;
                gameObject.SetActive(false);
            }
        }
        else
        {
            rb2d.gravityScale = 1;
        }
    }

    override protected void OnStateChanged()
    {
        if (!rb2d)
            return;

        switch (objectState)
        {
            case ObjectState.Free:
                {
                    rb2d.bodyType = RigidbodyType2D.Dynamic;
                    rb2d.gravityScale = 1;
                }
                break;
            case ObjectState.FixedKinematic:
                {
                    rb2d.bodyType = RigidbodyType2D.Kinematic;
                    rb2d.gravityScale = 0;
                }
                break;
            case ObjectState.FixedDynamic:
                {
                    rb2d.bodyType = RigidbodyType2D.Dynamic;
                    rb2d.gravityScale = 1;
                }
                break;
            case ObjectState.InAir:
                {
                    rb2d.bodyType = RigidbodyType2D.Dynamic;
                    rb2d.gravityScale = 0;
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StateChange(ObjectState.Free);
        //if (((int)ObjectTag.Projectile & objectTag) > 0)
        //{
        //    ObjectBasic obj = collision.transform.GetComponent<ObjectBasic>();
        //    if (obj)
        //    {
        //    }
        //}
    }
}
