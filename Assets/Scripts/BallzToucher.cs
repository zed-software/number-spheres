using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallzToucher : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.touchCount > 0) 
		{
			Vector3 wp = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);

		
			if (this.GetComponent<Collider2D>().OverlapPoint(wp)) 
			{
				this.GetComponentInParent<BallController> ().ballTouched ();
			}
		}
		
	}
}
