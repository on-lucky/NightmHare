using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransform : MonoBehaviour {

    // How long to do the transformations
    [SerializeField] private float duration = 1;

    // Transformations to do on the camera
    [SerializeField] private bool doTranslation = true;
    [SerializeField] private bool doRotation = true;
    [SerializeField] private float offsetY = 0;
    [SerializeField] private float z = 0;
    [SerializeField] private float rotation = 0;

    private bool go = false;
    private float ySpeed = 1;
    private float zSpeed = 1;
    private float rotSpeed = 1;
    private float currentTime = 0;

    // Original values
    private float originOffsetY = 0;
    private float originZ = 0;
    private float originRotation = 0;

    // Destination
    private float finalOffsetY = 0;
    private float finalZ = 0;
    private float finalRotation = 0;

    private void Start()
    {
        // Save original values
        originOffsetY = CameraFollower.instance.GetYOffset();
        originZ = Camera.main.transform.position.z;
        originRotation = Camera.main.transform.rotation.eulerAngles.x;
    }

    private void Update()
    {
        if (go)
        {
            currentTime += Time.deltaTime;
            
            if (doTranslation) UpdateTranslation();
            if (doRotation) UpdateRotation();

            if (currentTime >= duration)
            {
                go = false;

                // Clip final values to what they should really be to avoid accumulating floating point errors
                var camTransform = CameraFollower.instance.transform;
                if (doTranslation)
                {
                    camTransform.position = new Vector3(camTransform.position.x, camTransform.position.y, finalZ);
                    CameraFollower.instance.SetYOffset(finalOffsetY);
                }
                if (doRotation)
                {
                    camTransform.rotation = Quaternion.AngleAxis(finalRotation, Vector3.right);
                }
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // Calculate speeds
            var yOffset = CameraFollower.instance.GetYOffset();
            var camPos = Camera.main.transform.position;
            ySpeed = (offsetY - yOffset) / duration;
            zSpeed = (z - camPos.z) / duration;

            // Calculate rotation speed
            var camRotation = Camera.main.transform.rotation.eulerAngles;
            rotSpeed = Mathf.DeltaAngle(camRotation.x, rotation) / duration;

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
            // Calculate speeds to restore original values
            var yOffset = CameraFollower.instance.GetYOffset();
            var camPos = Camera.main.transform.position;
            ySpeed = (originOffsetY - yOffset) / duration;
            zSpeed = (originZ - camPos.z) / duration;

            // Calculate rotation speed
            var camRotation = Camera.main.transform.rotation.eulerAngles;
            rotSpeed = Mathf.DeltaAngle(camRotation.x, originRotation) / duration;

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
        var deltaY = ySpeed * Time.deltaTime;
        var deltaZ = zSpeed * Time.deltaTime;
        var camPos = CameraFollower.instance.transform.position;

        // Apply y offset
        var yOffset = CameraFollower.instance.GetYOffset();
        CameraFollower.instance.SetYOffset(yOffset + deltaY);

        // Apply z translation
        CameraFollower.instance.transform.position = new Vector3(camPos.x, camPos.y, camPos.z + deltaZ);
    }

    private void UpdateRotation()
    {
        var deltaRot = rotSpeed * Time.deltaTime;
        var camTransform = CameraFollower.instance.transform;
        var xAngle = camTransform.rotation.eulerAngles.x;
        camTransform.rotation = Quaternion.AngleAxis(xAngle+deltaRot, Vector3.right);
    }
}
