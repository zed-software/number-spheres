#if UNITY_ANDROID

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnTouchDown : MonoBehaviour
{
	// Detects a touch and uses rays to check for collisions with ball objects

	bool paused = false;

	public void TogglePause()
	{
		paused = !paused;
		Debug.Log ("Touch Pause: " + paused.ToString ());
	}

	void Update () 
	{
//		if (!paused)
//		{
//			Debug.Log ("TOUCH TEST");
			RaycastHit2D hit = new RaycastHit2D (); 	// Used to hold our hit information 
			Vector2 wp = new Vector2 ();			// Used to hold the world point of the touch

			for (int i = 0; i < Input.touchCount; ++i) // Runs if a touch is detected
			{
				if (Input.GetTouch (i).phase.Equals (TouchPhase.Began)) // Only the begining of the touch, no swiping
				{
					wp = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position); // Converts the touch to a world point
					hit = Physics2D.Raycast (wp, Vector2.zero); // Makes a ray directly forward from the world touch point

					if (hit.collider != null) // if the ray hit something
					{
						if (!paused)
						{
							Debug.Log("Pause touch test");
							hit.collider.gameObject.SendMessage ("TouchBall"); // Tell the ballz' touch collider that it was hit, and that will tell the ball controller it was hit
						}
							
					}
				}
			}
		}
//	}
}
#endif