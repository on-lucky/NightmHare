﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float hareSpeed = 5f;
    [SerializeField]
    private float hareAcceleration = 0.1f;
    [SerializeField]
    private float airAcceleration = 0.04f;

    [SerializeField] private float sprintCooldown = 0;
    [SerializeField] private float maxSprintDuration = 3;
    [SerializeField] private float hareSprint = 0.05f;
    public float shadowSpeedRatio = 0f;
    [SerializeField] private Ability sprintUI;    
    public bool canSprint = true;

    [SerializeField] private GameObject lightTrap;
    [SerializeField] private float trapCooldown;
    [SerializeField] private Ability trapUI;
    private bool canSetTrap = true;

    private bool isClimbing = false;

    [SerializeField] private float currentSpeed = 0f;
    private Animator animator;
    private OrientationManager orientationM;
    private Jumper jumper;
    private Climber climber;
    private bool inputEnabled = true;
    private AnxietyManager anxietyManager;

    public bool IsClimbing { get => isClimbing; set => isClimbing = value; }

    private float zPos = 0;
    
    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
        orientationM = GetComponent<OrientationManager>();
        jumper = GetComponent<Jumper>();
        climber = GetComponent<Climber>();
        anxietyManager = GetComponent<AnxietyManager>();
        shadowSpeedRatio = (hareSpeed + hareSprint) / hareSpeed;
    }

    private void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, zPos);
    }

    // Update is called once per frame
    void FixedUpdate() {        
        if (inputEnabled)
        {
            if (anxietyManager != null)
            {
                if (Input.GetKey(KeyCode.Q) && canSprint)
                {
                    StartSprinting();
                }

                if (Input.GetKey(KeyCode.W) && canSetTrap)
                {
                    SetLightTrap();
                }
            }
            
            if (Input.GetKey(KeyCode.RightArrow))
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
            else if (Input.GetKey(KeyCode.LeftArrow))
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

            else if (!climber.WallToFront)
            {
                Break();
            }
            else
            {
                currentSpeed = 0;
                StopClimbing();
            }
        }        
        else if (!climber.WallToFront)
        {
            Break();
        }
        else
        {
            currentSpeed = 0;
            StopClimbing();
        }                
        UpdateAnimator();       
    }    

    private void LookFoward(bool isRight)
    {
        if (orientationM.LookTo(isRight))
        {
            currentSpeed = -currentSpeed;
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
        if (currentSpeed > hareSpeed)
        {
            currentSpeed = hareSpeed;
        }
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

    public void StartSprinting()
    {
        if (anxietyManager.CanSprint())
        {
            if (sprintUI)
            {
                sprintUI.Fill();
            }
            hareSpeed += hareSprint;
            ShadowController.instance.speedRatio /= shadowSpeedRatio;
            canSprint = false;
            animator.speed = 2;            
            StartCoroutine(StopSprinting());
        }
    } 
    public void StopSprinting_()
    {
        if (!canSprint)
        {
            ShadowController.instance.speedRatio *= shadowSpeedRatio;
            hareSpeed -= hareSprint;
            animator.speed = 1;            
        }        
    }

    IEnumerator StopSprinting()
    {
        yield return new WaitForSeconds(maxSprintDuration);
        StopSprinting_();    
        StartCoroutine(RefreshSprintCooldown());
    }

    public void ResetSprintCooldown()
    {
        if (sprintUI)
        {
            sprintUI.Empty();
        }
        canSprint = true;
    }

    IEnumerator RefreshSprintCooldown()
    {
        if (!canSprint)
        {
            if (sprintUI)
            {
                sprintUI.StartCooldown(sprintCooldown);
            }
            yield return new WaitForSeconds(sprintCooldown);
            canSprint = true;
        }        
    }

    public void SetLightTrap()
    {       
        if (canSetTrap && anxietyManager.CanSetTrap() && lightTrap != null) {
            if (trapUI)
            {
                trapUI.Fill();
            }
            float x = this.transform.position.x;
            float y = this.transform.position.y + 0.1f;
            float z = this.transform.position.z;
            Instantiate(lightTrap, new Vector3(x, y, z), Quaternion.identity);
            lightTrap.tag = "trap";
            canSetTrap = false;
            StartCoroutine(RefreshTrapCooldown());
        }
    }

    public void ResetTrapCooldown()
    {
        if (trapUI)
        {
            trapUI.Empty();
        }
        canSetTrap = true;
    }

    IEnumerator RefreshTrapCooldown()
    {
        if (trapUI)
        {
            trapUI.StartCooldown(trapCooldown);
        }
        yield return new WaitForSeconds(trapCooldown);
        canSetTrap = true;
    }

    public void StopClimbing(){
        IsClimbing = false;
        GetComponent<Rigidbody>().isKinematic = false;
        animator.SetBool("StopClimbing", true);
    }

    private void UpdateAnimator()
    {
        float ratio = Mathf.Abs(currentSpeed) / hareSpeed;        
        animator.SetFloat("Speed", ratio);
    }

    public float GetCurrentSpeed(){
        return currentSpeed;
    }

    public void EnableInput(bool shouldEnable)
    {
        inputEnabled = shouldEnable;
    }

    public void SetCurrentSpeed(float _currentSpeed)
    {
        currentSpeed =  _currentSpeed;
    }

    public void SetZPos(float z)
    {
        this.zPos = z;
    } 

}
