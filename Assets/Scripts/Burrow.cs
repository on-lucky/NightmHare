﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burrow : MonoBehaviour {

    // Other end of the burrow
    [SerializeField]
    private Burrow end;

    // Whether the burrow is locked or not
    [SerializeField]
    private bool locked = false;


    [SerializeField]
    private MeshRenderer arrow;                 // the arrow floating above the burrow
    [SerializeField]
    private Material arrowShining;              // Material for when the arrow is visible
    [SerializeField]
    private Material arrowFaded;                // Material for when the arrow is transparent

    private bool teleportationEnabled = false;  // If the player is on the burrow
    private GameObject playerObj;

    void Start()
    {
        FadeArrow(true);
    }

    void Update() {
        // TODO Update the material when (un)locked
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (teleportationEnabled)
            {
                TeleportPlayer(playerObj);
            }
        }
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
            teleportationEnabled = true;
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
            teleportationEnabled = false;
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        // Check if the player entered from the right or left
        Transform playerTransform = player.transform;
        Vector3 endSize = end.GetComponent<Collider>().bounds.size;
        //float xOffset = playerTransform.position.x < transform.position.x ? endSize.x / 2 + 0.5f : -(endSize.x / 2 + 0.5f);

        // Teleport the player to the other burrow on the opposite side he entered from
        float playerHeight = player.GetComponent<Collider>().bounds.size.y;
        Vector3 otherEndPos = end.transform.position;
        float y = otherEndPos.y - endSize.y / 2 + playerHeight / 2 + 0.5f;
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

    public void Lock()
    {
        locked = true;
    }

    public void Unlock()
    {
        locked = false;
    }
    
}
