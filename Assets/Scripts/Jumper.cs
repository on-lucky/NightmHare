using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class Jump
{
    [Range(0, 1500f)]
    public float jumpForce = 400f; // upward force of the jump
    [Range(0, 1f)]
    public float speedModifier = 1f; // horizontal speed modifier of the jump (cumulative)
}

public class Jumper : MonoBehaviour {

    [SerializeField]
    private float JumpThrust = 5f;

    [SerializeField]
    private BoxCollider groundCheck;

    /* SAUTS CONSECUTIFS */
    [SerializeField] Jump[] airJumps;                   // the number of consecutive air jumps that the character can make    
    private int airJumpCount = 0;

    private Animator animator;
    private Rigidbody rb;
    private bool onGround = false;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround)
            {
                Jump(JumpThrust);
            }
            else if(airJumps.Length > 0)
            {
                Jump(airJumps[airJumpCount].jumpForce);
            }
        }
    }

    private void Jump(float thrust)
    {
        animator.SetBool("Jumping", true);
        rb.velocity = new Vector3(rb.velocity.x, 0,0);
        rb.AddForce(new Vector3(0, 1, 0) * thrust);
        onGround = false;
    }

    void OnTriggerEnter(Collider other)
    {
        animator.SetBool("Jumping", false);
        onGround = true;
    }

    void OnTriggerExit(Collider other)
    {
        onGround = false;
    }

    public bool CheckGround()
    {
        return onGround;
    }
}
