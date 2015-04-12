using UnityEngine;
using System.Collections;

public class MovingEnemyController : MonoBehaviour {
	private Vector3 otherp;
	// Use this for initialization
	void Start () {
		//this.rigidbody2D.velocity = new Vector2 (1f, 0f);
	}
//	void OnTriggerEnter2D(Collider2D other)
//	{
//		if (other.tag == "Enemy") 
//		{
//			this.rigidbody2D.velocity = new Vector2(-this.rigidbody2D.velocity.x,this.rigidbody2D.velocity.y);
//		}
//		if (other.tag == "Player") 
//		{
//			Destroy(other.gameObject);
//		}
//	}
}
