using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour {
	
	// Use this for initialization
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player") 
		{
			Destroy(other.gameObject);
		}
	}
	
}
