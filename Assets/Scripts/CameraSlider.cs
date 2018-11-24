using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSlider : MonoBehaviour {

    public float slidingDuration = 2;
    private float rotSpeed = 1;

    private Vector3 destination;
    private Vector3 origin;
    private Quaternion rotation;
    private Quaternion origRotation;
    private bool sliding = false;
    private float a;
    private float currentTime = 0;

    // Use this for initialization
    void Start () {
        a = 2 * slidingDuration / Mathf.PI;
        rotation = transform.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        if (sliding)
        {
            currentTime += Time.deltaTime;
            updateSlide(currentTime);

            if(currentTime >= slidingDuration)
            {
                sliding = false;
                GetComponent<CameraFollower>().EnableFollowing(true);
            }
        }
	}

    public void SetDestination(Vector3 _destination)
    {
        origin = transform.position;
        destination = _destination;
    }

    public void SetRotation(Vector3 eulers)
    {
        rotation = Quaternion.Euler(eulers);
        origRotation = transform.rotation;
        var angle = Quaternion.Angle(origRotation, rotation);
        rotSpeed = angle / slidingDuration;
    }

    public void Slide()
    {
        currentTime = 0;
        sliding = true;
        GetComponent<CameraFollower>().EnableFollowing(false);
    }

    private void updateSlide(float time)
    {
        float ratio = Mathf.Sin(time / a);
        transform.position = Vector3.Lerp(origin, destination, ratio);
        transform.rotation = Quaternion.RotateTowards(origRotation, rotation, Time.deltaTime*rotSpeed);
    }
}
