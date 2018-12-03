using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    public static CameraFollower instance;

    [SerializeField]
    private Transform subject;

    [SerializeField]
    private float xRange = 1;

    [SerializeField]
    private float yRange = 1;

    [SerializeField]
    private float yOffset = 2;

    [SerializeField]
    private float zMidPlane = 10;

    [SerializeField]
    private float zFrontPlane = -10;

    [SerializeField]
    private float zBackPlane = -5;

    private bool following = true;

    void Awake()
    {
        if(CameraFollower.instance != null)
        {
            Debug.LogError("More than one CameraFollower in the scene!");
        }
        else
        {
            CameraFollower.instance = this;
        }
    }

    // Update is called once per frame
    void Update () {
        if (following)
        {
            if (subject.position.x > transform.position.x + xRange)
            {
                UpdatePosX(false);
            }
            else if (subject.position.x < transform.position.x - xRange)
            {
                UpdatePosX(true);
            }
            if (subject.position.y > transform.position.y + yRange - yOffset)
            {
                UpdatePosY(false);
            }
            else if (subject.position.y < transform.position.y - yRange - yOffset)
            {
                UpdatePosY(true);
            }
            //UpdatePosZ();
        }
    }

    private void UpdatePosX(bool higher)
    {
        float posX = subject.position.x - xRange;
        if (higher)
        {
            posX = subject.position.x + xRange;
        }
        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }

    private void UpdatePosY(bool higher)
    {
        float posY = subject.position.y - yRange + yOffset;
        if (higher)
        {
            posY = subject.position.y + yRange + yOffset;
        }
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
    }

    private void UpdatePosZ()
    {
        if (subject.position.z < zMidPlane)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zFrontPlane);
        } else {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBackPlane);
        }
    }

    public void SlideTo(Vector3 destination)
    {
        GetComponent<CameraSlider>().SetDestination(destination);
        GetComponent<CameraSlider>().Slide();
    }

    public void EnableFollowing(bool shouldFollow)
    {
        following = shouldFollow;
    }

    public void SetYOffset(float offset)
    {
        yOffset = offset;
    }

    public void SetZPos(float z)
    {
        Vector3 camPos = transform.position;
        camPos.z = z;
        transform.position = camPos;
    }

    public float GetYOffset()
    {
        return yOffset;
    }
}
