using System;
using System.Collections;
using System.Collections.Generic;

using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StackBlock : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler,IScrollHandler
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
	public	Dropdown Options;
	/// <summary>
	public StackBlock publictarget;
	public GameObject Player;
	public float TravelSpeed = 2f;
	public int action = 0;
	public bool nextaction = true;
	public Slider SliderObj;
	public Text valuetext;
	public bool modifyingSlider = false;
	public float mousedelta = 0f;	
	StackBlock current;
    private static bool IsExecuting = false;
	private static Coroutine Runningroutine;
	int tick = 0;
	public GameObject bullet;
	public float JumpForce = 8500f;
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
	public void Update ()
	{ 
        if (Options.captionText.text == "Move")
		{
			action = 0;
		}
        if (Options.captionText.text == "Shoot")
        {
			action = 1;
        }
        if (Options.captionText.text == "Jump")
        {
			action = 2;
        }
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
		if (Options == null)
		{
			Debug.LogError("missing options");
		}
		
        valuetext.text = ((int)SliderObj.value).ToString();
		
		tick++;

    }
	public void OnScroll(PointerEventData eventData)
	{
		if (modifyingSlider)
		{
			print("modifilying slider: scroll value: "  + eventData.scrollDelta.y);
			if (eventData.scrollDelta.y > 0)
			{
				SliderObj.value +=1f;
			}
			if (eventData.scrollDelta.y < 0)
			{
				SliderObj.value -=1f;
            }
		}
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		modifyingSlider = true;
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		modifyingSlider = false;
	}
	public void OnPointerDown(PointerEventData eventData)
    {
        
        print("mousedownonme");
		dragging = true;
		if (above != null || below != null) Detach();
		Vector3 mouseworld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mouseworld.z = transform.position.z;
		dragoffset = transform.position - mouseworld;	
	}
	public void OnDrag(PointerEventData eventData)
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

	//code for actually executing a set of code
    void Execute()
	{
		Debug.Log("executing"); // place execution code here
		block.GetComponent<Renderer>().material = materialgreen;
		if (action == 0 && nextaction)
		{
			//Code for executing walking
            nextaction = false;
            int variable = (int)SliderObj.value;
			StartCoroutine(Travel(variable,false));
		}
		else if (action == 1 && nextaction)
		{
			//code for executing a SHOOT
			print("Shooting");
            nextaction = false;
			Shoot();

        }
		else if (action == 2 && nextaction)
		{
			//code for a JUMP
			print("jumping");
            Jump();
            nextaction = false;
        }
		else if (action == 3 && nextaction)
		{
            Debug.Log("Action not yet coded");
            nextaction = false;
        }
		else if (!nextaction)
		{
			print("no nextaction");
		}	
		else
		{
			Debug.LogError("something went wrong in executing");
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
	//^^^^^^^^^^^^ Causes crash

	public void SetAction(int action)
	{
		print(action + "  value changed");
	this.action = action; 
	}













	void ResetVisual()
	{
		foreach(var b in FindObjectsOfType<StackBlock>())
		{
			b.block.GetComponent<Renderer>().material = materialred;
		}
	}

	//Walking Code
	//PLEASE NOTE: i have commented out all debug mentions to test other stuff, keep in mind if we have to fix movement
	IEnumerator Travel(int distance, bool direction)
	{
		//FIXME: add a way to go both LEFT & RIGHT
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
            
			//print("done moving: " + distance + " traveled: " + CurrentTravelDist);
        }
		//print("completed travel");
		yield return new WaitForSeconds(1f* distance);
        nextaction = true;
		//Player.transform.position = new Vector3(Player.transform.position.y,Mathf.Round(Player.transform.position.x)+0.5f,0);
		// attempt to fix float drift
	}
	public void Shoot()
	{
		PropelProjectile ppj = null;
		ppj = Instantiate(bullet,Player.transform.position,Quaternion.identity,Player.transform).GetComponent<PropelProjectile>();
		ppj.transform.position = Player.transform.position;
		ppj.transform.parent = Player.transform;
		ppj.transform.parent = null;
		ppj.Initialize(8f,0.1f,false);
		nextaction=true;
	}
	public void Jump()
	{
		Player.GetComponent<Rigidbody2D>().AddForce(new Vector3(0,JumpForce,0));
		nextaction=true;
	}
	// timing code 1 unit takes ~1.6s
	IEnumerator ExecuteRoutine()
	{
		if (root == null) yield break;
		IsExecuting = true;
		StackBlock current = root;
		while (current != null)
		{
			current.Execute();
			
			yield return new WaitForSeconds(1.6f*SliderObj.value);
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
