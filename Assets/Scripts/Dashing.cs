using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour {

	// Editable variables
	[SerializeField]
    private float DashSpeed = 20f;
	[SerializeField]
    private float DashDuration = 0.5f;
	[SerializeField]
    private float DashCoolDown = 3f;
	[SerializeField]
	private float DashDeceleration = 0f;
	
	// State variables
	private bool isDashing = false;
	private bool canDash = true;
	public bool IsDashing { get => isDashing; set => isDashing = value; }
	public bool CanDash { get => canDash; set => canDash = value; }

	// Internal variables
	private Vector3 moveDirection;
	private float currentSpeed = 0;
	private float currentDashTime = 0f;
	private float currentYPosition = 0f;

	// External variables	
	private Animator animator;
	private Climber climber;
    private PlayerController playerController;

    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator>();
		climber = GetComponent<Climber>();
        playerController = GetComponent<PlayerController>();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R) && CanDash)
        {
			CanDash = false;
			IsDashing = true;

			currentDashTime = 0.0f;
			currentSpeed = DashSpeed;
			currentYPosition = transform.position.y;

            playerController.SetCurrentSpeed(0.2f);

            StartCoroutine(WaitForDashCoolDown());

            playerController.EnableInput(false);

            // Update animator
            animator.SetTrigger("Dash");
        }
		
		if (IsDashing)
        {
			Dash();
        }
	}

	private void Dash()
    {
		float deltaTime = Time.deltaTime;

		if (currentDashTime < DashDuration)
		{
			moveDirection = new Vector3(0, 0, currentSpeed);
			currentDashTime += deltaTime;
			currentSpeed -= DashDeceleration * deltaTime;
		}
		else
		{
            // Stop dashing
            StopDashing();
        }

		if (!climber.WallNearby) 
		{
			// Dash
			transform.Translate(moveDirection * deltaTime);

			// Air Dash
			if (!GetComponent<Jumper>().OnGround) 
			{
				// Keep height (y position) with air dash
				transform.position = new Vector3(transform.position.x, currentYPosition, transform.position.z);
			}
		}
		else 
		{
            // Stop dashing, we're hitting a wall
            StopDashing();
        }
        
    }

    private void StopDashing()
    {
        IsDashing = false;
        playerController.EnableInput(true);
        playerController.SetCurrentSpeed(currentSpeed);
        Vector3 v = GetComponent<Rigidbody>().velocity;
        v.y = 0;
        GetComponent<Rigidbody>().velocity = v;
    }

	IEnumerator WaitForDashCoolDown()
	{
		yield return new WaitForSeconds(DashCoolDown);
		CanDash = true;
	}
}
