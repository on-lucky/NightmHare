﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CheckerType
{
    Ground,
    FrontWall,
    BackWall,
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
    }
}
