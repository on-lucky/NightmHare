using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digger : MonoBehaviour {
    
    private bool diggingIn = false;
    private bool diggingOut = false;

    private Animator animator;
    private Rigidbody rb;
    private PlayerController controller;
    private Jumper jumper;
    private Dashing dashing;

    private Burrow origin;
    private Burrow destination;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<PlayerController>();
        jumper = GetComponent<Jumper>();
        dashing = GetComponent<Dashing>();
    }

    private void Update()
    {
        if (diggingIn && isAnimationPlaying("DigOut"))
        {
            // Teleport the player when dig in animation ends
            diggingIn = false;
            Teleport();
            diggingOut = true;
        }
        else if (diggingOut && !isAnimationPlaying("DigOut"))
        {
            // Give control back when dig out animation ends
            diggingOut = false;

            // FIX: Change z plane in player controller
            controller.SetZPos(transform.position.z);

            // reenable player controls
            SetScriptsEnabled(true);

            // make the character vulnerable again
            GetComponent<LifeManager>().SetInvinsible(false);
        }
    }

    private bool isAnimationPlaying(string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public void Dig(Burrow origin, Burrow destination, bool doCameraSlide = true)
    {
        if (!diggingIn && !diggingOut)
        {
            this.origin = origin;
            this.destination = destination;

            // disable player controls and stop movement
            SetScriptsEnabled(false);
            controller.SetCurrentSpeed(0);
            rb.velocity = Vector3.zero;

            // Move player to center of burrow for better effect
            Vector3 pos = transform.position;
            Vector3 originPos = origin.transform.position;
            transform.position = new Vector3(originPos.x, pos.y, originPos.z);

            if (doCameraSlide)
            {
                SlideCamera();
            }

            AudioManager.instance.PlaySound("Digging", 1f, 0f, false);

            diggingIn = true;
            animator.SetTrigger("Dig");
            GetComponent<LifeManager>().SetInvinsible(true);
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

    private void SetScriptsEnabled(bool enable)
    {
        controller.enabled = enable;
        jumper.enabled = enable;
        dashing.enabled = enable;
    }

}
