using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
public class FunctionDo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
	public bool isstart = false;
	bool islinked = false; // for top
	bool islink = false; // for bottom
	int action = 0; // used to assign action
	bool overui, canmove;
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
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0) && canmove)
        {
            transform.position = Input.mousePosition;
        }

        if (!overui && !isstart && !islinked)
		{
			print("not touching and not start");
			GameObject close = FindClosest();
			if (!close.GetComponent<FunctionDo>().islink)
			{
				float offset = 61f;
				transform.position = close.transform.position - new Vector3(0, offset,0);
				islinked = true;
				close.GetComponent<FunctionDo>().islink = true;
			
			}
		}

    }
	public GameObject FindClosest()
	{

		GameObject[] objects;
		objects = GameObject.FindGameObjectsWithTag("Brick");
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
	
}
