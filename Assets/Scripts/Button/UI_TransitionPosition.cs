using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TransitionPosition : MonoBehaviour {
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y + 1);
            transform.position = v3;
        }
    }
}
