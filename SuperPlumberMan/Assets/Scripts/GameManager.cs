using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject spikes;
	public GameObject platform;
	public GameObject PopupEnemy;
	public GameObject MovingEnemy;
	public float speed;
	private Vector3 starttop;
	private Vector3 startbot;
	private int sectiontop;
	GameObject[] sectionstop;
	private int sectionbot;
	GameObject[] sectionsbot;

	private bool setUpNextArea = false;

	// Use this for initialization
	void Start () {
		startbot = new Vector3 (0f, 0f, 0f);
		starttop = new Vector3 (0f, 25f, 0f);
		sectiontop = 1;
		sectionbot = 1;
		//int level = 3;
		//start = createPlatform (level,start,level);
		starttop = createSectiontop (starttop,sectiontop);
		startbot = createSectionbot (startbot,sectionbot);

		
		//start = createSection (start);

	}

	void Update () 
	{
		if (setUpNextArea) 
		{
			sectionstop = GameObject.FindGameObjectsWithTag("sectiontop");
			sectionsbot = GameObject.FindGameObjectsWithTag("sectionbot");

			if(sectionstop.Length > 2)
			{
				Destroy(sectionstop[0]);
				Destroy (sectionsbot[0]);
			}
			sectiontop++;
			sectionbot++;
			starttop = createSectiontop(starttop,sectiontop);
			startbot = createSectionbot(startbot,sectionbot);

			
		}
	}

	Vector3 createSectiontop(Vector3 start, int sectionNumber)
	{
		GameObject section = new GameObject ();
		section.name = "" + sectionNumber;
		section.tag = "sectiontop";
		for (int i = 0; i < 30; i++) 
		{
			float height;
			if(start.y < 20f)
			{
				height = Random.Range (15 - start.y, 7);
			}
			else if(start.y > 30f)
			{
				height = Random.Range (-5, 35 - start.y);
			}
			else
			{
				height = Random.Range (-5, 6);
			}
			float width = Random.Range (5, 10);
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
	Vector3 createSectionbot(Vector3 start, int sectionNumber)
	{
		GameObject section = new GameObject ();
		section.name = "" + sectionNumber;
		section.tag = "sectionbot";
		while(start.x < starttop.x)
		{
			float height;
			if(start.y < -5f)
			{
				height = Random.Range (-10 - start.y, 7);
			}
			else if(start.y > 5f)
			{
				height = Random.Range (-5, 10 - start.y);
			}
			else
			{
				height = Random.Range (-5, 6);
			}
			float width = Random.Range (3, 6);
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
			float len = Random.Range (4, 13);
			start = start + new Vector3 (len / 2, 0f, 0f);
			GameObject p1 = (GameObject)Instantiate (platform, start, Quaternion.identity);
			p1.layer = LayerMask.NameToLayer("Ground");
			start = start + new Vector3 (len / 2, 0f, 0f);
			p1.transform.localScale = new Vector3 (len, 1f, 0f);
			p1.transform.parent = group.transform;
			if (len > 9f) 
			{
				int chance = Random.Range (0, 100);
				if (chance < len*2) 
				{
					//p1.transform.FindChild("PointA").GetComponent<PointController>().create = true;
					Vector3 put = p1.transform.FindChild("PointA").transform.position;
					GameObject menemy = (GameObject)Instantiate(MovingEnemy,new Vector3(put.x + 1, put.y,put.z),Quaternion.identity);
					menemy.transform.parent = group.transform;
					menemy.rigidbody2D.velocity = new Vector2(speed,0f);
				}
				else if (chance < len*4) 
				{
					float place = Random.Range(2f,len-1f);
					GameObject enemy = (GameObject)Instantiate (PopupEnemy, new Vector3(start.x - place,start.y + 1f,start.z), Quaternion.identity);
					enemy.transform.parent = group.transform;
				}
			}
			int i = 0;
			int last = 0;
			while (i < parts) 
			{
				if (last == 0) 
				{
					last = 1;
					int spk = Random.Range (1, 6);
					start = start + new Vector3 (0.5f, 0f, 0f);
					for (int j = 0; j < spk; j++) 
					{
						GameObject spike = (GameObject)Instantiate (spikes, start, Quaternion.identity);
						spike.layer = LayerMask.NameToLayer("Ground");
						spike.transform.parent = group.transform;
						if (j == spk - 1)
							start = start + new Vector3 (0.5f, 0f, 0f);
						else
							start = start + new Vector3 (1f, 0f, 0f);

					}
				} else 
				{
					last = 0;
					len = Random.Range (4, 13);
					start = start + new Vector3 (len / 2, 0f, 0f);
					GameObject p = (GameObject)Instantiate (platform, start, Quaternion.identity);
					p.layer = LayerMask.NameToLayer("Ground");
					start = start + new Vector3 (len / 2, 0f, 0f);
					p.transform.localScale = new Vector3 (len, 1f, 0f);
					p.transform.parent = group.transform;
					if (len > 8f) 
					{
						int chance = Random.Range (0, 100);
						if (chance < len*3) 
						{
							//p.transform.FindChild("PointA").GetComponent<PointController>().create = true;
							Vector3 put = p.transform.FindChild("PointA").transform.position;
							GameObject menemy = (GameObject)Instantiate(MovingEnemy,new Vector3(put.x + 1, put.y,put.z),Quaternion.identity);
							menemy.transform.parent = group.transform;
							menemy.rigidbody2D.velocity = new Vector2(speed,0f);
						}
						else if (chance < len*5) 
						{
							float place = Random.Range(2f,len-1f);
							GameObject enemy = (GameObject)Instantiate (PopupEnemy, new Vector3(start.x - place,start.y + 1f,start.z), Quaternion.identity);
							enemy.transform.parent = group.transform;
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
			float len = Random.Range (4, 9);
			start = start + new Vector3 (len / 2, 0f, 0f);
			GameObject p1 = (GameObject)Instantiate (platform, start, Quaternion.identity);
			p1.layer = LayerMask.NameToLayer("Ground");
			start = start + new Vector3 (len / 2, 0f, 0f);
			p1.transform.localScale = new Vector3 (len, 1f, 0f);
			p1.transform.parent = group.transform;
			if (len > 9f) 
			{
				int chance = Random.Range (0, 100);
				if (chance < len*4) 
				{
					//p1.transform.FindChild("PointA").GetComponent<PointController>().create = true;
					Vector3 put = p1.transform.FindChild("PointA").transform.position;
					GameObject menemy = (GameObject)Instantiate(MovingEnemy,new Vector3(put.x + 1, put.y,put.z),Quaternion.identity);
					menemy.transform.parent = group.transform;
					menemy.rigidbody2D.velocity = new Vector2(speed,0f);

				}
				else if (chance < len*6) 
				{
					float place = Random.Range(2f,len-1f);
					GameObject enemy = (GameObject)Instantiate (PopupEnemy, new Vector3(start.x - place,start.y + 1f,start.z), Quaternion.identity);
					enemy.transform.parent = group.transform;
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
					start = start + new Vector3 (0.5f, 0f, 0f);
					for (int j = 0; j < spk; j++) 
					{
						GameObject spike = (GameObject)Instantiate (spikes, start, Quaternion.identity);
						spike.layer = LayerMask.NameToLayer("Ground");
						spike.transform.parent = group.transform;
						if (j == spk - 1)
							start = start + new Vector3 (0.5f, 0f, 0f);
						else
							start = start + new Vector3 (1f, 0f, 0f);
						
					}
				} else 
				{
					last = 0;
					len = Random.Range (4, 11);
					start = start + new Vector3 (len / 2, 0f, 0f);
					GameObject p = (GameObject)Instantiate (platform, start, Quaternion.identity);
					p.layer = LayerMask.NameToLayer("Ground");
					start = start + new Vector3 (len / 2, 0f, 0f);
					p.transform.localScale = new Vector3 (len, 1f, 0f);
					p.transform.parent = group.transform;
					if (len > 4f) 
					{
						int chance = Random.Range (0, 100);
						if (chance < len*7) 
						{
							//p.transform.FindChild("PointA").GetComponent<PointController>().create = true;
							Vector3 put = p.transform.FindChild("PointA").transform.position;
							GameObject menemy = (GameObject)Instantiate(MovingEnemy,new Vector3(put.x + 1, put.y,put.z),Quaternion.identity);
							menemy.transform.parent = group.transform;
							menemy.rigidbody2D.velocity = new Vector2(speed,0f);
						}
						else if (chance < len*11) 
						{
							float place = Random.Range(2f,len-1f);
							GameObject enemy = (GameObject)Instantiate (PopupEnemy, new Vector3(start.x - place,start.y + 1f,start.z), Quaternion.identity);
							enemy.transform.parent = group.transform;						}
					}
				}
				i++;
			}
		} else if (level == 3) 
		{
			int parts = Random.Range (5, 12);
			if (parts % 2 == 1)
				parts = parts - 1;
			float len = Random.Range (3, 7);
			start = start + new Vector3 (len / 2, 0f, 0f);
			GameObject p1 = (GameObject)Instantiate (platform, start, Quaternion.identity);
			p1.layer = LayerMask.NameToLayer("Ground");
			start = start + new Vector3 (len / 2, 0f, 0f);
			p1.transform.localScale = new Vector3 (len, 1f, 0f);
			p1.transform.parent = group.transform;
			int i = 0;
			int last = 0;
			while (i < parts) 
			{
				if (last == 0) 
				{
					last = 1;
					int spk = Random.Range (5, 11);
					start = start + new Vector3 (0.5f, 0f, 0f);
					for (int j = 0; j < spk; j++) {
						GameObject spike = (GameObject)Instantiate (spikes, start, Quaternion.identity);
						spike.layer = LayerMask.NameToLayer("Ground");
						spike.transform.parent = group.transform;
						if (j == spk - 1)
							start = start + new Vector3 (0.5f, 0f, 0f);
						else
							start = start + new Vector3 (1f, 0f, 0f);
						
					}
				} else 
				{
					last = 0;
					len = Random.Range (2, 9);
					start = start + new Vector3 (len / 2, 0f, 0f);
					GameObject p = (GameObject)Instantiate (platform, start, Quaternion.identity);
					p.layer = LayerMask.NameToLayer("Ground");
					start = start + new Vector3 (len / 2, 0f, 0f);
					p.transform.localScale = new Vector3 (len, 1f, 0f);
					p.transform.parent = group.transform;
					float place = 0f;
					float place2 = 0f;
					if (len > 5f) 
					{
						int chance = Random.Range (0, 100);
						if (chance < len*6) 
						{
							//p.transform.FindChild("PointA").GetComponent<PointController>().create = true;
							Vector3 put = p.transform.FindChild("PointA").transform.position;
							GameObject menemy = (GameObject)Instantiate(MovingEnemy,new Vector3(put.x + 1, put.y,put.z),Quaternion.identity);
							menemy.transform.parent = group.transform;
							menemy.rigidbody2D.velocity = new Vector2(speed,0f);
						}
						else if (chance < len*15) 
						{
							place = Random.Range(2f,len-1f);
							GameObject enemy = (GameObject)Instantiate (PopupEnemy, new Vector3(start.x - place,start.y + 1f,start.z), Quaternion.identity);
							enemy.transform.parent = group.transform;						}
						chance = Random.Range (0, 100);
						if (chance < len*5) 
						{
							if(place > len/2f)
							{
								//p.transform.FindChild("PointA").GetComponent<PointController>().create = true;
								Vector3 put2 = p.transform.FindChild("PointA").transform.position;
								GameObject menemy2 = (GameObject)Instantiate(MovingEnemy,new Vector3(put2.x + 1, put2.y,put2.z),Quaternion.identity);
								menemy2.transform.parent = group.transform;
								menemy2.rigidbody2D.velocity = new Vector2(speed,0f);
							}
							else
							{
								//p.transform.FindChild("PointB").GetComponent<PointController>().create = true;
								Vector3 put2 = p.transform.FindChild("PointA").transform.position;
								GameObject menemy2 = (GameObject)Instantiate(MovingEnemy,new Vector3(put2.x + 1, put2.y,put2.z),Quaternion.identity);
								menemy2.transform.parent = group.transform;
								menemy2.rigidbody2D.velocity = new Vector2(speed,0f);
							}


						}
						else if (chance < len*7) 
						{
							place2 = Random.Range(len/2f,len-1f);
							if(place == 0)
							{
								GameObject enemy2 = (GameObject)Instantiate (PopupEnemy, new Vector3(start.x - place2,start.y + 1f,start.z), Quaternion.identity);
								enemy2.transform.parent = group.transform;
							}
						}
					}
					else if (len > 2f) 
					{
						int chance = Random.Range (0, 100);
						if (chance < len*8) 
						{
							//p.transform.FindChild("PointA").GetComponent<PointController>().create = true;
							Vector3 put = p.transform.FindChild("PointA").transform.position;
							GameObject menemy = (GameObject)Instantiate(MovingEnemy,new Vector3(put.x + 1, put.y,put.z),Quaternion.identity);
							menemy.transform.parent = group.transform;
							menemy.rigidbody2D.velocity = new Vector2(speed,0f);

						}
						else if (chance < len*15) 
						{
							place = Random.Range(2f,len-1f);
							GameObject enemy = (GameObject)Instantiate (PopupEnemy, new Vector3(start.x - place,start.y + 1f,start.z), Quaternion.identity);
							enemy.transform.parent = group.transform;						}
					}
				}
				i++;
			}
		} else if (level == 0) 
		{
			float len = Random.Range (4, 13);
			start = start + new Vector3 (len / 2, 0f, 0f);
			GameObject p1 = (GameObject)Instantiate (platform, start, Quaternion.identity);
			p1.layer = LayerMask.NameToLayer("Ground");
			start = start + new Vector3 (len / 2, 0f, 0f);
			p1.transform.localScale = new Vector3 (len, 1f, 0f);
			p1.transform.parent = group.transform;
		}
		return start;
	}
}
