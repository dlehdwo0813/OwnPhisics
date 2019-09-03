using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PopUp : MonoBehaviour {

    public Vector2 size;
    public Vector2 targetSize = Vector2.one;
    public float timer = 1;

	// Use this for initialization
	void Start () {
    }


    public void StartPopUp()
    {
        StartCoroutine("PopUp");
    }

    public void EndPopUp()
    {
        StartCoroutine("PopUpEnd");
    }

    IEnumerator PopUp()
    {
        float t = Time.time;

        while (Time.time - t < timer)
        {
            float lt = (1 - timer) + Time.time - t;
            lt = Mathf.Cos(lt - .8f) + .05f;
            transform.localScale = Vector3.LerpUnclamped(size, targetSize,lt);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator PopUpEnd()
    {
        float t = Time.time;

        while (Time.time - t < timer)
        {
            transform.localScale = Vector3.LerpUnclamped(targetSize, size, (1 - timer) + Time.time - t);
            yield return new WaitForFixedUpdate();
        }
        gameObject.SetActive(false);
    }

}
