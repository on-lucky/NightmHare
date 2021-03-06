﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour {

    public static ShadowController instance;

    private GameObject objToFollow;

    private bool isFollowing = false;

    private Animator animator;
    private OrientationManager orientationManager;

    private float currentTime = 0f;
    public float speedRatio = 1;
    private float timeBetweenReading = -1f;
    private GameObject[] traps;
    [SerializeField] private int trapDuration;
    [SerializeField] private float slowRatio;
    [SerializeField] private ParticleSystem deathParticles;

    void Awake()
    {
        if (ShadowController.instance == null)
        {
            ShadowController.instance = this;
        }
        else
        {
            Debug.LogError("More than one ShadowController in the scene!");
        }
       
    }

    public void setObjToFollow(GameObject value)
    {
        objToFollow = value;
        timeBetweenReading = objToFollow.GetComponent<StateRecorder>().TimeBetweenRecording;
    }

    // Use this for initialization
    void Start () {
        animator = GetComponentInChildren<Animator>();
        GetComponent<BoxCollider>().enabled = false;
        orientationManager = GetComponentInChildren<OrientationManager>();
        AudioManager.instance.PlaySound("Laugh", 0.1f, 0f, false);
    }
	
	// Update is called once per frame
	void Update () {
        if (isFollowing)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= timeBetweenReading / speedRatio)
            {
                SetState(objToFollow.GetComponent<StateRecorder>().DequeueMomentState());
                currentTime -= timeBetweenReading / speedRatio;
            }
        }           
	}

    private void SetState(MomentCapture moment)
    {
        transform.position = new Vector3(moment.position.x, moment.position.y, moment.position.z);
        orientationManager.LookTo(moment.lookingRight);
        animator.SetFloat("Speed", moment.speed);
        animator.Play(moment.animationState.fullPathHash, 0, moment.animationState.normalizedTime);
    }

    public void StartFollowing(float timeToWait)
    {
        StartCoroutine(waitAndFollow(timeToWait));
        
    }

    public void StopFollowing()
    {
        isFollowing = false;
    }

    IEnumerator waitAndFollow(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        EnableShadowVisuals();
        isFollowing = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    public void EnableShadowVisuals() {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    public void TrapShadow()
    {
        animator.speed *= slowRatio;
        speedRatio = speedRatio*slowRatio;       
        StartCoroutine(FreeShadow());
    }

    IEnumerator FreeShadow()
    {
        yield return new WaitForSeconds(trapDuration);
        animator.speed = 1;
        speedRatio = speedRatio/slowRatio;       
    }

    public void Die()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        StopFollowing();
        objToFollow.GetComponent<StateRecorder>().ResetMomentStates();

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        if (deathParticles)
        {
            deathParticles = Instantiate(deathParticles, this.transform.position, this.transform.rotation);
            deathParticles.Play();
        }
        ShadowController.instance = null;
        Destroy(gameObject);
    }
}
