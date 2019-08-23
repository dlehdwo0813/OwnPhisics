using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType
{
    Touch = 0, Stay, Destroy, Collect
}

[RequireComponent(typeof(BoxCollider2D))]
public class ButtonBasic : ObjectBasic {

    public BoxController2D box2D;
    public ButtonType buttonType = ButtonType.Touch;

	// Use this for initialization
	public void Start () {
        box2D = GetComponent<BoxController2D>();

    }

    virtual public void EventStart()
    {

    }
    virtual public void EventEnd()
    {

    }
}
