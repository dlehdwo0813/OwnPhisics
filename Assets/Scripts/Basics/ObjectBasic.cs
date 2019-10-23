﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public enum ObjectTag
{
    Player = 1, Box = 2, Circle = 4, Platform = 8
    , Button = 0x10, Damage = 0x20, Projectile = 0x40, AttachAble = 0x80
}

[System.Serializable]
public enum ObjectState
{
    Free = 1, FixedKinematic = 2, FixedDynamic = 4, Projectile = 8
    , Air = 0x10, m = 0x20, e = 0x40
}

public interface IDamageAble
{
    
    void Attack(IDamageAble ida);
    void GetDamage(int damage);


    //preset 

    //#region IDamageAble

    //public int iMaxHP = 50;
    //public int iCurrentHP = 50;


    //public void Attack(IDamageAble ida)
    //{
    //    //이펙트
    //    //사운드
    //    //ida.GetDamage(iMaxHP);
    //}
    //public void GetDamage(int damage)
    //{
    ////    iCurrentHP -= damage;
    //    if (iCurrentHP <= 0)
    //    {
    //        Debug.Log(transform.ToString() + " : dead");
    //    }
    //}

    //#endregion
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
        objectState = objState;

        OnStateChanged();
    }

    virtual protected void OnStateChanged()
    {

    }

    virtual public void DeActivate()
    {
        gameObject.SetActive(false);

    }

    virtual public void Activate()
    {
        gameObject.SetActive(true);

    }

    virtual public void AddVelocity(Vector2 vel, ForceMode2D forceMode2D)
    {

    }



}
