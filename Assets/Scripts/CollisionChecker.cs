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

    private int groundIndex = 0;

    private void Start()
    {
        GameObject hare = transform.parent.parent.gameObject;
        if (hare)
        {
            hare.GetComponent<LifeManager>().HareDied += OnHareDied;
        }
        else
        {
            Debug.LogError("Hare GameObject cannot be found!");
        }
    }

    private void OnHareDied(object sender, System.EventArgs e)
    {
        // Quick fix for bug B33
        groundIndex = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        switch (type)
        {
            case CheckerType.Ground:
                if (obj.tag == "Ground")
                {
                    groundIndex++;
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
                    groundIndex--;
                    if(groundIndex == 0){
                        messageReceiver.GetComponent<Jumper>().Fall();
                    }
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
