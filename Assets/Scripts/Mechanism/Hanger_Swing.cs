using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint2D))]
public class Hanger_Swing : MonoBehaviour {

    HingeJoint2D hj2D;
    public float motorSpeed = 150;
    public float degMax = 80;
    public int lean = 4;
    int dir = 1;

    // Use this for initialization
    void Start () {
        hj2D = GetComponent<HingeJoint2D>();
	}
	
	// Update is called once per frame
    private void FixedUpdate()
    {

        Rigidbody2D connectedBody = hj2D.connectedBody;
        if (degMax != hj2D.limits.max)//필요 없으나 에디터 뷰에서 정확한 움직임 확인 가능
        {
            JointAngleLimits2D limit = hj2D.limits;
            limit.max = degMax;
            limit.min = -degMax;
            hj2D.limits = limit;
        }

        if (Mathf.Abs(connectedBody.rotation) >= degMax)
        {
            dir = -(int)Mathf.Sign(connectedBody.rotation);
            hj2D.connectedBody.rotation = degMax * (-dir);
        }


        JointMotor2D motor = hj2D.motor;

        float percentOfSwing = lean * (-Mathf.Pow(connectedBody.rotation / degMax, 2) + 1.1f);
        float spd = dir * motorSpeed * percentOfSwing;


        motor.motorSpeed = spd;
        hj2D.motor = motor;


    }
}
