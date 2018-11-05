using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour {

    private GameObject objToFollow;

    private bool isFollowing = false;

    private Animator animator;
    private OrientationManager orientationManager;

    private float currentTime = 0f;
    public float speedRatio = 1;
    private float timeBetweenReading = -1f;

    public void setObjToFollow(GameObject value)
    {
        objToFollow = value;
        timeBetweenReading = objToFollow.GetComponent<StateRecorder>().TimeBetweenRecording;
    }

    // Use this for initialization
    void Start () {
        animator = GetComponentInChildren<Animator>();
        orientationManager = GetComponentInChildren<OrientationManager>();
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
        transform.position = moment.position;
        orientationManager.LookTo(moment.lookingRight);
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
    }

    private void EnableShadowVisuals() {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
