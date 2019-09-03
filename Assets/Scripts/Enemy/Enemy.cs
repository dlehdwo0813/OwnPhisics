using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterController2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Enemy : MonoBehaviour {
    public bool standingOnSomething = false;

    public bool ropeHold;
    public bool SB_Down;


    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    public float moveSpeed = 6;
    public float maxGravity = -10;

    public Vector2 wallJumpClimb = new Vector2(4, 20);
    public Vector2 wallJumpOff = new Vector2(1, 5);
    public Vector2 wallLeap = new Vector2(15, 20);

    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    float gravity;
    public float gravityScale = 1.0f;
    float maxJumpvelocity;
    float velocityXSmoothing;

    //[HideInInspector]
    public GameObject InteractiveGameObject;

    public CharacterController2D controller;
    public Rigidbody2D rb2d;
    public BoxCollider2D boxCollider2D;

    public PlayerImage playerImage;

    //Animator animator;

    public Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;

    bool playerHurt;
    float playerHurtTimeMax = 0.3f;
    float playerHurtTimer = 0;

    void Start()
    {
        controller = GetComponent<CharacterController2D>();

        rb2d = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb2d.gravityScale = 0;

        playerImage = GetComponentInChildren<PlayerImage>();
        //animator = GetComponent<Animator>();


        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpvelocity = Mathf.Abs(gravity) * timeToJumpApex;

        //chracterState.Reset();
    }

    public Transform targetTransform;
    public Vector3 targetPosition;
    public bool isTargetMove;

    public float percentBetweenWaypoints = 0;

    float Ease(float x, float easeAmount)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    public void PlayerHurt()
    {
        playerHurt = true;
        //animator.SetBool("hurt", playerHurt);
    }


    private void FixedUpdate()
    {

        if (playerHurt)
        {
            playerHurtTimer += Time.deltaTime;
            Debug.Log(playerHurtTimer.ToString());
            if (playerHurtTimeMax < playerHurtTimer)
            {
                playerHurtTimer = 0;
                playerHurt = false;
                //animator.SetBool("hurt", playerHurt);

            }
        }

        if (ropeHold && InteractiveGameObject)
        {
            float distance = InteractiveGameObject.transform.position.x - transform.position.x;
            if (Mathf.Abs(distance) > boxCollider2D.size.x / 2)
            {
                ropeHold = false;
                InteractiveGameObject = null;
            }
        }

        Calculatevelocity();
        HandleWallSliding();

        if (targetTransform)
        {
            targetPosition = targetTransform.position;
        }
        if (targetPosition.magnitude != 0)
        {

            Vector2 dir = targetPosition - transform.position;
            Vector3 newPos = dir.normalized * moveSpeed * Time.deltaTime;

            Vector2 mt = Vector2.zero;
            controller.Move(gravityScale * newPos, mt);
        }
        else
        {
            controller.Move(gravityScale * controller.velocity * Time.deltaTime, directionalInput);
        }

        if (controller.collisions.above || controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                controller.velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                controller.velocity.y = 0;
            }
        }

        SpriteAnimation();

    }

    void SpriteAnimation()
    {
        //animator.SetFloat("controller.velocityX", Mathf.Abs(directionalInput.x) / moveSpeed);
        //animator.SetBool("grounded", controller.collisions.below);
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;

        if (directionalInput.x > 0.01f)
        {
            if (playerImage.spriteRenderer.flipX == true)
            {
                playerImage.spriteRenderer.flipX = false;
            }
        }
        else if (directionalInput.x < -0.01f)
        {
            if (playerImage.spriteRenderer.flipX == false)
            {
                playerImage.spriteRenderer.flipX = true;
            }
        }
    }

    public void OnJumpInputDown()
    {
        SB_Down = true;
        standingOnSomething = false;

        transform.localRotation.Set(0, 0, 0, 0);

        if (wallSliding)
        {
            if (wallDirX == Mathf.Sign(directionalInput.x))
            {
                controller.velocity.x = -wallDirX * wallJumpClimb.x;
                controller.velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                controller.velocity.x = -wallDirX * wallJumpOff.x;
                controller.velocity.y = wallJumpOff.y;
            }
            else
            {
                controller.velocity.x = -wallDirX * wallLeap.x;
                controller.velocity.y = wallLeap.y;
            }
        }

        if (ropeHold)
        {
            Debug.Log("rope off");
            if (directionalInput.x != 0)
            {
                controller.velocity.x = directionalInput.x * wallJumpClimb.x;
                controller.velocity.y = wallJumpClimb.y;
            }
            else
            {
                controller.velocity.x = 0;
                controller.velocity.y = wallJumpOff.y * 1.5f;
            }
            ropeHold = false;
            targetPosition.Set(0, 0, 0);
            isTargetMove = false;

            //controller.collisions.rope = false;
            //transform.rotation.Set(-transform.parent.rotation.x, -transform.parent.rotation.y, 
            //    -transform.parent.rotation.z, -transform.parent.rotation.w);
            //transform.parent = null;
        }

        if (controller.collisions.below)
        {
            if (controller.collisions.slidingDownMaxSlope)
            {
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x))
                { // not jumping against max slope
                    controller.velocity.y = maxJumpvelocity * controller.collisions.slopeNormal.y;
                    controller.velocity.x = maxJumpvelocity * controller.collisions.slopeNormal.x;
                }
            }
            else
            {
                controller.velocity.y = maxJumpvelocity;
            }
        }
    }

    public void OnJumpInputUp()
    {
        SB_Down = false;
        if (!ropeHold)
        {
            InteractiveGameObject = null;
        }

        if (controller.velocity.y > 0)
        {
            controller.velocity.y = controller.velocity.y * 0.5f;
        }
    }


    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;

        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && controller.velocity.y < 0)
        {
            wallSliding = true;

            if (controller.velocity.y < -wallSlideSpeedMax)
            {
                controller.velocity.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                controller.velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }

        }

    }

    void Calculatevelocity()
    {
        float targetvelocityX = directionalInput.x * moveSpeed;
        controller.velocity.x = Mathf.SmoothDamp(controller.velocity.x, targetvelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        if (!ropeHold)
        {
            if (!standingOnSomething)
                controller.velocity.y += gravity * Time.deltaTime;
            else
            {
                controller.velocity.y = 0;
            }
            //if(controller.velocity.y < maxGravity)
            //{
            //    controller.velocity.y = maxGravity;
            //}
        }
        else
        {
            controller.velocity.x = 0;
            controller.velocity.y = 0;
        }


    }

    public void Impulse(Vector2 impulsevelocity)
    {
        controller.velocity.x = impulsevelocity.x;
        controller.velocity.y = impulsevelocity.y;
    }

    //return Vector
    public Vector3 Getvelocity()
    {
        return controller.velocity;
    }

    public Vector3 GetCenterWorld()
    {
        Vector3 center = transform.position;
        center.y += boxCollider2D.size.y / 2;

        return center;
    }
    public Vector3 GetCenterLocal()
    {
        Vector3 center = new Vector3();
        center.y = boxCollider2D.size.y / 2;

        return center;
    }
    public Vector3 GetHand()
    {
        Vector3 hand = transform.position;
        hand.y += boxCollider2D.size.y * .66f;
        int dir = (int)Mathf.Sign(directionalInput.x);
        hand.x += boxCollider2D.size.x * .25f * dir;

        return hand;
    }
}

