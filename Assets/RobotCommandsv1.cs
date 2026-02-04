using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCommandsv1 : MonoBehaviour {

    /* This is the basic controller inside of the robot character itself
	 * currently i'm implemting his basic controls, then we will test the 
	 * controls with changable ints, then we'll move on to the proper controls
	 * in the ui
	 */
    public float movespeed = 0.5f;
	public int HowManyBlocks;
    public static bool time;
	public bool driveOn = false;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update()
	{
		if(driveOn == true)
		{
            transform.position += new Vector3(movespeed * Time.deltaTime, 0);
        }

		//move left or right
		if (Input.GetKey(KeyCode.D))
		{
			StartCoroutine(movement());  
        }

		//Attack (melee)


		//Jump


		//Crouch (maybe)
	}

	IEnumerator movement()
	{
		driveOn = true;
		yield return new WaitForSeconds((float)0.5 * HowManyBlocks);
		driveOn = false;
	}
}
