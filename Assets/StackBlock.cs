using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.EventSystems;

public class StackBlock : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
	
	public float offset = 1.1f;
	public static StackBlock root;
	public  StackBlock above;
	public  StackBlock below;

	private bool dragging;
	private Vector3 dragoffset;
	public float sensitivity = 1f;
	/// <summary>
	public StackBlock publictarget;





	

	/// </summary>


	// Use this for initialization
	private void Awake()
	{
		print("stackawake");
		if (root == null)
		{
			print("setting root cause no root :(");
			root = this;
			above = null;
		}
	}
	
	// Update is called once per frame
	private void Update () 
	{
		if (above != null && !dragging)
		{
			transform.position = above.transform.position - new Vector3(0, offset, 0);
		}
		
	}

	public void OnPointerDown(PointerEventData eventdata)
    {
		print("mousedownonme");
		dragging = true;
		Detach();
		Vector3 mouseworld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouseworld.z = transform.position.z;
		dragoffset = transform.position - mouseworld;	
	}
	public void OnDrag(PointerEventData eventdata)
    {
		if (!dragging) { return; }
        Vector3 mouseworld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseworld.z = transform.position.z;
		transform.position = mouseworld * sensitivity + dragoffset; 
    }
	public void OnPointerUp(PointerEventData eventdata)
	{
		print("pointerup");
		dragging = false;
		StackBlock target = FindSnapTarget();
		if (target != null)
			TryStack(target);
	}
	public void TryStack(StackBlock target)
	{
	    publictarget = target;
		print("stacking at " + target);
		if (target == null) return;
		if (target == this)
		{
			print("target is myself not TryStack");
			return;
		}
		if (target.above == null && target != root) return;

		Detach();
		above = target;
		below = target.below;
		if (target.below != null) target.below.above = this;
		target.below = this;
		print("stacked");
	}
	public void Detach()
	{
		print("deataching");
		if (above !=null) above.below = below;
		if (below != null) below.above = above;
		above = null;
		below = null;
		//if (root ==  this) root = null;
	}
	StackBlock FindSnapTarget()
	{
		print("finding snap target");
		StackBlock[] blocks = FindObjectsOfType<StackBlock>();
		float mindist = 1.2f;
		StackBlock best = null;
		foreach (StackBlock b in blocks)
		{
			if (b == this)
			{
				print("found me");
				continue;
			}
			float d = Vector3.Distance(transform.position, b.transform.position);
			if (d < mindist)
			{
				mindist = d;
				best = b;
			}
		}
		print(blocks.Length + "\t" + blocks);
		print("best: " + best);
		return best;
	}
}
