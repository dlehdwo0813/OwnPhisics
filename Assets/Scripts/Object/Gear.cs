using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : ObstacleBasic , IDamageAble
{

    #region IDamageAble

    public int iMaxHP = 10;
    public int iCurrentHP = 10;
    public bool isReadyToAttack_i = false;

    public void Attack(IDamageAble ida)
    {
        //이펙트
        //사운드
        ida.GetDamage(iMaxHP);
    }
    public void GetDamage(int damage)
    {
        //iDamage.iCurrentHP -= damage;
    }

    #endregion

    // Use this for initialization
    new void Start()
    {
        base.Start();
        iCurrentHP = iMaxHP;
        
    }

    private void FixedUpdate()
    {
        OnStateChanged();

        if (objectTag.isObjectTagIncluded(ObjectTag.Projectile))
        {
            lifeTimeTick += Time.deltaTime;
            if (lifeTimeTick > lifeTime)
            {
                lifeTimeTick = 0;
                objectTag -= (int)ObjectTag.Projectile;
                gameObject.SetActive(false);
            }
        }
        else
        {
            rb2d.gravityScale = 1;
        }
        if (objectState == ObjectState.Free)
        {
            if(rb2d.velocity.magnitude > .7f)
            {
                isReadyToAttack_i = true;
            }
            else
            {
                isReadyToAttack_i = false;
            }
        }

    }

    override protected void OnStateChanged()
    {
        if (!rb2d)
            return;
        if (objectState == lastObjState)
            return;

        switch (objectState)
        {
            case ObjectState.Free:
                {
                    rb2d.bodyType = RigidbodyType2D.Dynamic;
                    rb2d.gravityScale = 1;
                        }
                break;
            case ObjectState.FixedKinematic:
                {
                    rb2d.bodyType = RigidbodyType2D.Kinematic;
                    rb2d.gravityScale = 0;
                    isReadyToAttack_i = false;
                }
                break;
            case ObjectState.FixedDynamic:
                {
                    rb2d.bodyType = RigidbodyType2D.Dynamic;
                    rb2d.gravityScale = 1;
                    isReadyToAttack_i = true;
                }
                break;
            case ObjectState.Projectile:
                {
                    rb2d.bodyType = RigidbodyType2D.Dynamic;
                    rb2d.gravityScale = 0;
                    isReadyToAttack_i = true;
                }
                break;
            case ObjectState.Air:
                {
                    rb2d.bodyType = RigidbodyType2D.Dynamic;
                    rb2d.gravityScale = 1;
                    isReadyToAttack_i = true;
                }
                break;
        }
        lastObjState = objectState;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (objectState == ObjectState.Air)
        {
            if (objectTag.isObjectTagIncluded(ObjectTag.Projectile))
                StateChange(ObjectState.Free);
        }

        if (objectState == ObjectState.Projectile)
        {
            if (objectTag.isObjectTagIncluded(ObjectTag.Projectile))
                StateChange(ObjectState.Air);
        }


        ObjectBasic obj = collision.transform.GetComponent<ObjectBasic>();
        if (obj)
        {
            if (isReadyToAttack_i)
            {
                if (obj.objectTag.isObjectTagIncluded(ObjectTag.Damage))
                {
                    IDamageAble ida = collision.transform.GetComponent<IDamageAble>();
                    if (ida != null)
                    {
                        Attack(ida);
                    }
                }
            }

        }


    }

    public override void AddVelocity(Vector2 vel,ForceMode2D forceMode2D)
    {
        rb2d.AddForce(vel, forceMode2D);
    }
}
