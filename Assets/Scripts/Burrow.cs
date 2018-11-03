using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burrow : MonoBehaviour {

    // Other end of the burrow
    [SerializeField]
    private Burrow end;

    // Whether the burrow is locked or not
    [SerializeField]
    private bool locked = false;

    // If the player is on top of the burrow
    private bool playerInRange = false;

    // If the teleportation is enabled or not
    private bool teleportationEnabled = true;

    // The player GameObject
    private GameObject playerObj;

    // Floating object above the burrow
    private MeshRenderer floater;

    // Material for when the player is in range
    [SerializeField]
    private Material inRangeMaterial;

    // Material for when the player is away
    [SerializeField]
    private Material awayMaterial;

    // Material for when the burrow is locked
    [SerializeField]
    private Material lockedMaterial;

    [SerializeField]
    private Hinter hinter;

    void Start()
    {
        FadeArrow(true);
        floater = transform.Find("Floater").GetComponent<MeshRenderer>();
    }

    void Update() {
        // Update floater material
        if (!locked)
        {
            FadeArrow(!playerInRange);
        } else
        {
            floater.material = lockedMaterial;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (playerInRange && teleportationEnabled && !locked)
            {
                TeleportPlayer(playerObj);
            }
        }
        else
        {
            // Allow teleportation if the user stops holding the down arrow
            teleportationEnabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;
        if (!locked && go.tag == "Player")
        {
            playerObj = go;
            playerInRange = true;
            if (hinter)
            {
                hinter.StartTimer();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject go = other.gameObject;
        if (!locked && go.tag == "Player")
        {
            playerInRange = false;
            teleportationEnabled = true;
            if (hinter)
            {
                hinter.StopTimer();
            }
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        // Disable other burrow to avoid loops
        end.DisableTeleportation();

        // Teleport the player to the other burrow
        Transform playerTransform = player.transform;
        Vector3 endSize = end.GetComponent<Collider>().bounds.size;
        Vector3 playerSize = player.GetComponent<Collider>().bounds.size;
        Vector3 otherEndPos = end.transform.position;
        float y = otherEndPos.y - endSize.y / 2 + playerSize.y / 2;
        playerTransform.position = new Vector3(otherEndPos.x, y, otherEndPos.z);
    }

    private void FadeArrow(bool shouldFade)
    {
        
        if (floater != null)
        {
            if (shouldFade)
            {
                floater.material = awayMaterial;
            }
            else
            {
                floater.material = inRangeMaterial;
            }
        }
    }

    public void DisableTeleportation() { teleportationEnabled = false; }

    public void Lock() { locked = true; }

    public void Unlock(bool both = true) {
        locked = false;
        if (both)
        {
            end.Unlock(false);
        }
    }
    
}
