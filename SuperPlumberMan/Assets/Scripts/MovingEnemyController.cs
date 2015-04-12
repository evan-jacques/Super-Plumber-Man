using UnityEngine;
using System.Collections;

public class MovingEnemyController : MonoBehaviour {
	private Vector3 otherp;
	private GameObject p1;
	private GameObject p2;
	public float speed;
	// Use this for initialization
	void Start () {
		//p1 = this.transform.parent.FindChild ("PointA").gameObject;
		//p2 = this.transform.parent.FindChild ("PointB").gameObject;
		//this.rigidbody2D.velocity = new Vector2 (1f, 0f);
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Enemy") 
		{
			this.rigidbody2D.velocity = new Vector2(-this.rigidbody2D.velocity.x,this.rigidbody2D.velocity.y);
		}
		if (other.name == "PointA") 
		{
			this.rigidbody2D.velocity = new Vector2(speed,0f);
		}
		if (other.name == "PointB") 
		{
			this.rigidbody2D.velocity = new Vector2(-speed,0f);
		}
//		if (other.tag == "Player") 
//		{
//			Destroy(other.gameObject);
//		}
	}
}
