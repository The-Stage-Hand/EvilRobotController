using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropelProjectile : MonoBehaviour {
	public float lifetimeobj = 4f;
	float speedobj=0f;
	bool leftfaceobj=false;

	// Use this for initialization
	void Start () {
		print("bullet is alive");
		Debug.Log("bullet is here" + gameObject.name,gameObject);
		if (lifetimeobj == 0)
		{
			print("setting to 10f");
			lifetimeobj = 10f;
			speedobj = 0.1f;
		}
        StartCoroutine(LifeTime());
    }
	
	// Update is called once per frame
	void Update () {
		
        if (lifetimeobj <= 0)
		{
			Destroy(gameObject);
		}
		if (!leftfaceobj)
			transform.position += transform.right * speedobj;
		else
		{
	
			transform.position -= transform.right * speedobj;
			gameObject.GetComponent<SpriteRenderer>().flipX = true;
		}
	}
	
	public void Initialize(float lifetime, float speed, bool facingleft)
	{
		print("bullet has been initialized and set");
		lifetimeobj = lifetime;
		speedobj = speed;
		leftfaceobj = facingleft;
		
	}
	IEnumerator LifeTime()
	{
		while (lifetimeobj >= 0)
		{ 
			yield return new WaitForSecondsRealtime(.1f);
			lifetimeobj -= .1f;
		}
		if (lifetimeobj <= 0)
		{
			Destroy(gameObject);
		}
	}
}
