using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StackBlock : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
	
	public float offset = -200f;
	public static StackBlock root;
	public  StackBlock above;
	public  StackBlock below;
	private GameObject block;
	public Material materialred,materialgreen;
	private bool dragging;
	private Vector3 dragoffset;
	public float sensitivity = 1f;
	Dropdown Options;
	/// <summary>
	public StackBlock publictarget;
	public GameObject Player;
	public float TravelSpeed = 2f;
	public int action = 0;
	public bool nextaction = true;
	public Slider SliderObj;
	public Text valuetext;

	StackBlock current;
    private static bool IsExecuting = false;
	private static Coroutine Runningroutine;
	/// </summary>


	// Use this for initialization
	private void Awake()
	{
		block = gameObject.transform.GetChild(0).gameObject;
		block.GetComponent<Renderer>().material = materialred;
		print("stackawake");
		if (root == null)
		{
			print("setting root cause no root :(");
			root = this;
			above = null;
		}
		Player = GameObject.FindGameObjectWithTag("Player").gameObject;
		Options = gameObject.GetComponentInChildren<Dropdown>();
		SliderObj = gameObject.GetComponentInChildren<Slider>();
		valuetext = gameObject.transform.Find("ValueText").GetComponent<Text>();
	}
	
	// Update is called once per frame
	private void Update () 
	{
		
		if (above != null && !dragging)
		{
			transform.position = above.transform.position - new Vector3(0, offset, 0);
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (!IsExecuting && root != null)
				Runningroutine = StartCoroutine(ExecuteRoutine());
		}
		if (Player == null)
		{
            Player = GameObject.FindGameObjectWithTag("Player").gameObject;
        }
		action = Options.value;
		valuetext.text = ((int)SliderObj.value).ToString();

    }

	public void OnPointerDown(PointerEventData eventdata)
    {
		print("mousedownonme");
		dragging = true;
		if (above != null || below != null) Detach();
		Vector3 mouseworld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouseworld.z = transform.position.z;
		dragoffset = transform.position - mouseworld;	
	}
	public void OnDrag(PointerEventData eventdata)
    {
		if (!dragging) { return; }
        Vector3 mouseworld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseworld.z = transform.position.z;
		transform.position = mouseworld* sensitivity + dragoffset; 
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
		if (target == null || target == this) return;
		Detach();
		above = target;
		below = target.below;
		if (target.below != null)
			target.below.above = this;
		target.below = this;

		RootAgain();
		print("stacked");
	}
	public void Detach()
	{
		print("deataching");
		if (above !=null) above.below = below;
		if (below != null) below.above = above;
		
		if (root == this)
		{
			root = below;
			if (root != null)
				root.above = null;
		}
		above = null;
		below = null;
		RootAgain();
	}
	StackBlock FindSnapTarget()
	{
		print("finding snap target");
		StackBlock[] blocks = FindObjectsOfType<StackBlock>();
		float mindist = 200f;
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
				print((int)d + " distance + best is:  " + b);
				mindist = d;
				best = b;
			}
			print("FOREACH ITERATION");
		}
		print("found somethign  " + best);
		print(blocks.Length + "\t" + blocks);
		print("best: " + best);
		return best;
	}
	static void RootAgain()
	{
		StackBlock[] blocks = FindObjectsOfType<StackBlock>();
		root = null;
		foreach (var b in blocks)
		{ if (b.above == null)
			{
				root = b;
				break;
			}
		}
	}

    void LinkStep()
	{
		StopAllCoroutines();
		ResetVisual();
		StartCoroutine(RunStack());
		
	}


    //void LinkStep()
    //{
    //	if (root == null)
    //	{
    //		Debug.LogWarning("no root to execute");
    //		return;
    //	}
    //	StackBlock current = root;
    //	while (current != null)
    //	{
    //		current.Execute();
    //		current = current.below;
    //	}
    //}
    void Execute()
	{
		Debug.Log("executing"); // place execution code here
		block.GetComponent<Renderer>().material = materialgreen;
		if (action == 0 && nextaction)
		{
            nextaction = false;
            int variable = (int)SliderObj.value;
			StartCoroutine(Travel(variable,false));
		}
		else if (action == 1 && nextaction)
		{
            nextaction = false;
        }
		else if (action == 2 && nextaction)
		{
            nextaction = false;
        }
		else if (action == 3 && nextaction)
		{
            nextaction = false;
        }
		
	}



	IEnumerator RunStack()
	{
		if (root == null) yield break;

		StackBlock current = root;

		while (current != null)
		{
			if (!nextaction)
			{ yield return null; }
			else if (nextaction)
			{
				print("running next action in runstack");
				nextaction = false;

				current.Execute();

				current = current.below;
			}
		}


	}

	//void RunStack()
	//{

	//	if (root == null) return;
	//	current = root;
	//	if (current != null && !nextaction)
	//	{
	//		print("runstack void");
	//		nextaction = false;
	//		current.Execute();
	//		current = current.below;
	//	}
	//}
















	void ResetVisual()
	{
		foreach(var b in FindObjectsOfType<StackBlock>())
		{
			b.block.GetComponent<Renderer>().material = materialred;
		}
	}

	IEnumerator Travel(int distance, bool direction)
	{
        float CurrentTravelDist = 0;
        
		while (distance >= CurrentTravelDist)
		{
			yield return new WaitForSeconds(0.01f);
			if (direction)
			{
				Player.transform.position -= new Vector3(TravelSpeed * 0.01f, 0,0);
				CurrentTravelDist += TravelSpeed * 0.01f;
				yield return null;
			}
			else 
			{
				Player.transform.position += new Vector3(TravelSpeed * 0.01f, 0, 0);
				CurrentTravelDist += TravelSpeed * 0.01f;
				yield return null;
			}
            
			print("done moving: " + distance + " traveled: " + CurrentTravelDist);
        }
		print("completed travel");
		yield return new WaitForSeconds(1);
        nextaction = true;
		//Player.transform.position = new Vector3(Player.transform.position.y,Mathf.Round(Player.transform.position.x)+0.5f,0);
		// attempt to fix drift
	}


	IEnumerator ExecuteRoutine()
	{
		if (root == null) yield break;
		IsExecuting = true;
		StackBlock current = root;
		while (current != null)
		{
			current.Execute();
			yield return new WaitForSeconds(0.3f);
			current = current.below;
		}
		IsExecuting = false;
		Runningroutine = null;
	}


	IEnumerator DoAction(Action onDone)
	{
		float duration = 2f;
		float timer = 0f;

		Vector3 start = transform.position;
		Vector3 end = start + Vector3.right * 2f ;
		while (timer < duration)
		{
			timer+= Time.deltaTime;
			transform.position = Vector3.Lerp(start, end, timer / duration);
			yield return null;
		}
		onDone.Invoke();
	}
















        void LateUpdate()
	{
		if (root != null && root.above != null)
			Debug.LogError("root state bad");

		int tops = 0;
		foreach (var b in FindObjectsOfType<StackBlock>())
		{
			if (b.above == null)
            {
				tops++;
            }
        }
		if (tops > 1)
			Debug.LogWarning("multiple tops");
	
	
	}


}
