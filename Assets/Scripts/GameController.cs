using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Testing Repo

public class GameController : MonoBehaviour {

	public GameObject[] ballz;			// Array holding mathballz prefabs
	public Vector2 spawnValue;			// x and y range of ball spawning zone, should be set according to boundry size
	public Text problemText;			// Text that displays the problem to the user
	public TextMesh comboText;			// Text that displays the combo multiplier
	public TextMesh bonusTimeText;		// Text mesh that appears when time rolls over to the next level
	public TextMesh scoreText;			// Text mesh on the score ball
	public TextMesh timerText;			// Text mesh on the timer ball
	public float timerValue;			// Timer starting value, reset to this value when level is changed

	[Tooltip("Amount of correct Ballz clicked before the lvl up")]
	public int totalLevelProgressClicks;// The amount of correct clicks before the level progresses
	[Tooltip("The max combo multiplier achievable for the player")]
	public int maxCombo;				// The max combo multiplier possible
	[Tooltip("The maximum speed bonus multiplayer. It degrades per second, until it is 1x")]
	public int maxSpeedBonus;			

	private float timer;				// The timer that will count down
	private float bonusTime;			// Remainding timer after a level up, rolls over to the new timer
	private float scoreStartTime;		// Used to for the stopwatch between correct clicks that measures the speed bonus
	private float scoreStopTime;		// Used to for the stopwatch between correct clicks that measures the speed bonus
	private float scoreSpeedBonus;		// Used to multiply the score earned by the speed of the player getting answers correct 
	private GameObject[] ballzObjects;	// An array holding the instantiated mathballz
	private int valueAssignedOrder;		// Used to keep track of which value has been assigned to a ball, set to 0 right before all the ballz spawn
	private int[] ballValues;			// An array of all the ball values, first value is the correct one
	private float score;				// Total player score
	private int comboValue;				// Holds how many answers in a row the player got correct
	private bool gameOver;				// Bool used to check if the game has ended
	private bool noBallz;				// Bool used to check if there are any ballz in the gameworld
	private int level;					// Keeps track of the level
	private LevelController lc;			// Used to call the LevelController script
	private int levelProgressClicks;	// Used to keep track of how many clicks have been correct on current level

	void Start () 
	{
		bonusTime = 0;

		// Set to first level
		level = 1;				
		lc = GetComponent<LevelController> ();
		lc.SetLevel (level);	

		gameOver = false;
		noBallz = true;

		ResetScore();			// Sets score to 0
		ResetProgress ();		// Sets correct progress clicks to 0 
		ResetCombo();			// Sets the starting combo to 1x

		timer = timerValue;		// Sets the timer to its starting value

	}


	// Update is called once per frame
	void Update () 
	{

		// If the game is over, the user press R to reload the scene
		if (gameOver) {
			if (Input.GetKeyDown (KeyCode.R)) {
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

			}
		}

		// If there is still time left, and if the user hasn't beaten the final level, the game checks if there are any ballz in the game and then spawns them
		if (timer > 0 && !gameOver) {

			// If there is bonus time available, the bonus time counts down instead
			if (bonusTime > 0)
			{
				bonusTime -= Time.deltaTime;
				bonusTimeText.text = ("+ " + Mathf.Round (bonusTime).ToString () + "!");
			} else // No bonus time means the main timer counts down
			{
				timer -= Time.deltaTime;
				bonusTimeText.text = ("");
			}

			timerText.text = Mathf.Round (timer).ToString (); // Rounds the timer value and displays it on the timer ball

			// If ballz have just been deleted, more are spawned
			if (noBallz) {
				SpawnBallz ();
			}
		} else { // If the timer runs out or the game is over
			gameOver = true;
			SetGameOverText ();	// Sets the problem text to game over and reset instruction text
			ResetBallz ();	// Deletes any remaining ballz
		}
	}


	// Function that spawns ballz, called when there are no ballz in the game
	// This function also calls the LevelController to generate problems and values for the ballz
	void SpawnBallz()
	{
		StartWatch ();						// Stop watch sets its starting time whenever the ball wave is spawned, this is used for the speed bonus

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
			Destroy (ballzObjects [x].gameObject);
		}

		noBallz = true; // Indicates that all the ballz have been deleted
	}
		

	// Public function called by the correct ballz when they are clicked
	// Parameter s is the score value assigned to the mathball
	public void AddScore(int s) 
	{		
		score += (s * comboValue * scoreSpeedBonus);
		UpdateScore ();

		//Temporary, uncomment to display the multipliers and total points awarded for the correct answer
		Debug.Log ("Correct click score " + (s * comboValue * scoreSpeedBonus) + " = " + s + " * " + comboValue + " * "  + scoreSpeedBonus);
	}


	// Just sets score to 0
	void ResetScore()
	{
		score = 0;
		UpdateScore ();
	}
		

	// Updates the text mesh on the score ball
	void UpdateScore()
	{
		scoreText.text = Mathf.Round(score).ToString();
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
			level++;
			lc.SetLevel (level);

			bonusTime += timer; // Any remaining time is added to the bonus time
			timer = timerValue;	// Main timer is reset
		}

		// Temporary end state until we figure out total levels or level loops
		if (level > 5)
		{
			gameOver = true;
		}
	}
		

	// Function used to reset the correct clicks to 0 when the level changes
	void ResetProgress()
	{
		levelProgressClicks = 0;
	}


	// Function used to give player reset instructions once a game over is triggered
	void SetGameOverText()
	{
		problemText.text = ("Game Over: Press R to restart");
	}
		
	// Public function called by a correct mathball when clicked, adds to the combo bonus until it hits the max
	public void AddCombo()
	{
		comboText.text = ("COMBO x" + comboValue);  // Shows how many in a row you have clicked

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

		//Debug.Log ("Timer Combo x" + scoreSpeedBonus);
	}

}
