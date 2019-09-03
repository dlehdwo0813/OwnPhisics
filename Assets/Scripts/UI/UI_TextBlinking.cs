using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UI_TextBlinking : MonoBehaviour {

    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine("Blinking");
    }

    IEnumerator Blinking()
    {

        while (text)
        {
            float rgb = Mathf.Sin(Time.time);
            Color col = new Color(rgb, rgb, rgb);
            text.color = col;

            yield return new WaitForFixedUpdate();
        }

        StopCoroutine("Blinking");
    }

    private void OnDisable()
    {
        StopCoroutine("Blinking");

    }
}
