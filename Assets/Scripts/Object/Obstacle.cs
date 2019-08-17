using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : ObjectBasic {

	// Use this for initialization
	void Start () {
        objectTag = (int)ObjectTag.Circle;
        objectTag += (int)ObjectTag.Damage;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
