using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float hareSpeed = 5f;
    [SerializeField]
    private float hareAcceleration = 0.1f;
    [SerializeField]
    private float airAcceleration = 0.04f;

    private bool isClimbing = false;

    private float currentSpeed = 0f;
    private Animator animator;
    private OrientationManager orientationM;
    private Jumper jumper;
    private Climber climber;
    private Dashing dashing; 

    public bool IsClimbing { get => isClimbing; set => isClimbing = value; }
    
    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        orientationM = GetComponent<OrientationManager>();
        jumper = GetComponent<Jumper>();
        climber = GetComponent<Climber>();
        dashing = GetComponent<Dashing>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetKey(KeyCode.RightArrow) && !dashing.IsDashing)
        {
            LookFoward(true);
            if (IsClimbing && orientationM.IsLookingRight)
            {
                currentSpeed = 0f;
                climber.Climb();
            }
            else if (!climber.WallToFront)
            {
                Run(true);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !dashing.IsDashing)
        {
            LookFoward(false);
            if (IsClimbing && !orientationM.IsLookingRight)
            {
                currentSpeed = 0f;
                climber.Climb();
            }
            else if (!climber.WallToFront)
            {
                Run(false);
            }
        }
        else if (!climber.WallToFront && jumper.OnGround)
        {
            Break();
        }
        else
        {
            StopClimbing();
        }
        UpdateAnimator();
    }

    private void LookFoward(bool isRight)
    {
        if (orientationM.LookTo(isRight))
        {
            currentSpeed = -currentSpeed;
            climber.SwapWalls();
        }
    }

    private void Run(bool isRight)
    {
        StopClimbing();
        Accelerate(isRight);
        transform.Translate(new Vector3(0, 0, currentSpeed));
    }

    private void Break()
    {
        StopClimbing();
        if (currentSpeed > 0)
        {
            if (jumper.CheckGround())
            {
                currentSpeed -= hareAcceleration;
            }
            else
            {
                currentSpeed -= airAcceleration;
            }

            if (currentSpeed < 0)
            {
                currentSpeed = 0;
            }

            transform.Translate(new Vector3(0, 0, currentSpeed));
        }
        else if (currentSpeed < 0)
        {
            if (jumper.CheckGround())
            {
                currentSpeed += hareAcceleration;
            }
            else
            {
                currentSpeed += airAcceleration;
            }

            if (currentSpeed > 0)
            {
                currentSpeed = 0;
            }

            transform.Translate(new Vector3(0, 0, currentSpeed));
        }
    }

    private void Accelerate(bool isRight)
    {
        if (jumper.CheckGround())
        {
            currentSpeed += hareAcceleration;
        }
        else
        {
            currentSpeed += airAcceleration;
        }
        if (currentSpeed < 0)
        {
            if (jumper.CheckGround())
            {
                currentSpeed += hareAcceleration;
            }
            else
            {
                currentSpeed += airAcceleration;
            }
        }
        if (currentSpeed > hareSpeed)
        {
            currentSpeed = hareSpeed;
        }
    }

    public void StopClimbing(){
        IsClimbing = false;
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void UpdateAnimator()
    {
        float ratio = Mathf.Abs(currentSpeed) / hareSpeed;
        animator.SetFloat("Speed", ratio);
    }

    public float GetCurrentSpeed(){
        return currentSpeed;
    }
}
