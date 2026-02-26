using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyButtonCode : MonoBehaviour {

    public Button SelfButton;
  
    // Use this for initialization
    void Start()
    {
        SelfButton = GetComponent<Button>();
        SelfButton.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    
    public void TaskOnClick()
    {
        Destroy(transform.parent.parent.gameObject);
    }


}
