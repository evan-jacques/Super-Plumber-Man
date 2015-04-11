//Based upon code from official Unity 2D platformer tutorial

using UnityEngine;
using System.Collections;

public class PlumberAI : MonoBehaviour {
	
	public float moveForce = 365f;
	public float maxSpeed = 10f;
	public float jumpForce = 120f;
	public Transform groundCheck1;
	public Transform groundCheck2;
	public Transform groundCheck3;
	
	public bool running = false;
	private PlumberBehavior pb;
	
	private bool grounded = false;
	private Animator anim;
	private Rigidbody2D rb2d;
	
	private bool moving = false;
	
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
		grounded = Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground")) ||
			Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground")) ||
				Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground"));
		

	}
	
	void FixedUpdate()
	{
		if (!running)
			return;


	}
}