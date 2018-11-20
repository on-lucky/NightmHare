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
    public float TimeBetweenRecording { get => timeBetweenRecording; set => TimeBetweenRecording = value; }

    [SerializeField]
    private float timeBetweenRecording = 0.02f;

    private float currentTime = 0f;


    // Use this for initialization
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        orientationManager = GetComponentInChildren<OrientationManager>();

        MomentCaptures = new Queue<MomentCapture>();
    }

    void Update()
    {
        if (isRecording)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeBetweenRecording)
            {
                Record();
                currentTime -= timeBetweenRecording;
            }
        }

       
    }

    private void Record()
    {
        MomentCapture capture = new MomentCapture(transform.position, animator.GetCurrentAnimatorStateInfo(0), orientationManager.IsLookingRight, animator.GetFloat("Speed"));
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

    public float GetDelay()
    {
        return timeBetweenRecording * MomentCaptures.Count;
    }
}
