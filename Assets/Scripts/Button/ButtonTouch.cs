using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTouch : ButtonBasic {

	// Use this for initialization
	new void Start () {
        base.Start();
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectBasic obj = collision.transform.GetComponent<ObjectBasic>();
        if (obj)
        {
            if(obj.objectTag.isObjectTagIncluded(ObjectTag.Box) || obj.objectTag.isObjectTagIncluded(ObjectTag.Player))
                DeActivateObject();
            Debug.Log(obj.ToString() + " : is on");
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        ObjectBasic obj = collision.transform.GetComponent<ObjectBasic>();
        if (obj)
        {
            if (obj.objectTag.isObjectTagIncluded(ObjectTag.Box) || obj.objectTag.isObjectTagIncluded(ObjectTag.Player))
                ActivateObject();
            Debug.Log(obj.ToString() + " : is exit");
        }
    }

}
