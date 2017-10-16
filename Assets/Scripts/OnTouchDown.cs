#if UNITY_ANDROID

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OnTouchDown : MonoBehaviour
{
	void Update () 
	{
		RaycastHit2D hit = new RaycastHit2D();
		Vector2 wp = new Vector2 ();

		for (int i = 0; i < Input.touchCount; ++i) 
		{
			if (Input.GetTouch(i).phase.Equals(TouchPhase.Began)) 
			{
				wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
				hit = Physics2D.Raycast (wp, Vector2.zero); 

				if (hit.collider != null) 
				{
					Debug.Log (hit.collider.name);

					hit.collider.gameObject.SendMessage ("TouchBall");
				}
			}
		}
	}
}
#endif