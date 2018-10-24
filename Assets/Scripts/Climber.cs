using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climber : MonoBehaviour {

    public float speedConversionRatio = 0.6f;
    public float decelerationRate = 0.005f;

    private float currentUpwardSpeed = 0;
    private float collisionSpeed = 0;

    private bool wallToBack = false;
    private bool wallToFront = false;

    public bool WallToBack { get => wallToBack; set => wallToBack = value; }
    public bool WallToFront { get => wallToFront; set => wallToFront = value; }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void HitWall(Collider other, bool isFront)
    {
        GameObject wall = other.gameObject;

        if (wall.tag == "Terrain")
        {
            if (isFront)
            {
                wallToFront = true;
            }
            else
            {
                wallToBack = true;
            }
            GetComponent<PlayerController>().IsClimbing = true;
            GetComponent<Rigidbody>().isKinematic = true;
            collisionSpeed = GetComponent<PlayerController>().GetCurrentSpeed();
            currentUpwardSpeed = collisionSpeed * speedConversionRatio;
            Debug.Log("collisionSpeed: " + collisionSpeed);
        }
    }

    public void LeaveWall(Collider other, bool isFront)
    {
        GameObject wall = other.gameObject;

        if (wall.tag == "Terrain")
        {
            GetComponent<PlayerController>().StopClimbing();
            if (isFront)
            {
                wallToFront = false;
                if (!wallToBack)
                {
                    transform.Translate(new Vector3(0f, 0, 0.1f));
                }
            }
            else
            {
                wallToBack = false;
            }
        }
    }

    public void SwapWalls()
    {
        bool temp = wallToFront;
        wallToFront = wallToBack;
        wallToBack = temp;
    }

    private void RegisterCollisionSpeed(float speed)
    {
        collisionSpeed = speed;
    }

    public void Climb()
    {
        transform.Translate(new Vector3(0, currentUpwardSpeed, 0));
        currentUpwardSpeed -= decelerationRate;
        //Debug.
        if(currentUpwardSpeed <= 0)
        {
            GetComponent<PlayerController>().IsClimbing = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
