//Based upon code from official Unity 2D platformer tutorial

using UnityEngine;
using System.Collections;

public class PlumberBehavior : MonoBehaviour {
	
	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jumpVar = false;
	public float moveForce = 365f;
	public float maxSpeed = 10f;
	public float jumpForce = 120f;
	public Transform groundCheck1;
	public Transform groundCheck2;
	public Transform groundCheck3;
	
	private float horizInput;
	private PlumberAI pai;

	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;

	private float jumpTime;

	private bool moving = false;

	private bool AIRunning;

	// Use this for initialization
	void Awake () 
	{
		pai = GetComponent<PlumberAI> ();
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		startAI ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		grounded = Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground")) ||
				Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground")) ||
				Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground"));

		if (rb2d.velocity.x == 0) {
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
			stopAI ();
			jumpVar = true;
		}
	}
	
	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");

		if (h != 0 && AIRunning) {
			horizInput = h;
			stopAI ();
		} else if (!AIRunning) {
			horizInput = h;
		}
		
		//anim.SetFloat("Speed", Mathf.Abs(h));

		if (horizInput * rb2d.velocity.x < maxSpeed)
			rb2d.AddForce(Vector2.right * horizInput * moveForce);
		
		if (Mathf.Abs (rb2d.velocity.x) > maxSpeed)
			rb2d.velocity = new Vector2(Mathf.Sign (rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

		if (horizInput == 0 && grounded && rb2d.velocity.x != 0) { //slow down faster when on ground and not moving
			float accelerationMultiplier = 100F;
			rb2d.AddForce(Vector2.right * -rb2d.velocity.x * accelerationMultiplier);
		}

		if (horizInput > 0 && !facingRight)
			Flip ();
		else if (horizInput < 0 && facingRight)
			Flip ();
		
		if (jumpVar)
		{
			jump();
		}

		if (Input.GetKey (KeyCode.UpArrow) && rb2d.velocity.y > 0 && Time.fixedTime - jumpTime < .3F) { //holding space while jumping
			rb2d.AddForce(Vector2.up * jumpForce/30);
		}
	}

	public void jump()
	{
		//anim.SetTrigger("Jump");
		rb2d.AddForce(new Vector2(0f, jumpForce));
		jumpVar = false;
		jumpTime = Time.fixedTime;
	}
	
	void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void startAI()
	{
		pai.activate ();
		AIRunning = true;
	}

	private void stopAI()
	{
		pai.deactivate();
		AIRunning = false;
	}
}