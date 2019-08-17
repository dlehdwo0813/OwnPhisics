using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {

    public float launchSpeed = 50;

	// Use this for initialization
	void Start () {
		
	}
	public void Launch(ref ObjectBasic obj)
    {
        if (!obj.gameObject.activeInHierarchy)
        {
            obj.gameObject.SetActive(true);
            Vector3 offset = transform.up * transform.localScale.y;
            obj.transform.position = transform.position + offset;
            Debug.Log(obj.transform.position.ToString());
            //obj.transform.localScale = transform.localScale;
        }

        Rigidbody2D rb2d = obj.GetComponent<Rigidbody2D>();

        if (rb2d)
        {
            rb2d.bodyType = RigidbodyType2D.Dynamic;
            rb2d.AddForce(transform.up * launchSpeed, ForceMode2D.Impulse);
        }

    }

    public void InActivateObject(ref ObjectBasic obj)
    {
        obj.gameObject.SetActive(false);
    }

}
