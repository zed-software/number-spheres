using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * This class is used to control the game over menu and whatever options it may have
 **/ 
public class GameOverController : MonoBehaviour {

	public Text scoreText;		// Used to display the total user score after losing
	public Text TimetoAnswer; //Displays average time to answer a question

	// Use this for initialization
	void Start () 
	{
		scoreText.text = ("Score: " + PlayerPrefs.GetFloat ("Score")); // Sets the text objest to show the score
		TimetoAnswer.text = ("Time to Answer: " + (float)Mathf.Round(PlayerPrefs.GetFloat("TimeToAnswer")*10) / 10);
	}
	
	// Waits for the user to tap the screen before reloading the main game scene
	void Update () 
	{
//		for (int i = 0; i < Input.touchCount; ++i) 
//		{
//			if (Input.GetTouch (0).phase.Equals (TouchPhase.Began)) 
//			{
//				SceneManager.LoadScene (1);
//			}
//		}
//
//		if(Input.touchCount > 0)
//			SceneManager.LoadScene (1);
	}
}
