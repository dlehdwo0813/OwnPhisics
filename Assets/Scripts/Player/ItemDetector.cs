using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ItemDetector : MonoBehaviour {

    CircleCollider2D circle2D;
    public ObjectBasic objectBasic;

    // Use this for initialization
    void Start()
    {

        circle2D = GetComponent<CircleCollider2D>();
        circle2D.isTrigger = true;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ObjectBasic obj = collision.GetComponent<ObjectBasic>();
        if (obj)
        {
            if (!obj.transform.parent)
            {
                if (objectBasic)
                {
                    Vector2 distance = transform.position - objectBasic.transform.position;
                    Vector2 distance2 = transform.position - obj.transform.position;
                    if (distance.magnitude > distance2.magnitude)
                    {
                        objectBasic = obj;
                    }
                }
                else
                {
                    if (!obj.transform.parent)
                        objectBasic = obj;

                }
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ObjectBasic obj = collision.GetComponent<ObjectBasic>();
        if (obj)
        {
            if (objectBasic)
                if (obj.transform == objectBasic.transform)
                {
                    objectBasic = null;
                }
        }

    }



}
