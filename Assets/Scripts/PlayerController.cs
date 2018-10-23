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

    private float currentSpeed = 0f;
    private Animator animator;
    private OrientationManager orientationM;
    private Jumper jumper;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        orientationM = GetComponent<OrientationManager>();
        jumper = GetComponent<Jumper>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Run(true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Run(false);
        }
        else
        {
            Break();
        }
        UpdateAnimator();
    }

    private void Run(bool isRight)
    {
        Accelerate(isRight);
        //Debug.Log(new Vector3(currentSpeed, 0, 0));
        transform.Translate(new Vector3(0, 0, currentSpeed));
    }

    private void Break()
    {
        if(currentSpeed > 0)
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
        if (!isRight)
        {
            if (orientationM.LookTo(false))
            {
                currentSpeed = -currentSpeed;
            }
        }
        else
        {
            if (orientationM.LookTo(true))
            {
                currentSpeed = -currentSpeed;
            }
        }
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
        if(currentSpeed > hareSpeed)
        {
            currentSpeed = hareSpeed;
        }
    }

    private void UpdateAnimator()
    {
        float ratio = Mathf.Abs(currentSpeed) / hareSpeed;
        animator.SetFloat("Speed", ratio);
    }
}
