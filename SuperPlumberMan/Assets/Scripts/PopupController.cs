using UnityEngine;
using System.Collections;

public class PopupController : MonoBehaviour {
	public int move;
	public float speed;
	private float time;
	// Use this for initialization
	void Start () {
		move = -1;
		time = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(move > 0)
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.parent.position.x,this.transform.parent.position.y + 1, this.transform.parent.position.z), speed*Time.deltaTime);
		}
		else
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(this.transform.parent.position.x,this.transform.parent.position.y, this.transform.parent.position.z), speed*Time.deltaTime);
		}
		if(time > 3f){
			time = 0f;
			move = move*-1;
		}
		time = time + Time.deltaTime;
	}
}
