//Based upon code from official Unity 2D platformer tutorial

using UnityEngine;
using System.Collections;

public class PlumberAI : MonoBehaviour {

	public bool running = false;
	private PlumberBehavior pb;
	private Animator anim;
	private Rigidbody2D rb2d;

	//for maneuvering the world
	public Transform groundCheck1;
	public Transform groundCheck2;
	public Transform groundCheck3;
	public Transform airCheck;
	public Transform airCheck2;
	GameManager gm;

	//for dealing with enemies
	public GameObject avoidingMover = null;
	public GameObject avoidingPopup = null;

	//for switching levels
	private float lastTimeCheck = 0;

	// Use this for initialization
	void Awake () 
	{
		anim = GetComponent<Animator>();
		rb2d = GetComponent<Rigidbody2D>();
		pb = GetComponent<PlumberBehavior>();
		gm = GameObject.Find("GameManager").GetComponent<GameManager>();
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

		if (Time.time - lastTimeCheck > 20) {
			lastTimeCheck = Time.time;
			considerSuperJump();
		}

		bool grounded = Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground")) ||
			Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground")) ||
				Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground"));

		RaycastHit2D groundInFront = Physics2D.Linecast(airCheck.position, airCheck2.position, 1 << LayerMask.NameToLayer("Ground"));
		RaycastHit2D towardsGround = (Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(1, -1) * 7, 1 << LayerMask.NameToLayer("Ground")));
		RaycastHit2D checkAhead = (Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(1, -1) * 10, 1 << LayerMask.NameToLayer("Default")));
		RaycastHit2D downToGround = (Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(1, -1), 1 << LayerMask.NameToLayer("Ground")));

		//if nearest enemy is a popup enemy, wait for the popper and then keep going
		//if nearest enemy is a mover, wait for it to move toward you and jump over it
		if (avoidingMover) {
			if ((avoidingMover.transform.position - transform.position).x <= 0){
				avoidingMover = null;
				return;
			}
			if(Mathf.Sign (avoidingMover.rigidbody2D.velocity.x) == -1) { //if it is moving toward us we wait until it is close then jump over it
				float dist = (avoidingMover.transform.position - transform.position).magnitude;
				if(dist < 4)
				{
					pb.setUpInput(1.0F);
					pb.setHorizInput(1.0F);
					avoidingMover = null;
				}
				else
				{
					pb.setHorizInput(0.0F);
				}
			}
			else //otherwise we stop
			{
				float dist = (avoidingMover.transform.position - transform.position).magnitude;
				if(dist < 4)
				{
					pb.setHorizInput(0.0F);
				}
				else
				{
					pb.setHorizInput(1.0F);
				}

			}
			return;
		}

		if (avoidingPopup) {
			if ((avoidingPopup.transform.position - transform.position).x <= 0){
				avoidingPopup = null;
				return;
			}
			GameObject popper = avoidingPopup.GetComponentInChildren<PopupController>().gameObject;
			SpriteRenderer sr = popper.GetComponent<SpriteRenderer>();
			RaycastHit2D boxInFront = Physics2D.Linecast(transform.position, popper.transform.position, 1 << LayerMask.NameToLayer("Ground"));
			float dist = (avoidingPopup.transform.position - transform.position).magnitude;
			if(!boxInFront) { //if it is moving toward us we wait until it is close then jump over it
				if(dist >5)
				{
					pb.setHorizInput(1.0F);
				}
				else //we wait until the popper goes down and then jump over it
				{
					pb.setHorizInput(0.0F);
				}
			}
			else
			{
				if(dist > 5)
				{
					pb.setHorizInput(1.0F);
				}
				else
				{
					pb.setUpInput(1.0F);
					pb.setHorizInput(0.0F);
					avoidingPopup = null;
				}
			}
			return;
		}

		//We test a few directions and use that to guide us to the ground
		pb.setHorizInput (1); //default move is to go right, then we adjust it

		//check whether we are heading toward land while in the air
		if (!grounded && rb2d.velocity.y <0 && (getType (checkAhead) == "Spikes" || getType (checkAhead) == " ") && getType (downToGround) == "Platform") {
			if(rb2d.velocity.x != 0)
			{
				pb.setHorizInput(Mathf.Sign (rb2d.velocity.x) * -1);
			}
		}

		//test for enemies nearby only while grounded and can react
		if(grounded)
		{
		GameObject closest = null;
		float minMagnitude = 0;
		foreach(GameObject go in gm.enemies)
		{
			float dist = (go.transform.position - transform.position).magnitude;
			//only count it if it is the closest in the forward direction
			if((closest == null || dist < minMagnitude) && (go.transform.position - transform.position).x > 0)
			{
				closest = go;
				minMagnitude = dist;
			}
		}
		if (getType (closest) == "Mover" && minMagnitude < 10) {
			avoidingMover = closest;
		}
		else if(getType(closest) == "Popup"&& minMagnitude < 10)
		{
			avoidingPopup = closest;
		}
		}

		//check whether we should keep going up or to jump
		if (rb2d.velocity.y >= 0 && groundInFront.collider == null && towardsGround.collider == null) {
			pb.setUpInput (1.0F);
		} else {
			pb.setUpInput(0.0F);
		}
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
		if (go.name == "PopupEnemy(Clone)")
			return "Popup";
		return " ";
	}

	void OnGUI()
	{
		Rect curInfo = new Rect (Screen.width / 2 - Screen.height / 10, 2 * Screen.height / 10, Screen.width / 5, Screen.height / 20);
		GUI.Box (curInfo, "Next Jump Available in : " + (20 - (Time.time - lastTimeCheck)));
	}

	public void considerSuperJump()
	{
		//count the number of enemies and platforms in top row and bottom row in the coming area
		//choose which setup seems safer
		int dangersTop = 0;
		int dangersBot = 0;
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("sectiontop")){
			if (getType (go) == "Spikes" || getType (go) == "Spikes" || getType (go) == "Spikes"){
				if(go.transform.position.x - transform.position.x < 100) //only care about nearby paths
				{
					dangersTop ++;
				}
			}
		}
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("sectionbot")){
			if (getType (go) == "Spikes" || getType (go) == "Spikes" || getType (go) == "Spikes"){
				if(go.transform.position.x - transform.position.x < 100) //only care about nearby paths
				{
					dangersBot ++;
				}
			}
		}
		if (dangersTop <= dangersBot && transform.position.y < 0) {
			switchTop();
		}
		System.Random rnd = new System.Random ();
		if (dangersBot < dangersTop && transform.position.y > 0 && rnd.Next(4) < 3 ) { //gives a weighting to staying top
			switchBot();
		}
	}

	private void switchTop()
	{
		RaycastHit2D upToGround = (Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(0, 200), 1 << LayerMask.NameToLayer("Ground")));
		if (upToGround) {
			GameObject plat = upToGround.collider.gameObject;
			transform.position = new Vector3(transform.position.x, plat.transform.position.y + 2, 0);
			avoidingMover = null;
			avoidingPopup = null;
		}
	}

	private void switchBot()
	{
		RaycastHit2D downToGround = (Physics2D.Linecast(transform.position, (Vector2)transform.position + new Vector2(0, -200), 1 << LayerMask.NameToLayer("Ground")));
		if (downToGround) {
			GameObject plat = downToGround.collider.gameObject;
			transform.position = new Vector3(transform.position.x, plat.transform.position.y + 2, 0);
			avoidingMover = null;
			avoidingPopup = null;
		}
	}
}