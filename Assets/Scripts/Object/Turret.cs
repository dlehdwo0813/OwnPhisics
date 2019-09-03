using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementCalculator))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(ObjectPool))]
public class Turret : MonoBehaviour
{

    [SerializeField]
    Launcher launcher;
    [SerializeField]
    ObjectPool ammo;

    [SerializeField]
    MovementCalculator movementCalculator;
    CircleCollider2D detectArea;

    [SerializeField]
    bool isReloaded = false;
    public float bulletLifeTime = 1;

    public enum TurretState
    {

        Default = 0, Wait = 1, Reloading = 2, Fire = 4

    }
    public int state;

    // Use this for initialization
    void Start()
    {
        state = 1;
        launcher = GetComponentInChildren<Launcher>();
        ammo = GetComponent<ObjectPool>();
        movementCalculator = GetComponent<MovementCalculator>();

        detectArea = GetComponent<CircleCollider2D>();
        detectArea.isTrigger = true;

        ObstacleBasic ob = ammo.objectToPool.GetComponent<ObstacleBasic>();
        ob.lifeTime = bulletLifeTime;

    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    CharacterController2D cha = collision.GetComponent<CharacterController2D>();
    //    if (cha)
    //    {
    //        Debug.Log("CharacterController2D on");
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    CharacterController2D cha = collision.GetComponent<CharacterController2D>();
    //    if (cha)
    //    {
    //        Debug.Log("CharacterController2D off");
    //    }

    //}

    bool fire = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        CharacterController2D cha = collision.GetComponent<CharacterController2D>();
        if (cha)
        {
            Vector3 pos = cha.transform.position;
            Vector3 turretPos = transform.position;
            Vector2 fixedPos = new Vector2(pos.x - turretPos.x, pos.y - turretPos.y);
            float rad = Mathf.Atan2(fixedPos.x, fixedPos.y);
            float rot = (rad * 180) / Mathf.PI;

            transform.localEulerAngles = new Vector3(0, 0, -(rot + 90));
            Debug.Log("CharacterController2D stay");

            fire = true;
        }

    }

    private void FixedUpdate()
    {

        if((state & (int)TurretState.Reloading) > 0)
        {
            if (!isReloaded)
            {

                Vector2 vel = movementCalculator.CalculatePlatformMovement(transform, launcher.transform);
                launcher.reloadedObject.transform.position = new Vector3(vel.x,vel.y);
            }
            if (movementCalculator.Get_b_isIndexChanged())
            {
                Reloaded();
            }

        }

        if ((state & (int)TurretState.Wait) > 0)
        {
            if (!isReloaded)
            {
                ReloadBullet();
            }
            else
            {
                launcher.Wait();
            }
        }

        if ((state & (int)TurretState.Fire) > 0 && fire)
        {
            if (isReloaded)
            {
                Fire();
                fire = false;
            }
            state -= (int)TurretState.Fire;
        }

    }

    void ReloadBullet()
    {
        if (launcher && ammo && (state & (int)TurretState.Reloading) == 0)
        {
            ObjectBasic obj = ammo.GetInActivatedObject();
            obj.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + .1f);

            if (obj.objectState != ObjectState.FixedKinematic)
            {
                obj.StateChange(ObjectState.FixedKinematic);
            }

            obj.gameObject.SetActive(true);
            launcher.reloadedObject = obj;
            if ((state & (int)TurretState.Reloading) == 0)
                state += (int)TurretState.Reloading;
        }
    }

    void Reloaded()
    {
        isReloaded = true;
        state -= (int)TurretState.Reloading;
    }
    void Fire()
    {
        launcher.Launch(transform.position);
        isReloaded = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fire)
        {
            if ((state & (int)TurretState.Fire) == 0 && isReloaded)
                state += (int)TurretState.Fire;
        }
    }


}
