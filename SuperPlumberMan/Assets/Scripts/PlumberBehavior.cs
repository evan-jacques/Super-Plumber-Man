//Based upon code from official Unity 2D platformer tutorial

using UnityEngine;
using System.Collections;

public class PlumberBehavior : MonoBehaviour {

	private int lives = 3;
	private float invulnTime;

	//Movement related
	private float horizInput;
	private float upInput;
	private float jumpTime;
	[HideInInspector] public bool facingRight = true;
	[HideInInspector] public bool jumpVar = false;
	public float moveForce = 365f;
	public float maxSpeed = 10f;
	public float jumpForce = 120f;
	private bool grounded = false;
	private GameObject mostRecentPlatform = null;

	public Transform groundCheck1;
	public Transform groundCheck2;
	public Transform groundCheck3;
	private Animator anim;
	private Rigidbody2D rb2d;

	// AI related
	private PlumberAI pai;
	private bool moving = false;
	private bool AIRunning = true;
	private float lastInputTime = 0;

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
		RaycastHit2D onGround = Physics2D.Linecast (transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer ("Ground"));
		if(!onGround)
		{
			onGround = Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground"));
		}
		if(!onGround)
		{
			onGround = Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground"));
		}
		if (onGround) {
			grounded = true;
			mostRecentPlatform = onGround.collider.gameObject;
		} else {
			grounded = false;
		}
		if (rb2d.velocity.x == 0) {
			if(moving == true)
				moving = false;
				anim.SetBool("moving", false);
		} else {
			if(moving == false)
				moving = true;
				anim.SetBool ("moving", true);
		}
	}

	void OnTriggerEnter2D(Collider2D c)
	{
		PopupController puc = c.gameObject.GetComponent<PopupController>();
		MovingEnemyController mec = c.gameObject.GetComponent<MovingEnemyController>();

		if (mec != null) {
			respawn ();
		} else if (puc != null) {
			respawn ();
		} else if (getType (c.gameObject) == "Spikes") {
			respawn();
		}

	}

	void FixedUpdate()
	{
		if (lives <= 0) {
			if (Input.GetKey (KeyCode.Space)) {
				lives = 3;
			}
			return;
		}
		if (transform.position.y < -55)
			respawn ();

		if (Time.time - lastInputTime > 10)
			startAI ();

		//Getting Inputs -----------------------------------------------------
		float h = Input.GetAxis("Horizontal");
		if (h != 0 && AIRunning) {
			horizInput = h;
			stopAI ();
		} else if (!AIRunning) {
			horizInput = h;
		}
		if (Input.GetKey (KeyCode.UpArrow) && AIRunning) {
			upInput = 1;
			stopAI ();
		} else if (!AIRunning) {
			upInput = Input.GetKey (KeyCode.UpArrow)? 1 : 0;
		}
		if (upInput == 1 && grounded) {
			if(AIRunning && Input.GetKey (KeyCode.UpArrow))
			{
				stopAI ();
			}
			jumpVar = true;
		}

		//Using Inputs ---------------------------------------------------------
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
			mostRecentPlatform = currentPlatform();
			jump();
		}

		if (upInput == 1 && rb2d.velocity.y > 0 && Time.fixedTime - jumpTime < .4F) { //holding space while jumping
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

	GameObject currentPlatform()
	{

		RaycastHit2D p1 = Physics2D.Linecast (transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer ("Ground"));
		RaycastHit2D p2 = Physics2D.Linecast (transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer ("Ground"));
		RaycastHit2D p3 = Physics2D.Linecast (transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer ("Ground"));
		if (p1)
			return p1.collider.gameObject;
		if (p2)
			return p2.collider.gameObject;
		if (p3)
			return p3.collider.gameObject;
		return null;
	}

	public void setHorizInput(float input)
	{
		horizInput = input;
	}

	public void setUpInput(float input)
	{
		upInput = input;
	}

	private void startAI()
	{
		pai.activate ();
		AIRunning = true;
	}

	private void stopAI()
	{
		lastInputTime = Time.time;
		pai.deactivate();
		AIRunning = false;
	}

	void OnGUI()
	{
		Rect curInfo = new Rect (Screen.width / 2 - Screen.height / 10, Screen.height / 10, Screen.width / 5, Screen.height / 20);
		Rect lostOrWon = new Rect (Screen.width / 2 - Screen.width / 16, Screen.height / 5, Screen.width / 8, Screen.height / 20);
		GUI.Box (curInfo, "Current Lives : " + lives + "\nDistance Reached : " + (int)transform.position.x);
		if (lives == 0)
		{
			GUI.Box (lostOrWon, "YOU HAVE LOST, SORRY");
		}
	}

	private void respawn()
	{
		if (Time.time - invulnTime < 3)
			return; //3 second invulnerability period
		lives --;
		invulnTime = Time.time;

		RaycastHit2D hitDown= Physics2D.Linecast (transform.position + new Vector3(0, 0, 0), transform.position + new Vector3(0, -4, 0), 1 << LayerMask.NameToLayer("Ground"));
		if (hitDown)
			return;
		RaycastHit2D hitUp = Physics2D.Linecast (transform.position + new Vector3(0, 0, 0), transform.position + new Vector3(0, 40, 0), 1 << LayerMask.NameToLayer("Ground"));
		if (hitUp)
		{
			transform.position = hitUp.transform.position + new Vector3(0, 1, 0);
		}

		RaycastHit2D sideHitUp = Physics2D.Linecast (transform.position + new Vector3(-5, 0, 0), transform.position + new Vector3(-5, 40, 0), 1 << LayerMask.NameToLayer("Ground"));
		if (sideHitUp)
		{
			transform.position = sideHitUp.transform.position + new Vector3(0, 1, 0);
		}

		transform.position = mostRecentPlatform.transform.position + new Vector3 (0, 1, 0);
	}

	private string getType(GameObject hit)
	{
		if (!hit)
			return " ";
		if (hit.name == "Platform(Clone)")
			return "Platform";
		if (hit.name == "Spikes(Clone)")
			return "Spikes";
		if (hit.name == "MovingEnemy(Clone)")
			return "Mover";
		if (hit.name == "PopupEnemy(Clone)")
			return "Popup";
		return " ";
	}
}


















