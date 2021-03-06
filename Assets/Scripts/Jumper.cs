﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class Jump
{
    [Range(0, 20f)]
    public float jumpForce = 400f; // upward force of the jump
}

public class Jumper : MonoBehaviour {

    [SerializeField]
    private float JumpThrust = 5f;
    [SerializeField]
    private float WallJumpThrust = 300f;

    [SerializeField]
    private BoxCollider groundCheck;
    [SerializeField]
    private int MaxJumpInputCount = 10;

    /* SAUTS CONSECUTIFS */
    [SerializeField] Jump[] airJumps;                   // the number of consecutive air jumps that the character can make    
    private int airJumpCount = 0;

    private Animator animator;
    private Rigidbody rb;
    private Dashing dashing; 
    private Climber climber;
    private OrientationManager orientationManager;
    private PlayerController playerController;
    private bool onGround = false;
    private bool isJumping = false;
    private int jumpInputCount = 0;
    private bool canJump = true;

    public bool OnGround { get => onGround; set => onGround = value; }
    public bool IsJumping { get => isJumping; set => isJumping = value; }

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        dashing = GetComponent<Dashing>();
        climber = GetComponent<Climber>();
        orientationManager = GetComponent<OrientationManager>();
        playerController = GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && !dashing.IsDashing && canJump)
        {
            // Normal Jump
            if (onGround)
            {
                
                jumpInputCount = 0;
                IsJumping = true;
                rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                Jump(JumpThrust);
                animator.SetTrigger("Jumping");
            }
            // Wall Jump
            else if (climber.WallNearby) 
            {
                
                if (climber.WallToFront)
                {
                    // Flip character
                    orientationManager.LookTo(!orientationManager.IsLookingRight);
                    //climber.SwapWalls();
                }
                GetComponent<Rigidbody>().isKinematic = false;
                StartCoroutine(WaitForWallJump());
                WallJump(WallJumpThrust);
                animator.SetBool("AirBorn", true);
                animator.SetTrigger("Jumping");
            }
            // Air Jump
            else if (airJumpCount < airJumps.Length)
            {
                jumpInputCount = 0;
                IsJumping = true;
                rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                Jump(airJumps[airJumpCount].jumpForce);
                airJumpCount++;
                animator.SetTrigger("Jumping");
            }
        }
        
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space) && IsJumping && jumpInputCount < MaxJumpInputCount)
        {
            Jump(JumpThrust);
        }
        else
        {
            IsJumping = false;
        }
    }

    private void Jump(float thrust)
    {
        float jumpMultiplier = 1;
        //Initial jump is higher
        if (jumpInputCount == 0)
        {
            jumpMultiplier *= 25f;
        }

        animator.SetBool("AirBorn", true);
        
        rb.AddForce(new Vector3(0, jumpMultiplier, 0) * thrust);
        onGround = false;
        
        jumpInputCount++;
    }

    private void WallJump(float thrust) 
    {
        rb.velocity = new Vector3(0, 0, 0);
        rb.AddRelativeForce(new Vector3(0, 1f, 1f) * thrust);
        onGround = false;
        playerController.SetCurrentSpeed(0);

        jumpInputCount++;
        airJumpCount = 0;
    }

    public void Land(Collider other)
    {
        GameObject terrain = other.gameObject;
        GetComponent<Rigidbody>().isKinematic = false;

        if (terrain.tag == "Ground")
        {
            animator.SetBool("AirBorn", false);
            onGround = true;
        }

        airJumpCount = 0;
    }

    public void Fall()
    {
        animator.SetBool("AirBorn", true);
        onGround = false;
    }

    public bool CheckGround()
    {
        return onGround;
    }

    public void resetAirJumpCount()
    {
        airJumpCount = 0;
    }

    public void DisableJump(float duration)
    {
        StartCoroutine(WaitForJump(duration));
    }

    IEnumerator WaitForJump(float duration)
    {
        canJump = false;
        yield return new WaitForSeconds(duration);
        canJump = true;
    }

    IEnumerator WaitForWallJump()
    {
        playerController.EnableInput(false);
        yield return new WaitForSeconds(0.2f);
        playerController.EnableInput(true);
        playerController.SetCurrentSpeed(0.14f);
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }
}
