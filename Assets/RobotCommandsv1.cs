using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotCommandsv1 : MonoBehaviour {

    /* This is the basic controller inside of the robot character itself
	 * currently i'm implemting his basic controls, then we will test the 
	 * controls with changable ints, then we'll move on to the proper controls
	 * in the ui
	 */
    public Rigidbody2D rb2d;
	public GameObject FloorChecker;
    public float jumpPower = 300f;
    public float movespeed = 0.5f;
	public int HowManyBlocks;
    public static bool time;
	public static bool driveOn = false;
	public GameObject WinText;

	public static int jump;
	public static bool OnFloor;
	//direction controls the direction
	//0.02 is right and -0.02 is left
	public static float direction = 0.02f;
	
	// Use this for initialization
	void Start () {
		rb2d = gameObject.GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		if(driveOn == true)
		{
            transform.position += new Vector3(movespeed * direction, 0);
        }

		//PLEASE NOTE: Left and right movemnet is turned off since we have niki's alternative
		/*
		//move left or right

		//right
		if (Input.GetKeyDown(KeyCode.D))
		{
			direction = 0.02f;
			StartCoroutine(movement(HowManyBlocks));  
        }

		//left
        if (Input.GetKeyDown(KeyCode.A))
        {
			direction = -0.02f;
            StartCoroutine(movement(HowManyBlocks));
        }
		*/

        //Attack (melee)
        if (Input.GetKeyDown(KeyCode.S))
		{
			/*Attacking will Play an animation and during the animation will temporarily activate
			* a boxcollider inflicting damage. the attack will be tied into the animation itself, which
			* can have code in the animation for which direction to attack in. I'll finish this a bit
			* Later since i still need to make a basic animation */

			//FIXME:activate anim bool for attacking animation
			Debug.Log("Attacking!");
		}

		//Jump
		if (jump == 1)
		{
            if (OnFloor == true)
			{
                StartCoroutine(Jumping(jumpPower,rb2d));
                
            }
			else if (OnFloor == false)
			{
				
			}
			
        }

		//Crouch (maybe)
	}


	/*
    public static IEnumerator Movement(int HowManyBlocks)
	{
		Debug.Log("Starting movemnet");
		driveOn = true;
		yield return new WaitForSeconds((float)0.399 * HowManyBlocks); 
		driveOn = false;
		//reset directions
        direction = 0f;
        Debug.Log("ending movemnet");
    }*/

    public static IEnumerator Jumping(float jumpPower, Rigidbody2D rb2d)
    {
		Debug.Log("Jumping");
        yield return new WaitForSeconds(0.125f);
        rb2d.AddForce(new Vector3(0, 20 * jumpPower));
        Debug.Log("Jump complete");
    }

    void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "ReachGoal")
		{
			//FIXME:add more stuff to this
			Debug.Log("Level beat");
			WinText.SetActive(true);
		}
	}
}
