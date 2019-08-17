using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    [SerializeField]
    Launcher launcher;
    [SerializeField]
    ObjectPool ammo;


	// Use this for initialization
	void Start () {
        launcher = GetComponentInChildren<Launcher>();
        ammo = GetComponent<ObjectPool>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            ObjectBasic obj = ammo.GetInActivatedObject();
            if (obj)
            {
                launcher.Launch(ref obj);
            }
        }
	}
}
