using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {

    [SerializeField]
    private Transform subject;

    [SerializeField]
    private float xRange = 1;

    [SerializeField]
    private float yRange = 1;

    [SerializeField]
    private float yOffset = 2;

    // Update is called once per frame
    void Update () {
		if(subject.position.x > transform.position.x + xRange)
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
}
