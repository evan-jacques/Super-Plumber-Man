using UnityEngine;
using System.Collections;

public class PointController : MonoBehaviour {
	public bool create = false;
	public GameObject MovingEnemy;
	private Vector3 otherp;
	public float speed;
	// Use this for initialization
	void Start () {
		//if (this.name == "PointA")
		//	otherp = this.transform.parent.FindChild ("PointB").transform.position;
		//else
		//	otherp = this.transform.parent.FindChild ("PointA").transform.position;
	}

	// Update is called once per frame
	/*void Update () {
		if (create) 
		{
			GameObject e = (GameObject)Instantiate(MovingEnemy,this.transform.position,Quaternion.identity);
			if (this.transform.position.x - otherp.x > 0)
				e.rigidbody2D.velocity = new Vector2(-speed,0f);
			else
				e.rigidbody2D.velocity = new Vector2(speed,0f);
			create = false;
		}
	}*/
	/*void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Enemy") 
		{
			if (this.transform.position.x - otherp.x > 0)
				other.rigidbody2D.velocity = new Vector2(-speed,0f);
			else
				other.rigidbody2D.velocity = new Vector2(speed,0f);
		}
	}*/

}
