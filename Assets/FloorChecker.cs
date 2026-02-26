using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorChecker : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FloorBlock")
        {
            Debug.Log("OnFloor");
            RobotCommandsv1.OnFloor = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "FloorBlock")
        {
            Debug.Log("OffFloor");
            RobotCommandsv1.jump = 0;
            RobotCommandsv1.OnFloor = false;
        }
    }

}
