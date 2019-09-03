using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType
{
    Touch = 0, Stay, Destroy, Collect
}
public enum ButtonEvent
{
    Activate = 0, DeActivate, Operate
}

[RequireComponent(typeof(BoxCollider2D))]
public class ButtonBasic : ObjectBasic {

    public BoxCollider2D box2D;
    public ButtonType buttonType = ButtonType.Touch;

    [System.Serializable]
    public struct ObjectEvent
    {
        public ButtonEvent buttonEvent;
        public ObjectBasic objectBasic;
    }

    [SerializeField]
    public List<ObjectEvent> orderList;

	// Use this for initialization
	public void Start () {
        box2D = GetComponent<BoxCollider2D>();
        box2D.isTrigger = true;
    }

    virtual public void EventStart()
    {

    }

    virtual public void EventEnd()
    {

    }

    virtual public void DeActivateObject(ObjectBasic obj)
    {
        obj.DeActivate();
    }

    virtual public void ActivateObject(ObjectBasic obj)
    {

    }

    virtual public void OperateObject(ObjectBasic obj)
    {

    }

    virtual public void DeActivateObject()
    {
        for(int i=0;i< orderList.Count; i++)
        {
            if(orderList[i].buttonEvent == ButtonEvent.DeActivate)
            {
                if (orderList[i].objectBasic)
                    orderList[i].objectBasic.DeActivate();
            }

        }
    }

    virtual public void ActivateObject()
    {
        for (int i = 0; i < orderList.Count; i++)
        {
            if (orderList[i].buttonEvent == ButtonEvent.Activate)
            {
                if (orderList[i].objectBasic)
                    orderList[i].objectBasic.Activate();
            }

        }

    }

    virtual public void OperateObject()
    {

    }
}
