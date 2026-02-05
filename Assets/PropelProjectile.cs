using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropelProjectile : MonoBehaviour {
	public float lifetimeobj;
	float speedobj;
	bool leftfaceobj;

	// Use this for initialization
	void Start () {
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
	
	public void Initialize(float speed, float lifetime,bool facingleft)
	{
		lifetimeobj = lifetime;
		speedobj = speed;
		leftfaceobj = facingleft;
		Destroy(gameObject,lifetimeobj);
	}
	IEnumerator LifeTime()
	{
		while (true)
		{ 
			yield return new WaitForSecondsRealtime(.1f);
			lifetimeobj -= .1f;
		}
	}
}
