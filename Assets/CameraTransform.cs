using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransform : MonoBehaviour {

    // How long to do the transformations
    [SerializeField] private float duration = 1;

    // Indicates if forward is to the right
    [SerializeField] private bool isForward = true;

    // Transformations to do on the camera
    [SerializeField] private float offsetY = 0;
    [SerializeField] private float z = 0;
    [SerializeField] private float rotation = 0;

    private bool go = false;
    private float ySpeed = 1;
    private float zSpeed = 1;
    private float rotSpeed = 1;
    private float currentTime = 0;
    private float forward = 1;

    // Original values
    private float originOffsetY = 0;
    private float originZ = 0;
    private float originRotation = 0;

    // Destination
    private float finalOffsetY = 0;
    private float finalZ = 0;
    private float finalRotation = 0;

    private void Update()
    {
        if (go)
        {
            currentTime += Time.deltaTime;
            UpdateTranslation();
            UpdateRotation();

            if (currentTime >= duration)
            {
                go = false;

                // Clip final values to what they should be
                var camTransform = CameraFollower.instance.transform;
                CameraFollower.instance.SetYOffset(finalOffsetY);
                camTransform.position = new Vector3(camTransform.position.x, finalOffsetY, finalZ);
                camTransform.rotation = Quaternion.AngleAxis(finalRotation, Vector3.right);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Save current values
            originOffsetY = CameraFollower.instance.GetYOffset();
            originZ = Camera.main.transform.position.z;
            originRotation = Camera.main.transform.rotation.eulerAngles.x;

            // Calculate speeds
            var camPos = Camera.main.transform.position;
            var camRotation = Camera.main.transform.rotation.eulerAngles;
            ySpeed = (offsetY - camPos.y) / duration;
            zSpeed = (z - camPos.z) / duration;
            rotSpeed = (rotation - camRotation.x) / duration;

            // Set final values
            finalOffsetY = offsetY;
            finalZ = z;
            finalRotation = rotation;

            // Start moving
            go = true;
            currentTime = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            // Return to original values

            // Calculate speeds
            var camPos = Camera.main.transform.position;
            var camRotation = Camera.main.transform.rotation.eulerAngles;
            ySpeed = (originOffsetY - camPos.y) / duration;
            zSpeed = (originZ - camPos.z) / duration;
            rotSpeed = (originRotation - camRotation.x) / duration;

            // Set final values
            finalOffsetY = originOffsetY;
            finalZ = originZ;
            finalRotation = originRotation;

            // Start moving
            go = true;
            currentTime = 0;
        }
    }

    private void UpdateTranslation()
    {
        var deltaY = forward * ySpeed * Time.deltaTime;
        var deltaZ = forward * zSpeed * Time.deltaTime;
        var pos = CameraFollower.instance.transform.position;
        CameraFollower.instance.SetYOffset(pos.y + deltaY);
        CameraFollower.instance.transform.position = new Vector3(pos.x, pos.y + deltaY, pos.z + deltaZ);
    }

    private void UpdateRotation()
    {
        var deltaRot = forward * rotSpeed * Time.deltaTime;
        var camTransform = CameraFollower.instance.transform;
        var eulers = camTransform.rotation.eulerAngles + new Vector3(deltaRot, 0, 0);
        camTransform.rotation = Quaternion.Euler(eulers);
    }
}
