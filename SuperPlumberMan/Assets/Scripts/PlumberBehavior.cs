//Based upon code from official Unity 2D platformer tutorial

using UnityEngine;
using System.Collections;

public class PlumberBehavior : MonoBehaviour {
	
	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jump = false;
	public float moveForce = 365f;
	public float maxSpeed = 10f;
	public float jumpForce = 120f;
	public Transform groundCheck1;
	public Transform groundCheck2;
	public Transform groundCheck3;
	
	
	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;

	private float jumpTime;

	private bool moving = false;
	
	// Use this for initialization
	void Awake () 
	{
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground")) ||
				Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground")) ||
				Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground"));

		if (grounded && (rb2d.velocity.x == 0)) {
			if(moving == true)
				moving = false;
				anim.SetBool("moving", false);
		} else {
			if(moving == false)
				moving = true;
				anim.SetBool ("moving", true);
		}

		if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
		{
			jump = true;
		}
	}
	
	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");
		
		//anim.SetFloat("Speed", Mathf.Abs(h));

		float accelerationMultiplier = 2F;

		if (h * rb2d.velocity.x < maxSpeed)
			rb2d.AddForce(Vector2.right * h * moveForce);
		
		if (Mathf.Abs (rb2d.velocity.x) > maxSpeed)
			rb2d.velocity = new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

		if (h == 0 && grounded && rb2d.velocity.x != 0) { //slow down faster when on ground and not moving
			rb2d.AddForce(Vector2.right * -rb2d.velocity.x *2F);
		}

		if (h > 0 && !facingRight)
			Flip ();
		else if (h < 0 && facingRight)
			Flip ();
		
		if (jump)
		{
			//anim.SetTrigger("Jump");
			rb2d.AddForce(new Vector2(0f, jumpForce));
			jump = false;
			jumpTime = Time.fixedTime;
		}

		if (Input.GetKey (KeyCode.UpArrow) && rb2d.velocity.y > 0 && Time.fixedTime - jumpTime < .3F) { //holding space while jumping
			rb2d.AddForce(Vector2.up * jumpForce/30);
		}
	}
	
	
	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}