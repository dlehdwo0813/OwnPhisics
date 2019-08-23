using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum ObjectTag
{
    Player = 1, Box = 2, Circle = 4, Platform = 8
    , Button = 0x10, Damage = 0x20, Projectile = 0x40
}

[System.Serializable]
public enum ObjectState
{
    Free = 1, FixedKinematic = 2, FixedDynamic = 4, InAir = 8
    , o = 0x10, m = 0x20, e = 0x40
}

public class ObjectBasic : MonoBehaviour {

    public uint objectTag = 0;
    public ObjectState objectState;
    protected ObjectState lastObjState;

    // Use this for initialization
    void Start () {
		
	}
	
    virtual public void StateChange(ObjectState objState)
    {
        lastObjState = objectState;
        objectState = objState;

        OnStateChanged();
    }

    virtual protected void OnStateChanged()
    {

    }

}
