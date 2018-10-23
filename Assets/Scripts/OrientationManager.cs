using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationManager : MonoBehaviour {

    private bool isLookingRight = true;
    public Camera PlayerCam;

	public bool LookTo(bool shouldLookRight)
    {
        bool hasTurned = false;
        if (isLookingRight && !shouldLookRight)
        {
            transform.eulerAngles = new Vector3(0, -90, 0);

            if (PlayerCam)
            {
                PlayerCam.transform.localPosition = new Vector3(-50, 10, 0);
                PlayerCam.transform.localEulerAngles = new Vector3(0, 90, 0);
            }
            
            isLookingRight = false;
            hasTurned = true;
        }
        else if(!isLookingRight && shouldLookRight)
        {
            transform.eulerAngles = new Vector3(0, 90, 0);

            if (PlayerCam)
            {
                PlayerCam.transform.localPosition = new Vector3(50, 10, 0);
                PlayerCam.transform.localEulerAngles = new Vector3(0, -90, 0);
            }

            isLookingRight = true;
            hasTurned = true;
        }
        return hasTurned;
    }
}
