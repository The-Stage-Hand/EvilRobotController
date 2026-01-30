using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotCommandsv1 : MonoBehaviour {

    /* This is the basic controller inside of the robot character itself
	 * currently i'm implemting his basic controls, then we will test the 
	 * controls with changable ints, then we'll move on to the proper controls
	 * in the ui
	 */
    public float movespeed = 1f;
    public static bool time;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update()
	{ 
		//move left or right
		if (Input.GetKeyDown(KeyCode.D))
		{
            transform.position += new Vector3(movespeed, 0 * Time.deltaTime);
        }

		//Attack


		//Jump


		//Crouch (maybe)
	}
}
