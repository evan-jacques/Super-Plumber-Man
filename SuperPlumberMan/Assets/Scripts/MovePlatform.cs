using UnityEngine;
using System.Collections;

public class MovePlatform : MonoBehaviour {
	public float speed;
	public int direction;
	
	void Start() 
	{
		direction = 1;
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.DrawRay (this.transform.position, new Vector3 (0f,7f, 0f), Color.blue);
		//Debug.DrawRay (this.transform.position, new Vector3 (0f,-7f, 0f), Color.red);
		//Ray shootrayup = new Ray (this.transform.position, Vector3.up);
		//Ray shootraydown = new Ray (this.transform.position, Vector3.down);
		
		
		Ray shootrayup = new Ray (this.transform.position, new Vector3 (0f,7f, 0f));
		Ray shootrayupleft = new Ray (this.transform.position, new Vector3 (-2.5f,7f, 0f));
		Ray shootrayupright = new Ray (this.transform.position, new Vector3 (2.5f,7f, 0f));


		Ray shootraydown = new Ray (this.transform.position, new Vector3 (0f,-7f, 0f));
		Ray shootraydownleft = new Ray (this.transform.position, new Vector3 (2.5f,-7f, 0f));
		Ray shootraydownright = new Ray (this.transform.position, new Vector3 (-2.5f,-7f, 0f));

		//Debug.Log (shootray);
		Debug.DrawRay (this.transform.position, new Vector3 (0f,7f, 0f), Color.black);
		Debug.DrawRay (this.transform.position, new Vector3 (0f,-7f, 0f), Color.blue);
		Debug.DrawRay(this.transform.position, new Vector3 (-2.5f,7f, 0f),Color.red);
		Debug.DrawRay(this.transform.position, new Vector3 (2.5f,7f, 0f),Color.red);
		Debug.DrawRay(this.transform.position, new Vector3 (-2.5f,-7f, 0f),Color.cyan);
		Debug.DrawRay(this.transform.position, new Vector3 (2.5f,-7f, 0f),Color.cyan);

		
		
		RaycastHit hit;
		int mask = LayerMask.GetMask ("Ground");
		Debug.Log (mask);
		float range = 7f;
		Debug.Log (mask);
		//int mask = LayerMask.GetMask ("Ground");
		//RaycastHit2D hitup = Physics2D.Raycast (new Vector2 (this.transform.position.x, this.transform.position.y), new Vector2 (0f, 1f), 7f,mask);
		//RaycastHit2D hitdown = Physics2D.Raycast (new Vector2 (this.transform.position.x, this.transform.position.y), new Vector2 (0f, 1f), 7f,mask);
		
		//Debug.Log (hitup.transform.tag);
		if(Physics.Raycast(shootrayup,out hit, 7f,mask) || Physics.Raycast(shootrayupleft,out hit, 7.5f,mask) || Physics.Raycast(shootrayupright,out hit, 7.5f,mask))
		{
			Debug.Log("Here");
			direction = -1;
		}
		else if(Physics.Raycast(shootraydown,out hit, 7f,mask) || Physics.Raycast(shootraydownleft,out hit, 7.5f,mask) || Physics.Raycast(shootraydownright,out hit, 7.5f,mask))
		{
			Debug.Log("Here1");
			
			direction = 1;
		}
		if (this.transform.position.y > 19f || this .transform.position.y < -19f)
			direction = direction * -1;
		this.transform.position = Vector3.MoveTowards(this.transform.position,new Vector3(this.transform.position.x,20f*direction,0f),speed*Time.deltaTime);
		
		
	}
}
