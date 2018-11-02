using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CheckerType
{
    Ground,
    FrontWall,
    BackWall,
    Wall
}

public class CollisionChecker : MonoBehaviour {

    public CheckerType type;

    public GameObject messageReceiver;

    void OnTriggerEnter(Collider other)
    {
        if(type == CheckerType.Ground)
        {
            messageReceiver.GetComponent<Jumper>().Land(other);
        }
        else if (type == CheckerType.FrontWall)
        {
            messageReceiver.GetComponent<Climber>().HitWall(other, true);
        }
        else if (type == CheckerType.BackWall)
        {
            messageReceiver.GetComponent<Climber>().HitWall(other, false);
        }
        else if (type == CheckerType.Wall)
        {
            messageReceiver.GetComponent<Climber>().RegisterWallNearby(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (type == CheckerType.Ground)
        {
            messageReceiver.GetComponent<Jumper>().Fall();
        }
        else if (type == CheckerType.FrontWall)
        {
            messageReceiver.GetComponent<Climber>().LeaveWall(other, true);
        }
        else if (type == CheckerType.BackWall)
        {
            messageReceiver.GetComponent<Climber>().LeaveWall(other, false);
        }
        else if (type == CheckerType.Wall)
        {
            messageReceiver.GetComponent<Climber>().RegisterWallNearby(false);
        }
    }
}
