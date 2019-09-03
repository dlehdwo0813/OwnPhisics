using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EnemyAI : MonoBehaviour {

    Enemy enemy;
    CircleCollider2D view;

    [SerializeField]
    Transform curTarget;
    Vector3 lastTarget;
    Vector2 direction;

    public List<Transform> targets;
    int targetcnt = 0;

    public enum CharacterState
    {
        Wait,
        Patrol,
        Chase,
    }

    public CharacterState state;

    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        if (!enemy)
        {
            Debug.Log(transform.ToString() + " : enemy is empty");
        }
        view = GetComponent<CircleCollider2D>();

        state = CharacterState.Wait;
        //action = CharacterAction.Search;
        StartCoroutine(FSM());
    }

    IEnumerator FSM()
    {
        while (true)
            yield return StartCoroutine(state.ToString());
    }

    public float waitTime = 2;
    bool isFoundSomething = false;
    IEnumerator Wait()
    {
        //print("Entering the mine...");
        //yield return null;

        bool queing = true;
        float t = Time.time;
        while (queing)
        {
            if (Time.time - t > waitTime)
            {
                state = CharacterState.Patrol;
                queing = false;
                curTarget = targets[targetcnt % targets.Count];
                targetcnt++;
                break;
            }

            if (isFoundSomething)
            {
                state = CharacterState.Chase;
                queing = false;
                break;
            }

            yield return new WaitForSeconds(.5f);
        }

    }

    IEnumerator Patrol()
    {
        bool isReached = false;
        float t = Time.time;

        while (!isReached)
        {
            EnemyMovement();

            if (!(moveX||moveY))
            {
                state = CharacterState.Wait;
                isReached = true;
                break;
            }

            if (isFoundSomething)
            {
                state = CharacterState.Chase;
                isReached = true;
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator Chase()
    {
        bool isReached = false;
        float t = Time.time;

        while (!isReached)
        {
            EnemyMovement();

            if (!isFoundSomething)
            {
                state = CharacterState.Wait;
                isReached = true;
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    void EnemyMovement()
    {
        if (!enemy)
        {
            Debug.Log("EnemyAI : enemey is empty");
            return;
        }

        if (curTarget)
        {
            moveX = true;
            moveY = true;

            HorizontalMove();
            VerticalMove();
        }
    }

    bool moveX = true;
    bool moveY = true;
    void HorizontalMove()
    {
        if (curTarget.position != lastTarget)
        {
            lastTarget = curTarget.position;
            direction = lastTarget - enemy.transform.position;
            direction.Normalize();
        }

        if (direction.x != 0)
        {
            enemy.SetDirectionalInput(direction);
        }

        if ((int)curTarget.position.x == (int)enemy.transform.position.x)
        {
            direction.x = 0;
            enemy.SetDirectionalInput(direction);
            moveX = false;
        }
    }

    void VerticalMove()
    {
            if (enemy.controller.collisions.left || enemy.controller.collisions.right)
            {
                if (!enemy.controller.collisions.climbingSlope)
                {
                    enemy.OnJumpInputDown();
                    Invoke("NormalJump", .5f);
                }
            }

        if ((int)curTarget.position.x == (int)enemy.transform.position.x)
        {
            direction.x = 0;
            enemy.SetDirectionalInput(direction);
            moveY = false;
        }


    }

    void NormalJump()
    {
        enemy.OnJumpInputUp();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            curTarget = player.transform;
            isFoundSomething = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player)
        {
            curTarget = null;
            isFoundSomething = false;
        }
    }

}
