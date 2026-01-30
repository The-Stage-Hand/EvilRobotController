using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
public class FunctionDo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	public bool isstart = false;

    public bool islinked = false; // for top
	public bool islink = false; // for bottom
	int action = 0; // used to assign action
	bool overui, canmove;
    public float offset = 61f;
    public void OnPointerEnter(PointerEventData eventData)
    {
        overui = true;
	}
    public void OnPointerExit(PointerEventData eventData)
    {

        if (!Input.GetMouseButton(0)) overui = false;
        

    }
	public void OnPointerDown(PointerEventData eventdata)
	{
        canmove = true;
    }
    public void OnPointerUp(PointerEventData eventdata)
    {
        canmove=false;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButton(0) && canmove)
        {
            transform.position = Input.mousePosition;
        }

        if (!isstart && !islinked)
		{
			print("not touching and not start");
            GameObject close = FindClosest();
            if (!close.GetComponent<FunctionDo>().islink)
			{
				
                transform.position = close.transform.position - new Vector3(0, offset, 0);
                islinked = true;
				close.GetComponent<FunctionDo>().islink = true;
			}
			if (!islinked)
            {
                print("notlinked move");
				GameObject[] objects = FindClosestarray();
                GameObject closest = null;
                float distance = Mathf.Infinity;
                Vector3 position = transform.position;
                foreach (GameObject obj in objects)
                {
                    Vector3 difference = obj.transform.position - position;
                    float currentdist = difference.sqrMagnitude;
                    if (currentdist < distance)
                    {
                        if (!obj.GetComponent<FunctionDo>().islink)
                        closest = obj;
                        distance = currentdist;
                    }
                }
                islinked = true;
                transform.position = new Vector3(close.transform.position.x, close.transform.position.y - offset, close.transform.position.z);

            }
		}
		else if (islinked)
		{
			print("islinked is moving along with link");
            GameObject close = FindClosest();
            float offset = 61f;
            transform.position = new Vector3(close.transform.position.x, close.transform.position.y - offset, close.transform.position.z);
        }

    }
	public GameObject FindClosest()
	{

		GameObject[] objects;
		objects = GameObject.FindGameObjectsWithTag("Brick");
        objects = objects.Skip(1).ToArray();
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

		foreach (GameObject obj in objects)
		{
			Vector3 difference = obj.transform.position - position;
			float currentdist = difference.sqrMagnitude;
			if (currentdist < distance)
			{
				closest = obj;
				distance = currentdist;
			}
		}
		 return closest;

    }
    public GameObject[] FindClosestarray()
    {

        GameObject[] objects;
        objects = GameObject.FindGameObjectsWithTag("Brick");
        objects = objects.Skip(1).ToArray();
        return objects;

    }

}
