//Based upon code from official Unity 2D platformer tutorial

using UnityEngine;
using System.Collections;

public class PlumberAI : MonoBehaviour {

	public bool running = false;
	private PlumberBehavior pb;
	private Animator anim;
	private Rigidbody2D rb2d;
	public Transform groundCheck1;
	public Transform groundCheck2;
	public Transform groundCheck3;
	public Transform airCheck;
	public Transform airCheck2;
	
	// Use this for initialization
	void Awake () 
	{
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		pb = GetComponent<PlumberBehavior>();
	}
	
	public void activate()
	{
		running = true;
	}
	
	public void deactivate()
	{
		running = false;
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	
	void FixedUpdate()
	{
		float fwd = pb.facingRight ? 1 : -1;
		if (!running)
			return;

		bool grounded = Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground")) ||
			Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground")) ||
				Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground"));

		RaycastHit2D groundInFront = Physics2D.Linecast(airCheck.position, airCheck2.position, 1 << LayerMask.NameToLayer("Ground"));
		RaycastHit2D towardsGround = (Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(1, -1) * 4, 1 << LayerMask.NameToLayer("Ground")));
		RaycastHit2D checkAhead = (Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(1, -1) * 6));
		RaycastHit2D downToGround = (Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(1, -10), 1 << LayerMask.NameToLayer("Ground")));

		//We test a few directions and use that to guide us to the ground
		pb.setHorizInput (1); //default move is to go right, then we adjust it

		//check whether we are heading toward land while in the air
		if (!grounded && rb2d.velocity.y <0 && (getType (checkAhead) == "Spikes" || getType (checkAhead) == " ") && getType (downToGround) == "Platform") {
			if(rb2d.velocity.x != 0)
			{
				pb.setHorizInput(Mathf.Sign (rb2d.velocity.x) * -1);
			} 
		}



		//check whether we should keep going up or to jump
		if (!groundInFront && !towardsGround && !downToGround) {
			pb.setUpInput (1.0F);
		} else {
			pb.setUpInput(0.0F);
		}
	}

	private string getType(RaycastHit2D hit)
	{
		if (!hit)
			return " ";
		Collider2D c = hit.collider;
		GameObject go = c.gameObject;
		if (go.name == "Platform(Clone)")
			return "Platform";
		if (go.name == "Spikes(Clone)")
			return "Spikes";
		if (go.name == "MovingEnemy(Clone)")
			return "Mover";
		if (go.name == "MovingEnemy(Clone)")
			return "Mover";
		return " ";
	}
}