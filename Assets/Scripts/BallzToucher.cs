﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallzToucher : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Original ball toucher code
		// Every individual ball used to check if they were tapped when a touch was detected
//		Vector3 wp = new Vector3 ();
//
//		for (int i = 0; i < Input.touchCount; ++i) 
//		{
//			if (Input.GetTouch (i).phase.Equals (TouchPhase.Began)) 
//			{
//				wp = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position); // ScreenToWorldPoint takes a screen position (the touch) and returns a position in world space
//				
//				if (this.GetComponent<Collider2D>().OverlapPoint(wp)) // Checks if the screen touch world position overlaps the touch collider on this object
//				{
//					transform.parent.gameObject.SendMessage ("BallTouched");
////					this.GetComponentInParent<BallController> ().BallTouched ();
//				}
//			}
//		}
	}


	// Called by the OnTouchDown script on the camera
	public void TouchBall()
	{
		transform.parent.gameObject.SendMessage ("BallTouched"); // Sends this message to our ball controller script, so the script needs a BallTouched() function
	}
}
