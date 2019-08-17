using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBasic : MonoBehaviour {

    public uint objectTag;


    [System.Serializable]
    public enum ObjectTag
    {
        Player = 1, Box = 2, Circle = 4, Platform = 8
        , Button = 0x10, Damage = 0x20, Projectile = 0x30
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
