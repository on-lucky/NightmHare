using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSlider : MonoBehaviour {

    public float slidingDuration = 2;

    private Vector3 destination;
    private Vector3 origin;
    private bool sliding = false;
    private float a;
    private float currentTime = 0;

    // Use this for initialization
    void Start () {
        a = 2 * slidingDuration / Mathf.PI;
    }
	
	// Update is called once per frame
	void Update () {
        if (sliding)
        {
            currentTime += Time.deltaTime;
            UpdateSlide(currentTime);

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

    public void Slide()
    {
        currentTime = 0;
        sliding = true;
        GetComponent<CameraFollower>().EnableFollowing(false);
    }

    private void UpdateSlide(float time)
    {
        float ratio = Mathf.Sin(time / a);
        transform.position = Vector3.Lerp(origin, destination, ratio);
    }
}
