using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    Player player;

    ItemDetector itemDetector;
    [SerializeField]
    ObjectBasic objectBasic;
    public float distance = .7f;
    int dir = 1;

    // Use this for initialization
    void Start () {
        itemDetector = GetComponent<ItemDetector>();
        player = GetComponentInParent<Player>();

    }

    private void FixedUpdate()
    {
        if (!player)
            return;
        if (!itemDetector)
            return;

        if (Input.GetKeyDown(KeyCode.G))
        {
            GrabObject();
        }

        MoveAttachedObject();

        ThrowObject();
    }

    void MoveAttachedObject()
    {
        if (objectBasic && objectBasic.transform.parent == transform)
        {
            dir = -player.controller.collisions.faceDir;

            objectBasic.transform.position
                = new Vector3(transform.position.x /*+ dir * distance*/, transform.position.y + distance, transform.position.z + .2f);
        }
    }

    public Vector2 throwStart;
    public Vector2 throwEnd;
    public bool onDrawLine = false;


    void ThrowObject()
    {

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Vector3 v3 = Input.mousePosition;
            v3.z = 10.0f;
            if (!onDrawLine)
            {
                throwStart = Camera.main.ScreenToWorldPoint(v3);
                onDrawLine = true;
            }

            throwEnd = Camera.main.ScreenToWorldPoint(v3);
        }
        else
        {
            if (onDrawLine)
            {
                onDrawLine = false;

                if (objectBasic && objectBasic.transform.parent == transform)
                {

                    objectBasic.transform.parent = null;

                    Vector2 v2 = throwStart - throwEnd;
                    objectBasic.StateChange(ObjectState.Free);
                    objectBasic.AddVelocity(v2 * 2, ForceMode2D.Impulse);

                    objectBasic = null;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (onDrawLine)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(throwStart,new Vector2(.1f,.1f));

            Gizmos.DrawLine(throwStart, throwEnd);
        }
    }

    public void GrabObject()
    {
        if (itemDetector.objectBasic)
        {
            objectBasic = itemDetector.objectBasic;
            itemDetector.objectBasic = null;

            objectBasic.transform.parent = transform;
            objectBasic.StateChange(ObjectState.FixedKinematic);

        }
    }

    public void Flip(Vector2 position)
    {
        if (itemDetector.objectBasic && itemDetector.objectBasic.transform.parent)
        {
            Vector2 pos = transform.position = position;
            itemDetector.objectBasic.transform.position = position - pos;
        }
    }
}
