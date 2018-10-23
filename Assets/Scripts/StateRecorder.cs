using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRecorder : MonoBehaviour
{
    private bool isRecording;

    private Queue<MomentCapture> momentCaptures;

    private Animator animator;
    private OrientationManager orientationManager;

    public Queue<MomentCapture> MomentCaptures { get => momentCaptures; set => momentCaptures = value; }


    // Use this for initialization
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        orientationManager = GetComponentInChildren<OrientationManager>();

        MomentCaptures = new Queue<MomentCapture>();
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
        MomentCapture capture = new MomentCapture(transform.position, animator.GetCurrentAnimatorStateInfo(0), animator.GetFloat("Speed"), animator.GetBool("Jumping"), orientationManager.IsLookingRight);
        MomentCaptures.Enqueue(capture);
    }

    public void StartRecording()
    {
        isRecording = true;
    }

    public void StopRecording()
    {
        isRecording = false;
    }

    public void ResetMomentStates()
    {
        momentCaptures.Clear();
    }

    public MomentCapture DequeueMomentState()
    {
        return momentCaptures.Dequeue();
    }
}
