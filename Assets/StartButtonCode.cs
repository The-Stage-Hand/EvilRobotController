using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonCode : MonoBehaviour { 
	public Button SelfButton;
    public StackBlock[] Stackblocks = new StackBlock[100];
    // Use this for initialization
    void Start () {
		SelfButton = GetComponent<Button>();
        SelfButton.onClick.AddListener(TaskOnClick);
    }
	
	// Update is called once per frame
	void Update () {
		Stackblocks = FindObjectsOfType<StackBlock>();

	}
	public void TaskOnClick()
	{
		foreach (var block in Stackblocks)
		{
			block.StartCode();
		}
	}
}
