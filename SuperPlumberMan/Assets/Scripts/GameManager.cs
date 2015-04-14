using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public GameObject spikes;
	public GameObject platform;
	public GameObject PopupEnemy;
	public GameObject MovingEnemy;
	public GameObject MovingPlatform;
	public float speed;
	private Vector3 starttop;
	private Vector3 startbot;
	//private Vector3 startmid;
	private int sectiontop;
	//private int sectionmid;
	GameObject[] sectionstop = {};
	//GameObject[] sectionsmid;
	private int sectionbot;
	GameObject[] sectionsbot;
	public GameObject player;
	private float checkpoint;
	private bool setUpNextArea = false;
	public List<GameObject> enemies;
	public List<GameObject> movingPlatforms;

	// Use this for initialization
	void Start () {
		enemies = new List<GameObject> ();
		movingPlatforms = new List<GameObject> ();
		startbot = new Vector3 (0f, -25f, 0f);
		starttop = new Vector3 (0f, 25f, 0f);
		//startmid = new Vector3 (0f, 0f, 0f);
		Instantiate(player,new Vector3(2f,-24f,0f),Quaternion.identity);
		sectiontop = 1;
		sectionbot = 1;
		//sectionmid = 1;
		//int level = 3;
		//start = createPlatform (level,start,level);
		starttop = createSectiontop (starttop,sectiontop);
		startbot = createSectionbot (startbot,sectionbot);
		//startmid = createSectionmid (startmid, sectionmid);
		float st = GameObject.FindGameObjectWithTag ("sectiontop").transform.GetChild (0).GetChild(0).position.x;
		float end = GameObject.FindGameObjectWithTag ("sectiontop").transform.GetChild (29).GetChild(0).position.x;
		checkpoint = ((end - st) * 0.5f) + st;
		//Debug.Log (checkpoint);
		//Debug.Log (GameObject.FindGameObjectWithTag("sectiontop").transform.childCount);
		
		
		//start = createSection (start);
		
	}
	
	void FixedUpdate () 
	{
		if(GameObject.FindGameObjectWithTag("Player").transform.position.x > checkpoint)
			setUpNextArea = true;
		if (Input.GetKey ("a"))
			setUpNextArea = true;
		if (setUpNextArea) 
		{
			//Debug.Log("HERE");
			sectionstop = GameObject.FindGameObjectsWithTag("sectiontop");
			sectionsbot = GameObject.FindGameObjectsWithTag("sectionbot");
			//sectionsmid = GameObject.FindGameObjectsWithTag("sectionmid");
			sectiontop++;
			sectionbot++;
			//sectionmid++;
			starttop = createSectiontop(starttop,sectiontop);
			startbot = createSectionbot(startbot,sectionbot);
			//startmid = createSectionmid(startmid,sectionmid);
			sectionstop = GameObject.FindGameObjectsWithTag("sectiontop");
			if(sectionstop.Length < 3)
				checkpoint = sectionstop[1].transform.GetChild(0).GetChild(0).position.x + ((sectionstop[1].transform.GetChild (29).GetChild(0).position.x - sectionstop[1].transform.GetChild(0).GetChild(0).position.x)* 0.5f);
			else
				checkpoint = sectionstop[2].transform.GetChild(0).GetChild(0).position.x + ((sectionstop[1].transform.GetChild (29).GetChild(0).position.x - sectionstop[1].transform.GetChild(0).GetChild(0).position.x)* 0.5f);

			//Debug.Log(checkpoint + " " + sectionstop[1]);
			//Debug.Log (sectionstop);
			setUpNextArea = false;
		}
		if(sectionstop.Length > 3)
		{
			foreach(GameObject go in enemies.GetRange(0, enemies.Count)) //use a copy so we don't haev to worry about the changes we make
			{
				if(go.transform.root.gameObject.name == sectionstop[0].name ||
				   go.transform.root.gameObject.name == sectionsbot[0].name)
				{
					enemies.Remove (go);
				}
				else{
					break;
				} //they are added in order, easy to do
			}
			foreach(GameObject go in movingPlatforms.GetRange(0, movingPlatforms.Count))
			{
				if(go.transform.root.gameObject.name == sectionstop[0].name)
				{
					movingPlatforms.Remove (go);
				}
				else{
					break;
				} //they are added in order, easy to do
			}
			
			Destroy(sectionstop[0]);
			Destroy (sectionsbot[0]);
			if(sectionstop[0].name == "section: 1")
				Destroy(sectionstop[0]);
			//Destroy(sectionsmid[0]);
			Debug.Log (checkpoint + " inside");
			
		}
		//Debug.Log (checkpoint + " outside");
	}
	
	Vector3 createSectiontop(Vector3 start, int sectionNumber)
	{
		GameObject section = new GameObject ();
		section.name = "section: " + sectionNumber;
		section.tag = "sectiontop";
		for (int i = 0; i < 30; i++) 
		{
			float height;
			if(start.y < 15f)
			{
				height = Random.Range (10 - start.y, 6);
			}
			else if(start.y > 35f)
			{
				height = Random.Range (-5, 40 - start.y);
			}
			else
			{
				height = Random.Range (-5, 6);
			}
			float width = Random.Range (7, 15);
			int dist = Random.Range (0,100);
			int level;
			if (dist < Mathf.Log(sectionNumber,2) - 1)
				level = 3;
			else if (dist < 5*Mathf.Log (sectionNumber,2) - 5)
				level = 2;
			else
			{
				dist = Random.Range(0,100);
				if (dist > 55)
					level = 1;
				else
					level = 0;
			}
			start = createPlatform (level, start, level, section);
			if (i % 10 == 1)
				createPlatformmid(start,sectionNumber);
			start = start + new Vector3 (width, height, 0f);


		}
		return start;
	}
	Vector3 createSectionbot(Vector3 start, int sectionNumber)
	{
		GameObject section = new GameObject ();
		section.name = "section: " + sectionNumber;
		section.tag = "sectionbot";
		while(start.x < starttop.x)
		{
			float height;
			if(start.y < -35f)
			{
				height = Random.Range (-40 - start.y, 5);
			}
			else if(start.y > -15f)
			{
				height = Random.Range (-4, -10 - start.y);
			}
			else
			{
				height = Random.Range (-5, 6);
			}
			float width = Random.Range (7, 15);
			int dist = Random.Range (0,100);
			int level;
			if (dist < Mathf.Log(sectionNumber,2) - 1)
				level = 3;
			else if (dist < 5*Mathf.Log (sectionNumber,2) - 5)
				level = 2;
			else
			{
				dist = Random.Range(0,100);
				if (dist > 55)
					level = 1;
				else
					level = 0;
			}
			start = createPlatform (level, start, level, section);
			start = start + new Vector3 (width, height, 0f);
		}
		return start;
	}
	
	void createPlatformmid(Vector3 start, int sectionNumber)
	{
		//GameObject section = new GameObject ();
		//section.name = "" + sectionNumber;
		//section.tag = "sectionmid";
		GameObject prent = GameObject.Find ("section: " + sectionNumber);
		float len = 5;
		start = new Vector3 (start.x + 1f + len / 2, 0f, 0f);
		GameObject p = (GameObject)Instantiate (MovingPlatform, start, Quaternion.identity);
		movingPlatforms.Add (p);
		p.layer = LayerMask.NameToLayer("Ground");
		start = start + new Vector3 (len / 2, 0f, 0f);
		p.transform.localScale = new Vector3 (len, 2f, 0f);
		p.transform.parent = prent.transform;
			//start = start + new Vector3(305f,0f,0f);
	}
	
	Vector3 createPlatform(int level, Vector3 start, int name, GameObject section)
	{
		GameObject group = new GameObject ();
		group.name = "" + name + "";
		group.transform.parent = section.transform;
		if (level == 1) 
		{
			int parts = Random.Range (1, 4);
			if (parts % 2 == 1)
				parts = parts - 1;
			float len = Random.Range (10, 40);
			start = start + new Vector3 (len / 2, 0f, 0f);
			GameObject p1 = (GameObject)Instantiate (platform, start, Quaternion.identity);
			p1.layer = LayerMask.NameToLayer("Ground");
			start = start + new Vector3 (len / 2, 0f, 0f);
			p1.transform.localScale = new Vector3 (len, 2f, 0f);
			p1.transform.parent = group.transform;
			if (len > 25f) 
			{
				int chance = Random.Range (0, 100);
				if (chance < len*0.7) 
				{
					//p1.transform.FindChild("PointA").GetComponent<PointController>().create = true;
					Vector3 put = p1.transform.FindChild("PointA").transform.position;
					GameObject menemy = (GameObject)Instantiate(MovingEnemy,new Vector3(put.x + 2f, put.y,put.z),Quaternion.identity);
					menemy.transform.parent = group.transform;
					menemy.rigidbody2D.velocity = new Vector2(speed,0f);
					enemies.Add(menemy);
				}
				else if (chance < len) 
				{
					float place = Random.Range(2f,len-1f);
					GameObject enemy = (GameObject)Instantiate (PopupEnemy, new Vector3(start.x - place,start.y + 2f,start.z), Quaternion.identity);
					enemy.transform.parent = group.transform;
					enemies.Add(enemy);
				}
			}
			int i = 0;
			int last = 0;
			while (i < parts) 
			{
				if (last == 0) 
				{
					last = 1;
					int spk = Random.Range (2, 6);
					start = start + new Vector3 (1f, 0f, 0f);
					for (int j = 0; j < spk; j++) 
					{
						GameObject spike = (GameObject)Instantiate (spikes, start, Quaternion.identity);
						spike.transform.parent = group.transform;
						if (j == spk - 1)
							start = start + new Vector3 (1f, 0f, 0f);
						else
							start = start + new Vector3 (2f, 0f, 0f);
						
					}
				} else 
				{
					last = 0;
					len = Random.Range (10, 35);
					start = start + new Vector3 (len / 2, 0f, 0f);
					GameObject p = (GameObject)Instantiate (platform, start, Quaternion.identity);
					p.layer = LayerMask.NameToLayer("Ground");
					start = start + new Vector3 (len / 2, 0f, 0f);
					p.transform.localScale = new Vector3 (len, 2f, 0f);
					p.transform.parent = group.transform;
					if (len > 20f) 
					{
						int chance = Random.Range (0, 100);
						if (chance < len*0.7) 
						{
							//p.transform.FindChild("PointA").GetComponent<PointController>().create = true;
							Vector3 put = p.transform.FindChild("PointA").transform.position;
							GameObject menemy = (GameObject)Instantiate(MovingEnemy,new Vector3(put.x + 1, put.y,put.z),Quaternion.identity);
							menemy.transform.parent = group.transform;
							menemy.rigidbody2D.velocity = new Vector2(speed,0f);
							enemies.Add(menemy);
						}
						else if (chance < len) 
						{
							float place = Random.Range(2f,len-1f);
							GameObject enemy = (GameObject)Instantiate (PopupEnemy, new Vector3(start.x - place,start.y + 2f,start.z), Quaternion.identity);
							enemy.transform.parent = group.transform;
							enemies.Add(enemy);
						}
					}
				}
				i++;
			}
		} else if (level == 2) 
		{
			int parts = Random.Range (3, 8);
			if (parts % 2 == 1)
				parts = parts - 1;
			float len = Random.Range (10, 30);
			start = start + new Vector3 (len / 2, 0f, 0f);
			GameObject p1 = (GameObject)Instantiate (platform, start, Quaternion.identity);
			p1.layer = LayerMask.NameToLayer("Ground");
			start = start + new Vector3 (len / 2, 0f, 0f);
			p1.transform.localScale = new Vector3 (len, 2f, 0f);
			p1.transform.parent = group.transform;
			if (len > 20f) 
			{
				int chance = Random.Range (0, 100);
				if (chance < len) 
				{
					//p1.transform.FindChild("PointA").GetComponent<PointController>().create = true;
					Vector3 put = p1.transform.FindChild("PointA").transform.position;
					GameObject menemy = (GameObject)Instantiate(MovingEnemy,new Vector3(put.x + 1, put.y,put.z),Quaternion.identity);
					menemy.transform.parent = group.transform;
					menemy.rigidbody2D.velocity = new Vector2(speed,0f);
					enemies.Add(menemy);
				}
				else if (chance < len*1.5) 
				{
					float place = Random.Range(2f,len-1f);
					GameObject enemy = (GameObject)Instantiate (PopupEnemy, new Vector3(start.x - place,start.y + 2f,start.z), Quaternion.identity);
					enemy.transform.parent = group.transform;
					enemies.Add(enemy);
				}
			}
			int i = 0;
			int last = 0;
			while (i < parts) 
			{
				if (last == 0) 
				{
					last = 1;
					int spk = Random.Range (3, 11);
					start = start + new Vector3 (1f, 0f, 0f);
					for (int j = 0; j < spk; j++) 
					{
						GameObject spike = (GameObject)Instantiate (spikes, start, Quaternion.identity);
						spike.transform.parent = group.transform;
						if (j == spk - 1)
							start = start + new Vector3 (1f, 0f, 0f);
						else
							start = start + new Vector3 (2f, 0f, 0f);
						
					}
				} else 
				{
					last = 0;
					len = Random.Range (10, 25);
					start = start + new Vector3 (len / 2, 0f, 0f);
					GameObject p = (GameObject)Instantiate (platform, start, Quaternion.identity);
					p.layer = LayerMask.NameToLayer("Ground");
					start = start + new Vector3 (len / 2, 0f, 0f);
					p.transform.localScale = new Vector3 (len, 2f, 0f);
					p.transform.parent = group.transform;
					if (len > 15f) 
					{
						int chance = Random.Range (0, 100);
						if (chance < len*1.2) 
						{
							//p.transform.FindChild("PointA").GetComponent<PointController>().create = true;
							Vector3 put = p.transform.FindChild("PointA").transform.position;
							GameObject menemy = (GameObject)Instantiate(MovingEnemy,new Vector3(put.x + 1, put.y,put.z),Quaternion.identity);
							menemy.transform.parent = group.transform;
							menemy.rigidbody2D.velocity = new Vector2(speed,0f);
							enemies.Add(menemy);
						}
						else if (chance < len*2) 
						{
							float place = Random.Range(2f,len-1f);
							GameObject enemy = (GameObject)Instantiate (PopupEnemy, new Vector3(start.x - place,start.y + 2f,start.z), Quaternion.identity);
							enemy.transform.parent = group.transform;	
							enemies.Add(enemy);
						}
					}
				}
				i++;
			}
		} else if (level == 3) 
		{
			int parts = Random.Range (5, 12);
			if (parts % 2 == 1)
				parts = parts - 1;
			float len = Random.Range (8, 20);
			start = start + new Vector3 (len / 2, 0f, 0f);
			GameObject p1 = (GameObject)Instantiate (platform, start, Quaternion.identity);
			p1.layer = LayerMask.NameToLayer("Ground");
			start = start + new Vector3 (len / 2, 0f, 0f);
			p1.transform.localScale = new Vector3 (len, 2f, 0f);
			p1.transform.parent = group.transform;
			int i = 0;
			int last = 0;
			while (i < parts) 
			{
				if (last == 0) 
				{
					last = 1;
					int spk = Random.Range (5, 11);
					start = start + new Vector3 (1f, 0f, 0f);
					for (int j = 0; j < spk; j++) {
						GameObject spike = (GameObject)Instantiate (spikes, start, Quaternion.identity);
						spike.transform.parent = group.transform;
						if (j == spk - 1)
							start = start + new Vector3 (1f, 0f, 0f);
						else
							start = start + new Vector3 (2f, 0f, 0f);
						
					}
				} else 
				{
					last = 0;
					len = Random.Range (8, 20);
					start = start + new Vector3 (len / 2, 0f, 0f);
					GameObject p = (GameObject)Instantiate (platform, start, Quaternion.identity);
					p.layer = LayerMask.NameToLayer("Ground");
					start = start + new Vector3 (len / 2, 0f, 0f);
					p.transform.localScale = new Vector3 (len, 2f, 0f);
					p.transform.parent = group.transform;
					float place = 0f;
					float place2 = 0f;
					if (len > 14f) 
					{
						int chance = Random.Range (0, 100);
						if (chance < len*3) 
						{
							//p.transform.FindChild("PointA").GetComponent<PointController>().create = true;
							Vector3 put = p.transform.FindChild("PointA").transform.position;
							GameObject menemy = (GameObject)Instantiate(MovingEnemy,new Vector3(put.x + 1, put.y,put.z),Quaternion.identity);
							menemy.transform.parent = group.transform;
							menemy.rigidbody2D.velocity = new Vector2(speed,0f);
							enemies.Add(menemy);
						}
						else if (chance < len*5) 
						{
							place = Random.Range(2f,len-1f);
							GameObject enemy = (GameObject)Instantiate (PopupEnemy, new Vector3(start.x - place,start.y + 2f,start.z), Quaternion.identity);
							enemy.transform.parent = group.transform;			
							enemies.Add(enemy);
						}
						chance = Random.Range (0, 100);
						if (chance < len*4) 
						{
							if(place > len/2f)
							{
								//p.transform.FindChild("PointA").GetComponent<PointController>().create = true;
								Vector3 put2 = p.transform.FindChild("PointA").transform.position;
								GameObject menemy2 = (GameObject)Instantiate(MovingEnemy,new Vector3(put2.x + 1, put2.y,put2.z),Quaternion.identity);
								menemy2.transform.parent = group.transform;
								menemy2.rigidbody2D.velocity = new Vector2(speed,0f);
								enemies.Add(menemy2);
							}
							else
							{
								//p.transform.FindChild("PointB").GetComponent<PointController>().create = true;
								Vector3 put2 = p.transform.FindChild("PointA").transform.position;
								GameObject menemy2 = (GameObject)Instantiate(MovingEnemy,new Vector3(put2.x + 1, put2.y,put2.z),Quaternion.identity);
								menemy2.transform.parent = group.transform;
								menemy2.rigidbody2D.velocity = new Vector2(speed,0f);
								enemies.Add(menemy2);
							}
							
							
						}
						else if (chance < len*4) 
						{
							place2 = Random.Range(len/2f,len-1f);
							if(place == 0)
							{
								GameObject enemy2 = (GameObject)Instantiate (PopupEnemy, new Vector3(start.x - place2,start.y + 2f,start.z), Quaternion.identity);
								enemy2.transform.parent = group.transform;
							}
						}
					}
					else if (len > 9f) 
					{
						int chance = Random.Range (0, 100);
						if (chance < len*3) 
						{
							//p.transform.FindChild("PointA").GetComponent<PointController>().create = true;
							Vector3 put = p.transform.FindChild("PointA").transform.position;
							GameObject menemy = (GameObject)Instantiate(MovingEnemy,new Vector3(put.x + 1, put.y,put.z),Quaternion.identity);
							menemy.transform.parent = group.transform;
							menemy.rigidbody2D.velocity = new Vector2(speed,0f);
							enemies.Add(menemy);
						}
						else if (chance < len*7) 
						{
							place = Random.Range(2f,len-1f);
							GameObject enemy = (GameObject)Instantiate (PopupEnemy, new Vector3(start.x - place,start.y + 2f,start.z), Quaternion.identity);
							enemy.transform.parent = group.transform;
							enemies.Add(enemy);
						}
					}
				}
				i++;
			}
		} else if (level == 0) 
		{
			float len = Random.Range (10, 25);
			start = start + new Vector3 (len / 2, 0f, 0f);
			GameObject p1 = (GameObject)Instantiate (platform, start, Quaternion.identity);
			p1.layer = LayerMask.NameToLayer("Ground");
			start = start + new Vector3 (len / 2, 0f, 0f);
			p1.transform.localScale = new Vector3 (len, 2f, 0f);
			p1.transform.parent = group.transform;
		}
		return start;
	}
}
