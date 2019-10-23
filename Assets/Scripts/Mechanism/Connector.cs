using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Connector : MonoBehaviour {

    CircleCollider2D circle2D;

    // Use this for initialization
    void Start () {

        circle2D = GetComponent<CircleCollider2D>();
        if (!circle2D)
        {
            circle2D = gameObject.AddComponent<CircleCollider2D>();
        }
        circle2D.isTrigger = true;
        circle2D.radius = .2f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectBasic obj = collision.GetComponent<ObjectBasic>();
        if (obj)
        {
            if (obj.transform.parent) return;


            if (obj.objectTag.isObjectTagIncluded(ObjectTag.AttachAble))
            {
                obj.transform.parent = transform;
                obj.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + .1f);
                WheelJoint2D joint2D = GetComponent<WheelJoint2D>();
                if (joint2D)
                {
                    Rigidbody2D rb2d = obj.GetComponent<Rigidbody2D>();
                    if (rb2d)
                    {
                        joint2D.connectedBody = rb2d;
                        obj.StateChange(ObjectState.FixedDynamic);
                        if (obj.objectTag.isObjectTagIncluded(ObjectTag.Projectile))
                        {
                            obj.objectTag -= (int)ObjectTag.Projectile;
                        }
                    }
                    else
                        Debug.Log(transform.ToString() + ".OnTriggerEnter2D : !rb2d");
                }
                else
                {
                    Debug.Log("! joint2D");

                }
            }
        }
    }
}
