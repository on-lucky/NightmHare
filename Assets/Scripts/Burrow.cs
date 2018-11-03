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

    // Arrow floating above the burrow
    private MeshRenderer arrow;

    // Lock floating above the burrow
    private MeshRenderer lockIcon;

    // Material for when the arrow is visible
    [SerializeField]
    private Material arrowShining;

    // Material for when the arrow is transparent
    [SerializeField]
    private Material arrowFaded;

    // If the player is on top of the burrow
    private bool playerInRange = false;

    // If the teleportation is enabled or not
    private bool teleportationEnabled = true;

    private GameObject playerObj;

    void Start()
    {
        FadeArrow(true);
        Transform floater = transform.Find("Floater");
        arrow = floater.Find("Arrow").GetComponent<MeshRenderer>();
        lockIcon = floater.Find("Lock").GetComponent<MeshRenderer>();
    }

    void Update() {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (playerInRange && teleportationEnabled && !locked)
            {
                TeleportPlayer(playerObj);
            }
        }

        // Update the material when (un)locked
        arrow.gameObject.SetActive(!locked);
        lockIcon.gameObject.SetActive(locked);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // TODO fix problem with wall checks where wall checks trigger this instead
        // For now the wall checks are disabled in the scene
        GameObject go = other.gameObject;
        if (!locked && go.tag == "Player")
        {
            playerObj = go;
            FadeArrow(false);
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // TODO fix problem with wall checks where wall checks trigger this instead
        // For now the wall checks are disabled in the scene
        GameObject go = other.gameObject;
        if (!locked && go.tag == "Player")
        {
            FadeArrow(true);
            playerInRange = false;
            teleportationEnabled = true;
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        // Disable other burrow to avoid loops
        end.DisableTeleportation();

        Transform playerTransform = player.transform;
        Vector3 endSize = end.GetComponent<Collider>().bounds.size;

        // Teleport the player to the other burrow
        float playerHeight = player.GetComponent<Collider>().bounds.size.y;
        Vector3 otherEndPos = end.transform.position;
        float y = otherEndPos.y - endSize.y / 2 + playerHeight / 2;
        playerTransform.position = new Vector3(otherEndPos.x, y, otherEndPos.z);
    }

    private void FadeArrow(bool shouldFade)
    {
        
        if (arrow)
        {
            if (shouldFade)
            {
                arrow.material = arrowFaded;
            }
            else
            {
                arrow.material = arrowShining;
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
