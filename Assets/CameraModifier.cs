using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraModifier : MonoBehaviour {

    // Indicates if forward is to the right
    [SerializeField] private bool forward = true;

    // Transformations to do on the camera
    [SerializeField] private Vector3 translation = Vector3.zero;
    [SerializeField] private Vector3 rotation = Vector3.zero;

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            bool isRight = other.transform.position.x > transform.position.x;
            if (forward == isRight)
            {
                Forward();
            } else {
                Backward();
            }
        }
    }

    void Forward()
    {
        if (translation.magnitude >= 0.01)
        {
            var destination = Camera.main.transform.position + translation;
            CameraFollower.instance.SlideTo(destination);
        }
        if (rotation.magnitude >= 0.01)
        {

        }
    }

    void Backward()
    {

    }
}
