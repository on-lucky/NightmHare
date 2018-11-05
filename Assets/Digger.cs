using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour {

    private bool isDigging = false;

    private Animator animator;
    private Rigidbody rb;
    private PlayerController controller;

    private Burrow origin;
    private Burrow destination;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<PlayerController>();
    }

    private void LateUpdate()
    {
        if (isDigging && !isAnimationPlaying())
        {
            // The animation just ended so teleport the player
            isDigging = false;
            Teleport();

            // reenable player controls
            controller.EnableInput(true);
        }
    }

    private bool isAnimationPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Dig");
    }

    public void Dig(Burrow origin, Burrow end)
    {
        if (!isDigging)
        {
            this.origin = origin;
            this.destination = end;

            // Disable player movemnet, move player to center of burrow for better effect
            rb.velocity = Vector3.zero;
            controller.EnableInput(false);
            Vector3 pos = transform.position;
            Vector3 originPos = origin.transform.position;
            transform.position = new Vector3(originPos.x, pos.y, originPos.z);

            SlideCamera();

            isDigging = true;
            animator.SetTrigger("Dig");
        }
        // else finish digging first
    }

    public void Teleport()
    {
        // Disable other burrow to avoid loops
        destination.DisableTeleportation();

        // Teleport the player to the other burrow
        Vector3 endSize = destination.GetComponent<Collider>().bounds.size;
        Vector3 playerSize = GetComponent<Collider>().bounds.size;
        Vector3 destinationPos = destination.transform.position;
        float y = destinationPos.y - endSize.y / 2 + playerSize.y / 2;
        transform.position = new Vector3(destinationPos.x, y, destinationPos.z);
    }

    private void SlideCamera()
    {
        Vector3 destinationPos = destination.transform.position;
        
        float deltaCamY = CameraFollower.instance.transform.position.y - origin.transform.position.y;

        float posCamZ = -10;
        if (destinationPos.z > 5)
        {
            posCamZ = -5;
        }

        CameraFollower.instance.SlideTo(new Vector3(destinationPos.x, destinationPos.y + deltaCamY, posCamZ));
    }

}
