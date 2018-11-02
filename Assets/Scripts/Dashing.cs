using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashing : MonoBehaviour {

	// Variables éditables
	[SerializeField]
    private float DashSpeed = 20f;
	[SerializeField]
    private float DashCoolDown = 3f;
	[SerializeField]
    private float DashDuration = 0.5f;
	[SerializeField]
	private float DashDeceleration = 0f;
	
	// Variables d'état
	private bool isDashing = false;
	private bool canDash = true;
	public bool IsDashing { get => isDashing; set => isDashing = value; }
	public bool CanDash { get => canDash; set => canDash = value; }

	// Variables internes
	private Vector3 moveDirection;
	private float currentSpeed = 0;
	private float currentDashTime = 0f;
	private float currentYPosition = 0f;

	// Variables externes	
	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.V) && CanDash)
        {
			CanDash = false;
			IsDashing = true;

			currentDashTime = 0.0f;
			currentSpeed = DashSpeed;
			currentYPosition = transform.position.y;

			StartCoroutine(WaitForDashCoolDown());
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
			moveDirection = new Vector3(0, 0, 0);
			IsDashing = false;
		}

		transform.Translate(moveDirection * deltaTime);

		// Air Dash
		if (!GetComponent<Jumper>().OnGround) 
		{
			// Keep height (y position) with air dash
			transform.position = new Vector3(transform.position.x, currentYPosition, transform.position.z);
		}

		// Update animator
        animator.SetFloat("Speed", 1f);
    }

	IEnumerator WaitForDashCoolDown()
	{
		yield return new WaitForSeconds(DashCoolDown);
		CanDash = true;
	}
}
