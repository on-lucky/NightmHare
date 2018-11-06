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
        GameObject obj = other.gameObject;
        switch (type)
        {
            case CheckerType.Ground:
                if (obj.tag == "Ground")
                {
                    messageReceiver.GetComponent<Jumper>().Land(other);
                }
                break;
            case CheckerType.FrontWall:
                if (obj.tag == "Wall")
                {
                    messageReceiver.GetComponent<Climber>().HitWall(other, true);
                }
                break;
            case CheckerType.BackWall:
                if (obj.tag == "Wall")
                {
                    messageReceiver.GetComponent<Climber>().HitWall(other, false);
                }
                break;
            case CheckerType.Wall:
                if (obj.tag == "Wall")
                {
                    messageReceiver.GetComponent<Climber>().RegisterWallNearby(other, true);
                }
                break;
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject obj = other.gameObject;

        switch (type)
        {
            case CheckerType.Ground:
                if (obj.tag == "Ground")
                {
                    Debug.Log("fall");
                    messageReceiver.GetComponent<Jumper>().Fall();
                }
                break;
            case CheckerType.FrontWall:
                if (obj.tag == "Wall")
                {
                    messageReceiver.GetComponent<Climber>().LeaveWall(other, true);
                }
                break;
            case CheckerType.BackWall:
                if (obj.tag == "Wall")
                {
                    messageReceiver.GetComponent<Climber>().LeaveWall(other, false);
                }
                break;
            case CheckerType.Wall:
                if (obj.tag == "Wall")
                {
                    messageReceiver.GetComponent<Climber>().RegisterWallNearby(other, false);
                }
                break;
        }
    }
}
