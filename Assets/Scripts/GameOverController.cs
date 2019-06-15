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

	// public Text addPctCorrect, subPctCorrect, multPctCorrect, divPctCorrect;  //text used to display Pct Correct stats.
	// public Text addSpeed, subSpeed, multSpeed, divSpeed;
	// public Text sessionStreak, recordStreak;

	//public bool newRecord;

	public GameObject MenuAudio;

//	public Text TimetoAnswer; //Displays average time to answer a question
	private HighScore hs;
	private AudioSource menuAudioSource;

	// Use this for initialization
	void Start () 
	{
	//	newRecord = false;
		menuAudioSource = MenuAudio.GetComponent<AudioSource> ();

		if (PlayerPrefs.GetInt ("isMuteSoundEffects") == 1)
			menuAudioSource.volume = 0;
		
		scoreText.text = ("Score: " + PlayerPrefs.GetFloat ("Score")); // Sets the text objest to show the score

		hs = GetComponent<HighScore> ();

		hs.SetScore (PlayerPrefs.GetFloat ("Score"));
//		TimetoAnswer.text = ("Time to Answer: " + (float)Mathf.Round(PlayerPrefs.GetFloat("TimeToAnswer")*10) / 10);

		highScoreText0.text = ("1. " + PlayerPrefs.GetFloat("HS00"));
		highScoreText1.text = ("2. " + PlayerPrefs.GetFloat("HS01"));
		highScoreText2.text = ("3. " + PlayerPrefs.GetFloat("HS02"));
		highScoreText3.text = ("4. " + PlayerPrefs.GetFloat("HS03"));
		highScoreText4.text = ("5. " + PlayerPrefs.GetFloat("HS04"));

		// addPctCorrect.text = (PlayerPrefs.GetFloat ("addPctSession") + "%");
		// subPctCorrect.text = (PlayerPrefs.GetFloat ("subPctSession") + "%");
		// multPctCorrect.text = (PlayerPrefs.GetFloat ("multPctSession") + "%");
		// divPctCorrect.text = (PlayerPrefs.GetFloat ("divPctSession") + "%");

		// addSpeed.text = (PlayerPrefs.GetString ("addSpeedSession") + "s");
		// subSpeed.text = (PlayerPrefs.GetString ("subSpeedSession") + "s");
		// multSpeed.text = (PlayerPrefs.GetString ("multSpeedSession") + "s");
		// divSpeed.text = (PlayerPrefs.GetString ("divSpeedSession") + "S");

		// sessionStreak.text = (PlayerPrefs.GetInt ("sessionHighStreak").ToString());
		// recordStreak.text = (PlayerPrefs.GetInt ("highStreak").ToString());

		// if (PlayerPrefs.GetString ("newHighStreak") == "true") {
		// 	gameObject.SetActive (true);
		// }

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
