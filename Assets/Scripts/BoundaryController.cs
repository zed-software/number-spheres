using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryController : MonoBehaviour {


	void Start () 
	{
//		Debug.Log ("Height: " + Screen.height);
//		Debug.Log ("Width: " + Screen.width);

		float distanceAdjuster = 1;
		float uiAdjuster = 1.3f;

		if (transform.position.z == 10)
		{
			distanceAdjuster = 2;
			uiAdjuster = 0;
		}

		float height = Camera.main.orthographicSize * distanceAdjuster;
//		Debug.Log ("Camera.main.orthographicSize: " + height);

		float width = height * Screen.width / Screen.height;
//		Debug.Log ("height * Screen.width / Screen.height: " + width); 

		transform.localScale = new Vector3 (width, (height - uiAdjuster), 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
