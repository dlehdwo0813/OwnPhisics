using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour {

    public float launchSpeed = 50;
    public ObjectBasic reloadedObject;

	// Use this for initialization
	void Start () {
		
	}
    public void Launch(ref ObjectBasic obj, Vector3 from)
    {
        if (!obj.gameObject.activeInHierarchy)
        {
            obj.gameObject.SetActive(true);
            obj.transform.position = transform.position;
        }

        Rigidbody2D rb2d = obj.GetComponent<Rigidbody2D>();

        if (rb2d)
        {
            Vector2 force = (transform.position - from) * launchSpeed;
            //rb2d.bodyType = RigidbodyType2D.Dynamic;
            //rb2d.AddForce(force, ForceMode2D.Impulse);
            rb2d.bodyType = RigidbodyType2D.Kinematic;
            rb2d.velocity = force;
            Debug.Log(transform.forward.ToString());
        }

    }

    public void Wait()
    {
        reloadedObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + .1f);
    }

    public void Launch(Vector3 from)
    {
        if (!reloadedObject.gameObject.activeInHierarchy)
        {
            reloadedObject.gameObject.SetActive(true);
            reloadedObject.transform.position = transform.position;
        }

        if (reloadedObject.objectState != ObjectState.InAir)
        {
            reloadedObject.StateChange(ObjectState.InAir);
        }
        if ((reloadedObject.objectTag & (int)(ObjectTag.Projectile)) == 0)
            reloadedObject.objectTag += (int)(ObjectTag.Projectile);


        Rigidbody2D rb2d = reloadedObject.GetComponent<Rigidbody2D>();

        if (rb2d)
        {
            Vector2 force = (transform.position - from) * launchSpeed;
            rb2d.AddForce(force, ForceMode2D.Impulse);

        }

    }

}
