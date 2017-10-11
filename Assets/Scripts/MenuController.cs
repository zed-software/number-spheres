using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is used to control the main menu and whatever options it will have
 **/
public class MenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{	
		// Currently just waits for a tap from the user to begin the game
		for (int i = 0; i < Input.touchCount; ++i) 
		{
			if (Input.GetTouch (0).phase.Equals (TouchPhase.Began)) 
			{
				SceneManager.LoadScene (1);
			}
		}
	}
}
