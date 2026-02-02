using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class SpawnBlock : MonoBehaviour {
	public GameObject block;
	Vector3 vector3;
	public Transform canvas;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        vector3 = Input.mousePosition;
		
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			Instantiate(block, vector3, Quaternion.Euler(0, 0, 0),canvas);
		}

	}
}
