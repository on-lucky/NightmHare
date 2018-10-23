using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRecorder : MonoBehaviour
{

    /*private bool isRewinding;
    private bool isRecording;

    private MomentCapture[] momentCaptures;
    private int index = -1;
    private int currentMaxIndex = -1;
    private const int MAX_INDEX = 500;

    private Rigidbody rb;
    private Animator animator;
    private OrientationManager orientationManager;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        orientationManager = GetComponentInChildren<OrientationManager>();

        momentCaptures = new MomentCapture[MAX_INDEX];
        TimeController.instance.AddRewindableElement(this);
    }

    private void FixedUpdate()
    {
        if (isRecording)
        {
            Record();
        }
    }

    private void Record()
    {
        momentCaptures[++index] = new MomentCapture(transform.position, animator.GetCurrentAnimatorStateInfo(0), animator.GetFloat("Speed"), orientationManager.isRight);
        currentMaxIndex = index;
    }

    public void RewindTo(float ratio)
    {
        index = (int)Mathf.Round(ratio * (float)currentMaxIndex);
        transform.position = momentCaptures[index].position;

        SetStickmanState(momentCaptures[index]);
    }

    private void SetStickmanState(MomentCapture moment)
    {
        orientationManager.LookTo(moment.lookingRight);
        animator.SetFloat("Speed", moment.speed);
        animator.Play(moment.animationState.fullPathHash, 0, moment.animationState.normalizedTime);
    }

    public void StartRecording()
    {
        isRecording = true;
    }

    public void StopRecording()
    {
        isRecording = false;
    }*/
}
