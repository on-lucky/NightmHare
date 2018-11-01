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
    private Dashing dashing; 
    private bool onGround = false;

    public bool OnGround { get => onGround; set => onGround = value; }

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        dashing = GetComponent<Dashing>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && !dashing.IsDashing)
        {
            if (onGround)
            {
                Jump(JumpThrust);
                animator.SetTrigger("Jumping");
            }
            else if (airJumpCount < airJumps.Length)
            {
                Jump(airJumps[airJumpCount].jumpForce);
                airJumpCount++;
                animator.SetTrigger("Jumping");
            }
        }
    }

    private void Jump(float thrust)
    {
        animator.SetBool("AirBorn", true);
        rb.velocity = new Vector3(rb.velocity.x, 0,0);
        rb.AddForce(new Vector3(0, 1, 0) * thrust);
        onGround = false;
    }

    public void Land(Collider other)
    {
        GameObject terrain = other.gameObject;
        GetComponent<Rigidbody>().isKinematic = false;

        if (terrain.tag == "Terrain")
        {
            animator.SetBool("AirBorn", false);
            onGround = true;
        }

        airJumpCount = 0;
    }

    public void Fall()
    {
        onGround = false;
    }

    public bool CheckGround()
    {
        return onGround;
    }
}
