using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public GameObject[] ballz;			// Array holding mathballz prefabs
	public GameObject[] powerUps;		// Array holding power up prefabs
	public GameObject levelTransition;	// Level transition image and text
	public GameObject speedBonus;		// UI element for the speed bonus
	public GameObject speedBonusScoreText;
	[Tooltip("x and y range of ball spawning zone, should be set according to boundry size")]
	public Vector2 spawnValue;			// x and y range of ball spawning zone, should be set according to boundry size
	public Text problemText;			// Text that displays the problem to the user
	public Text comboText;				// Text that displays the combo multiplier
	public Text bonusTimeText;			// Text that appears when time rolls over to the next level
	public Text scoreText;				// Text UI for the the score
	public Text timerText;				// Text UI for the timer
	[Tooltip("Timer starting value, reset to this value when level is changed")]
	public float timerValue;			// Timer starting value, reset to this value when level is changed
	[Tooltip("How long (in seconds) the level transitions last")]
	public float levelStartDelay;		// How long (in seconds) the level transitions last
	[Tooltip("Starting health")]
	public int healthStart;				// The starting health
	public Slider healthSlider;			// Health bar UI
	public Slider speedBonusSlider;

	[Tooltip("Amount of correct Ballz clicked before the lvl up")]
	public int totalLevelProgressClicks;// The amount of correct clicks before the level progresses
	[Tooltip("The max combo multiplier achievable for the player")]
	public int maxCombo;				// The max combo multiplier possible
	[Tooltip("The maximum speed bonus multiplayer. It degrades per second, until it is 1x")]
	public int maxSpeedBonus;		
	[Tooltip("The max range of numbers that are chosen for the problem increases by this much every level up past level 4")]
	public int problemValueRaise;	

	private GameObject sbst;
	private float timer;				// The timer that will count down
	private float bonusTime;			// Remainding timer after a level up, rolls over to the new timer
	private float scoreStartTime;		// Used to for the stopwatch between correct clicks that measures the speed bonus
	private float scoreStopTime;		// Used to for the stopwatch between correct clicks that measures the speed bonus
	private float scoreSpeedBonus;		// Used to multiply the score earned by the speed of the player getting answers correct 
	private GameObject[] ballzObjects;	// An array holding the instantiated mathballz
	private int valueAssignedOrder;		// Used to keep track of which value has been assigned to a ball, set to 0 right before all the ballz spawn
	private int[] ballValues;			// An array of all the ball values, first value is the correct one
	private float totalScore;			// Total player score
	private float score;				// Score * combo * time
	private int comboValue;				// Holds how many answers in a row the player got correct
	private bool gameOver;				// Bool used to check if the game has ended
	private bool noBallz;				// Bool used to check if there are any ballz in the gameworld
	private int level;					// Keeps track of the level
	private LevelController lc;			// Used to call the LevelController script
	private int levelProgressClicks;	// Used to keep track of how many clicks have been correct on current level
	private int health;					// Used to keep track of current health
	private Text levelText;				// Level transition text
	private bool transitioningLevel;	// Used to stop timer and ballspawnz while the level transition card is up
	private float TimeToAnswerInitial;
	private float TimeToAnswerTotal;
	private int TimeToAnswerIncrement;


	void Start () 
	{
		bonusTime = 0;

		// Set to first level
		level = 1;				
		lc = GetComponent<LevelController> ();
		lc.SetLevel (level);	

		gameOver = false;
		noBallz = true;

		TimeToAnswerIncrement = 0;
		TimeToAnswerInitial = 0;
		TimeToAnswerTotal = 0;


		ResetScore();			// Sets score to 0
		ResetProgress ();		// Sets correct progress clicks to 0 
		ResetCombo();			// Sets the starting combo to 1x
		ResetHealth();

		timer = timerValue;		// Sets the timer to its starting value

		levelText = levelTransition.GetComponentInChildren<Text> ();

		TransitionLevel ();	// Brings up first level card
	}

	// Update is called once per frame
	void Update () 
	{
		if (!transitioningLevel) // Stops timer and ballz from spawning while level transition cards are up
		{
			// If there is still time left, and if the user hasn't beaten the final level, the game checks if there are any ballz in the game and then spawns them
			if (timer > 0 && !gameOver)
			{

				// If there is bonus time available, the bonus time counts down instead
				if (bonusTime > 0)
				{
					bonusTime -= Time.deltaTime;
					bonusTimeText.text = ("+ " + Mathf.Round (bonusTime).ToString () + "!");
				} else
				{ // No bonus time means the main timer counts down
					timer -= Time.deltaTime;
					bonusTimeText.text = ("");
				}

				if (timer >= 10)//Place holder timer until actual 0:20 format can be coded
					timerText.text = ("0:" + Mathf.Round (timer).ToString ()); // Rounds the timer value and displays it on the timer UI
				else
					timerText.text = ("0:0" + Mathf.Round (timer).ToString ());

				// If ballz have just been deleted, more are spawned
				if (noBallz)
				{
					SpawnBallz ();
				}

				if (speedBonusSlider.IsActive ()) 
				{
					speedBonusSlider.value -= Time.deltaTime;

					if (speedBonusSlider.value <= 0)
						speedBonus.SetActive (false);
				}
			} else
			{ // If the timer runs out or the game is over
				GameOver ();
			}
		}

	}


	// Function that spawns ballz, called when there are no ballz in the game
	// This function also calls the LevelController to generate problems and values for the ballz
	void SpawnBallz()
	{
		StartWatch ();						// Stop watch sets its starting time whenever the ball wave is spawned, this is used for the speed bonus

		speedBonus.SetActive (true);
		speedBonusSlider.value = maxSpeedBonus;

		noBallz = false;					// Sets this to false so the game wont spawn more ballz until they are all reset
		valueAssignedOrder = 0;				// Used when the ballz call the GetBallValue() function to retrieve a value, keeps track of which values have been assigned

		lc.GenerateProblem (ballz.Length);		// Generates a problem for the user to solve, depending on the level
		ballValues = lc.GetValues (); 			// Gets the correct and incorrect values from the LevelController

		ballzObjects = new GameObject[ballz.Length];	// Declares size of array that will hold instatiated ballz to the size of the ballz array set in the unity inspector

		// loop that spawns ballz in array
		for (int x = 0; x < ballz.Length; x++) 
		{
			Vector2 spawnLocation = new Vector2 (Random.Range (-spawnValue.x, spawnValue.x), Random.Range (-spawnValue.y, spawnValue.y)); // Picks random coordinates within specified range

			ballzObjects[x] = (GameObject) Instantiate(ballz [x], spawnLocation, Quaternion.identity); // Quaternion.identity corresponds to "no rotation", used to align object with the world or parent. Quaternions still confuse me
		}

		//Used to spawn health balls every 5 levels, at the beginning of the level
		//Currently programmed to pick a random power up from the power up array, but only one exits so far
		if ((level % 5 == 0) && levelProgressClicks == 0)
		{
			Vector2 spawnLocation = Random.insideUnitCircle.normalized * 10; // Picks a random location along a circle with radius of 10
			Instantiate(powerUps[Random.Range(0, powerUps.Length)], spawnLocation, Quaternion.identity);
		}
	}
		

	// Public function called by ballz when they spawn to get assigned a value
	// This function returns an array with 2 slots, 1st slot has the number value, second slot tells if its the correct answer to the problem
	public int[] GetBallValue()
	{
		int[] returnArray = new int[2]; // Array that will be returned

		if (valueAssignedOrder == 0) // First value is the correct one, and is given to the first ball to spawn
		{
			returnArray [0] = ballValues [0];
			returnArray [1] = 1;
		} else // Rest of the ballz get these shitty incorrect values
		{
			returnArray [0] = ballValues [valueAssignedOrder];
			returnArray [1] = 0;
		}

		valueAssignedOrder++; // Keeping track of which values were assigned

		return returnArray;
	}


	// Public function called by the correct mathball once its clicked, destroys the ballz on the screen
	public void ResetBallz ()
	{
		//Loops through the array holding our insatiated ballz and destroys them
		for (int x = 0; x < ballz.Length; x++) 
		{	
			if( ballzObjects [x] != null )
				ballzObjects [x].gameObject.GetComponent<BallController> ().Explode (); // Explodes remaining ballz

			Destroy (ballzObjects [x].gameObject);
		}

		if (gameOver == false) {
			TimeToAnswerInitial = Time.time;
		}
		noBallz = true; // Indicates that all the ballz have been deleted
	}
		

	// Public function called by the correct ballz when they are clicked
	// Parameter s is the score value assigned to the mathball
	public void AddScore(int s) 
	{	
		score = (s * comboValue * scoreSpeedBonus);
		totalScore += score;
		UpdateScore ();

		//Debug.Log ("Correct click score " + (s * comboValue * scoreSpeedBonus) + " = " + s + " * " + comboValue + " * "  + scoreSpeedBonus);
	}

	// Public function called by ball controller on correct clicks, used to display pop up score
	public float GetBallScore()
	{
		return score;
	}


	// Just sets score to 0
	void ResetScore()
	{
		totalScore = 0;
		UpdateScore ();
	}
		

	// Updates the score text in the UI
	void UpdateScore()
	{
		scoreText.text = Mathf.Round(totalScore).ToString();
	}
		

	// Public function called by the level controller to change the UI text for the problem
	public void UpdateProblem(string p)
	{
		problemText.text = p;
	}


	// Public function called by the correct ball when its clicked, it counts up how many times the correct ball has been clicked per level
	// and checks if its enough to level up, also checks if level 5 has been completed and triggers a game over if it has
	public void AddProgress()
	{
		levelProgressClicks++;
		StopWatch (); // Stops the speed bonus timer when the correct mathball calls this function

		if (levelProgressClicks == totalLevelProgressClicks) // Level up
		{
			ResetProgress ();

			level++; // ++ local int that keeps track of the level

			StartCoroutine ("WaitForTransition"); // Brings up the level transition card after a short wait
//			ResetHealth ();					// Sets health back to starting value

			// If we have leveled past the first 4, the max range of the generated numbers is increased and a random level is picked, 
			// this is the game loop from this point on
			if (level > 4)
			{
				lc.RaiseMaxProblemValues (problemValueRaise);
			}

			lc.SetLevel (level); // Letting the level controller know what level it is now

			bonusTime += timer; // Any remaining time is added to the bonus time
			timer = timerValue;	// Main timer is reset
		}
	}
		

	// Function used to reset the correct clicks to 0 when the level changes
	void ResetProgress()
	{
		levelProgressClicks = 0;
	}
		
	// Public function called by a correct mathball when clicked, adds to the combo bonus until it hits the max
	public void AddCombo()
	{
		comboText.text = ("x" + comboValue);  // Shows how many in a row you have clicked

		if (comboValue < maxCombo)
			comboValue++;
	}
		

	// Public function called by an incorrect mathball when clicked, resets the combo bonus to 1x
	public void ResetCombo()
	{
		comboText.text = (""); // Removes combo text from screen
		comboValue = 1;
	}
		
	// Starts the counter for the speed bonus by setting scoreStartTime to the current seconds since game launched
	void StartWatch()
	{
		scoreStartTime = Time.time;
	}
		

	// Stops the counter for the speed bonus and calculates the difference from the start time, then subtracts that from the max speed bonus multiplier
	// The remaining value is used to multiply the score
	void StopWatch()
	{
		scoreStopTime = Time.time;
		scoreSpeedBonus = (maxSpeedBonus - (scoreStopTime - scoreStartTime));

		if (scoreSpeedBonus < 1) // If the timer counted past the max multiplier set, the speed bonus is just set to 1x;
			scoreSpeedBonus = 1;

//		sbst = Instantiate (speedBonusScoreText, speedBonusSlider.transform.position, Quaternion.identity);
//		sbst.GetComponent<TextMesh> ().text = ("x" + scoreSpeedBonus);

		//Debug.Log ("Timer Combo x" + scoreSpeedBonus);
	}


	// Public function called by the incorrect mathball when clicked
	// Reduces health variable by 1 and if health reaches 0, it deletes the balls and causes a game over
	public void LoseHealth()
	{
		health -= 1;

		healthSlider.value = health; // Sets UI health bar to current health

		if (health == 0) {
			GameOver ();
		}
	}
		

	// Resets health variable back to the starting value
	// Called in start() and during level ups
	public void ResetHealth()
	{
		health = healthStart;
		healthSlider.value = health;
	}


	// Called when the timer or health hits 0
	// Deletes ballz on screen then waits for a second before loading the game over screen with the user's score
	void GameOver()
	{
		gameOver = true; 	// Stops more ballz from spawning while waiting for game over scene
		ResetBallz();		

		PlayerPrefs.SetFloat ("Score", Mathf.Round(totalScore)); // Saves the score to a settings file for the game over screen
		PlayerPrefs.SetFloat ("TimeToAnswer", TimeToAnswerTotal / TimeToAnswerIncrement);
		StartCoroutine ("WaitForGameOver");		// Coroutine that will wait a little before loading game over screen, no waiting makes the transition too abrupt
	}


	// Waits a little after hitting game over state then loads the game over scene
	IEnumerator WaitForGameOver()
	{
		yield return new WaitForSeconds (2);	// Waits 2 seconds
		SceneManager.LoadScene (2);				// Loads main scene
	}


	// Waits a little after hitting a level up to show the level transition card
	IEnumerator WaitForTransition()
	{
		transitioningLevel = true;	// Stops timer and ballz from spawning
		yield return new WaitForSeconds (0.8f);
		TransitionLevel ();
	}


	// Brings up the level transition card then disables it after a few seconds
	void TransitionLevel ()
	{
		string levelOperator;

		transitioningLevel = true; 	// Stops timer and ballz from spawning

		switch (level)
		{
			case 1:
				{
					levelOperator = "Addition";
					break;
				}
			case 2:
				{
					levelOperator = "Subtraction";
					break;
				}
			case 3:
				{
					levelOperator = "Multiplication";
					break;
				}
			case 4:
				{
					levelOperator = "Division";
					break;
				}
			default:
				{
					levelOperator = "Grab Bag";
					break;
				}
		}

		levelText.text = "Level " + level + "\n" + levelOperator;
		levelTransition.SetActive (true);	// Turns on the level up UI

		Invoke ("HideLevelTransition", levelStartDelay); // Invoke is a way to wait before calling a function 
	}

	// disables the level up transition card
	void HideLevelTransition()
	{
		levelTransition.SetActive (false);
		transitioningLevel = false; // allows ballz to spawn again
	}
<<<<<<< HEAD

	public void TimeToAnswer()
	{
		TimeToAnswerTotal += Time.time - TimeToAnswerInitial;
		TimeToAnswerIncrement++;
	}
}
=======
}
>>>>>>> Johan_Dev_Laptop
