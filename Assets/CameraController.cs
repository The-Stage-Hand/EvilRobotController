using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public static bool Automantic;
	public int moveSpeed = 2;
	// Use this for initialization
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		
		if(Automantic == true)
		{
			if(Input.GetKeyDown(KeyCode.A))
			{
                transform.position -=
                 new Vector3(moveSpeed * Time.deltaTime, 0);
            }

			if(Input.GetKeyDown(KeyCode.D))
			{
                transform.position +=
                new Vector3(moveSpeed * Time.deltaTime, 0);
            }

		}
		else
		{


		}
	}
}
