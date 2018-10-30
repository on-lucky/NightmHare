using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burrow : MonoBehaviour {

    [SerializeField]
    private Burrow end;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        // TODO fix problem with wall checks where wall checks trigger this instead
        // For now the wall checks are disabled in the scene
        GameObject go = other.gameObject;
        if (go.tag == "Player")
        {
            TeleportPlayer(go);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        // Check if the player entered from the right or left
        Transform playerTransform = player.transform;
        Vector3 endSize = end.GetComponent<Collider>().bounds.size;
        float xOffset = playerTransform.position.x < transform.position.x ? endSize.x / 2 + 0.5f : -(endSize.x / 2 + 0.5f);

        // Teleport the player to the other burrow on the opposite side he entered from
        float playerHeight = player.GetComponent<Collider>().bounds.size.y;
        Vector3 otherEndPos = end.transform.position;
        float y = otherEndPos.y - endSize.y / 2 + playerHeight / 2;
        playerTransform.position = new Vector3(otherEndPos.x + xOffset, y, otherEndPos.z);
    }
}
