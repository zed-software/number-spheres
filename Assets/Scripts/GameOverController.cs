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
	public Text highScoreText0, highScoreText1, highScoreText2, highScoreText3, highScoreText4;
//	public Text TimetoAnswer; //Displays average time to answer a question
	private HighScore hs;

	// Use this for initialization
	void Start () 
	{
		
		scoreText.text = ("Score: " + PlayerPrefs.GetFloat ("Score")); // Sets the text objest to show the score

		hs = GetComponent<HighScore> ();

		hs.SetScore (PlayerPrefs.GetFloat ("Score"));
//		TimetoAnswer.text = ("Time to Answer: " + (float)Mathf.Round(PlayerPrefs.GetFloat("TimeToAnswer")*10) / 10);

		highScoreText0.text = ("1. " + PlayerPrefs.GetFloat("HS00"));
		highScoreText1.text = ("2. " + PlayerPrefs.GetFloat("HS01"));
		highScoreText2.text = ("3. " + PlayerPrefs.GetFloat("HS02"));
		highScoreText3.text = ("4. " + PlayerPrefs.GetFloat("HS03"));
		highScoreText4.text = ("5. " + PlayerPrefs.GetFloat("HS04"));

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
