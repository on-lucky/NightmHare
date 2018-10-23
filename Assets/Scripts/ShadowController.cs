using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour {

    [SerializeField]
    private GameObject objToFollow;

    private bool isFollowing = false;

    private Animator animator;
    private OrientationManager orientationManager;


    // Use this for initialization
    void Start () {
        animator = GetComponentInChildren<Animator>();
        orientationManager = GetComponentInChildren<OrientationManager>();

        StartFollowing(1f);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (isFollowing)
        {
            SetState(objToFollow.GetComponent<StateRecorder>().DequeueMomentState());
        }
	}

    private void SetState(MomentCapture moment)
    {
        transform.position = moment.position;
        orientationManager.LookTo(moment.lookingRight);
        animator.SetFloat("Speed", moment.speed);
        animator.SetBool("Jumping", moment.jumping);
        animator.Play(moment.animationState.fullPathHash, 0, moment.animationState.normalizedTime);
    }

    public void StartFollowing(float timeToWait)
    {
        StartCoroutine(waitAndFollow(timeToWait));
    }

    IEnumerator waitAndFollow(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        isFollowing = true;
    }
}
