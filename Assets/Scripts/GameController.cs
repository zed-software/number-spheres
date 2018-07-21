using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class GameController : MonoBehaviour {

	public GameObject[] ballz;			// Array holding mathballz prefabs
	public GameObject[] powerUps;		// Array holding power up prefabs
	public Material[] ballMaterials;
	public Material[] backgroundMaterials;
	public Sprite[] ballFaces;
	public GameObject backGround;
	public GameObject levelTransition;	// Level transition image and text
	public GameObject speedBonus;		// UI element for the speed bonus
	public GameObject speedBonusScoreText;
	public GameObject shieldIcon;		// UI icon of shield
	[Tooltip("x and y range of ball spawning zone, should be set according to boundry size")]
//	public Vector2 spawnValue;			// x and y range of ball spawning zone, should be set according to boundry size
	public Text problemText;			// Text that displays the problem to the user
	public Text comboText;				// Text that displays the combo multiplier
	public Text bonusTimeText;			// Text that appears when time rolls over to the next level
	public Text scoreText;				// Text UI for the the score
	public Text timerText;				// Text UI for the timer
	public Text doublePointsText;		// Text UI for when the double points power up is active
	[Tooltip("Timer starting value, reset to this value when level is changed")]
	public float timerValue;			// Timer starting value, reset to this value when level is changed
	[Tooltip("How long (in seconds) the level transitions last")]
	public float levelStartDelay;		// How long (in seconds) the level transitions last
	[Tooltip("Starting health")]
	public int healthStart;				// The starting health
	public Slider healthSlider;			// Health bar UI
	public Slider speedBonusSlider;		// Slider used as visual representation of the speed bonus slider
	public Slider timerSlider;			// Slider used as visual representation of the game timer
	public Slider bonusTimeSlider;		// Time bonus power up slider

	[Tooltip("Amount of correct Ballz clicked before the lvl up")]
	public int totalLevelProgressClicks;// The amount of correct clicks before the level progresses
	[Tooltip("The max combo multiplier achievable for the player")]
	public int maxCombo;				// The max combo multiplier possible
	[Tooltip("The maximum speed bonus multiplayer. It degrades per second, until it is 1x")]
	public int maxSpeedBonus;			
	[Tooltip("The max range of numbers that are chosen for the problem increases by this much every level up past level 4")]
	public int problemValueRaise;	

//	private GameObject sbst;
	private float timer;				// The timer that will count down
	private float bonusTime;			// Remainding timer after a level up, rolls over to the new timer
	private float doublePointsTimer;	// Used to keep track of time left for the douple points power up
	private float freezeTimer;
	private float scoreStartTime;		// Used to for the stopwatch between correct clicks that measures the speed bonus
	private float scoreStopTime;		// Used to for the stopwatch between correct clicks that measures the speed bonus
	private float scoreSpeedBonus;		// Used to multiply the score earned by the speed of the player getting answers correct 
	private GameObject[] ballzObjects;	// An array holding the instantiated mathballz
	private int valueAssignedOrder;		// Used to keep track of which value has been assigned to a ball, set to 0 right beffffore all the ballz spawn
	private int[] ballValues;			// An array of all the ball values, first value is the correct one
	private float totalScore;			// Total player score
	private float score;				// Score * combo * time
	private int comboValue;				// Holds how many answers in a row the player got correct
	private bool gameOver;				// Bool used to check if the game has ended
	private bool noBallz;				// Bool used to check if there are any ballz in the gameworld
	private int level;					// Keeps track of the level
	private LevelController lc;			// Used to call the LevelController script
//	private HighScore hs;
	private int levelProgressClicks;	// Used to keep track of how many clicks have been correct on current level
	private int health;					// Used to keep track of current health
	private Text levelText;				// Level transition text
	private bool transitioningLevel;	// Used to stop timer and ballspawnz while the level transition card is up
//	private float timeToAnswerInitial;
//	private float timeToAnswerTotal;
//	private int timeToAnswerIncrement;
	private bool isShielded;			// Bool to keep track of the shield power up
	private bool isDoublePoints;		// Bool used to keep track of the double points power up
	private bool isFrozen;
	private Vector2 spawnValue;


	void Start () 
	{
		// Calculates the ball spawning range based on the screen resolution
		float ballzRadius = (transform.localScale.x / 2f);
		float spawnHeight = (Camera.main.orthographicSize) - ballzRadius - 1.3f; // Subtracting 1.3 to adjust for the UI
		float spawnWidth = (spawnHeight * Screen.width / Screen.height) - ballzRadius;

		spawnValue = new Vector2 (spawnWidth, spawnHeight);

		bonusTime = 0;

		// Set to first level
		level = 1;				
		lc = GetComponent<LevelController> ();
		lc.SetLevel (level);

//		hs = GetComponent<HighScore> ();

		gameOver = false;
		noBallz = true;
		isShielded = false;
		isFrozen = false;

//		timeToAnswerIncrement = 0;
//		timeToAnswerInitial = 0;
//		timeToAnswerTotal = 0;


		ResetScore();			// Sets score to 0
		ResetProgress ();		// Sets correct progress clicks to 0 
		ResetCombo();			// Sets the starting combo to 1x
		ResetHealth();

		DisableDoublePoints ();

		timer = timerValue;		// Sets the timer to its starting value
		timerSlider.maxValue = timerValue;
		timerSlider.value = timerSlider.maxValue;

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
					bonusTimeSlider.value = bonusTime;
					//bonusTimeText.text = ("+ " + Mathf.Round (bonusTime).ToString ());
				} else
				{ // No bonus time means the main timer counts down
					bonusTimeSlider.gameObject.SetActive(false);
					timer -= Time.deltaTime;
					//bonusTimeText.text = ("");
				}

				timerSlider.value = timer;

//				if (timer >= 10)//Place holder timer until actual 0:20 format can be coded
//					timerText.text = ("0:" + Mathf.Round (timer).ToString ()); // Rounds the timer value and displays it on the timer UI
//				else
//					timerText.text = ("0:0" + Mathf.Round (timer).ToString ());

				// If ballz have just been deleted, more are spawned
				if (noBallz)
				{
					SpawnBallz ();
				}

				// Controls the speed bonus slider
				if (speedBonusSlider.IsActive ()) 
				{
					speedBonusSlider.value -= Time.deltaTime;

					if (speedBonusSlider.value <= 0)
						speedBonus.SetActive (false);
				}


				// Counts down the double points timer
				if (isDoublePoints)
				{
					doublePointsTimer -= Time.deltaTime;

					if (doublePointsTimer <= 0)
						DisableDoublePoints ();
				}


				// Counts down the freeze timer
				if (isFrozen)
				{
					freezeTimer -= Time.deltaTime;

					if (freezeTimer <= 0)
						DisableFreeze ();
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
		ShuffleArray (ballMaterials);		// Randomizes the array of ball color materials
		ShuffleArray (ballFaces);			// Randomizes the array of ball faces

		StartWatch ();						// Stop watch sets its starting time whenever the ball wave is spawned, this is used for the speed bonus

		speedBonus.SetActive (true);		// Enables the speed bonus UI slider
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

			ballzObjects[x].GetComponentInChildren<MeshRenderer> ().material = ballMaterials[x];
			//ballzObjects [x].GetComponentInChildren<SpriteRenderer> ().sprite = ballFaces [x];
		}

		/////////////////////
		/// Power ups
		////////////////////


		// If the freeze power up is active, this will lower the mass of the new ballz and slow them down
		if (isFrozen)
		{
			FreezeBallz ();
		}


		if (levelProgressClicks > 0) // Stops power ups from spawning with the first wave of ballz every level, power ups only spawn with correct clicks
		{
			float randomPowerUpSeed = Random.value;
			Debug.Log ("Random Seed: " + randomPowerUpSeed);

			// 20% chance of spawning a power up
			if (randomPowerUpSeed > 0.8f)
			{
				SpawnPowerUp (1, powerUps.Length);
			}

			randomPowerUpSeed = Random.value; // Reroll for health

			// If health is at 2, 10% chance of health spawning; if health is at 1, 20% chance of health spawning 
			if ((health == 2 && randomPowerUpSeed > 0.90f) || (health == 1 && randomPowerUpSeed > 0.80f))
			{		
				SpawnPowerUp (0, 0); // First slot in the power ups array is the health ball, so the range here is 0 to 0
			}
		}
	}


	// Spawns a random power up in the power up array
	// Parameters are the range within the power ups array that can be chosen
	void SpawnPowerUp(int rangeMin, int rangeMax)
	{
		int powerUpIDRandomSeed = Random.Range (rangeMin, rangeMax);
		Debug.Log ("Random Power Up: " + powerUpIDRandomSeed);

		Vector2 spawnLocation = Random.insideUnitCircle.normalized * 10; // Picks a random location along a circle with radius of 10
		Instantiate (powerUps [powerUpIDRandomSeed], spawnLocation, Quaternion.identity);
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

//		4/25/2018 disabling Time to Answer code
//		if (gameOver == false) {
//			timeToAnswerInitial = Time.time;
//		}

		noBallz = true; // Indicates that all the ballz have been deleted
	}
		

	// Public function called by the correct ballz when they are clicked
	// Parameter s is the score value assigned to the mathball
	public void AddScore(int s) 
	{	
		score = (s * comboValue * scoreSpeedBonus);

		if (isDoublePoints)
			score *= 2;

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
//				backGround.GetComponent<MeshRenderer> ().material = backgroundMaterials [4];
			} 
//			else
//			{
//				//Debug.Log ("TEST");
//				backGround.GetComponent<MeshRenderer> ().material = backgroundMaterials [level - 1];
//			}

			lc.SetLevel (level); // Letting the level controller know what level it is now

//			bonusTime += timer; // Any remaining time is added to the bonus time
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
		if(isShielded)  // Checks if shield power up is active
		{
			RemoveShield ();
		}
		else
		{
			health -= 1;

			healthSlider.value = health; // Sets UI health bar to current health

			if (health == 0) {
				GameOver ();
			}
		}
	}
		

	// Resets health variable back to the starting value
	// Called in start() and during level ups
	public void ResetHealth()
	{
		health = healthStart;
		healthSlider.value = health;
	}

	// Enables the shield power up and draws the icon
	// Public because it is called in PowerUpBallController
	public void AddShield ()
	{
		isShielded = true;
		shieldIcon.SetActive (true);

	}


	// Disables shield icon and stops drawing the icon
	void RemoveShield()
	{
		isShielded = false;
		shieldIcon.SetActive (false);
	}


	// Called when the timer or health hits 0
	// Deletes ballz on screen then waits for a second before loading the game over screen with the user's score
	void GameOver()
	{
		gameOver = true; 	// Stops more ballz from spawning while waiting for game over scene
		ResetBallz();		


		PlayerPrefs.SetFloat ("Score", Mathf.Round(totalScore)); // Saves the score to a settings file for the game over screen
//		hs.SetScore(score);

//		4/25/18 Disabling TimeToAnswer code
//		PlayerPrefs.SetFloat ("TimeToAnswer", timeToAnswerTotal / timeToAnswerIncrement);

		StartCoroutine ("WaitForGameOver");		// Coroutine that will wait a little before loading game over screen, no waiting makes the transition too abrupt
	}


	// Waits a little after hitting game over state then loads the game over scene
	IEnumerator WaitForGameOver()
	{
		yield return new WaitForSeconds (2);	// Waits 2 seconds
	//	hs.SetScore(totalScore);
	//	Advertisement.Show();

		SceneManager.LoadScene (2);				// Loads Game over scene
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

		if (level < 5)
		{
			backGround.GetComponent<MeshRenderer> ().material = backgroundMaterials [level - 1];
		} else
		{
			backGround.GetComponent<MeshRenderer> ().material = backgroundMaterials [backgroundMaterials.Length - 1];
		}

		Invoke ("HideLevelTransition", levelStartDelay); // Invoke is a way to wait before calling a function 
	}

	// disables the level up transition card
	void HideLevelTransition()
	{
		levelTransition.SetActive (false);
		transitioningLevel = false; // allows ballz to spawn again
	}


	//4/25/18 disabling TTA code
/*	public void TimeToAnswer()
	{
		timeToAnswerTotal += Time.time - timeToAnswerInitial;
		timeToAnswerIncrement++;
	}
*/

	// Public function called by power up ball controller
	// Sets the timer and text for the double points power up affect
	public void EnabelDoublePoints()
	{
		doublePointsTimer = 10f;
		doublePointsText.text = "x2";
		isDoublePoints = true;
	}


	// Called once the double points timer hits 0
	void DisableDoublePoints()
	{
		doublePointsText.text = "";
		isDoublePoints = false;
	}


	public void EnableFreeze()
	{
		Debug.Log ("Enable Freeze");

		isFrozen = true;
		freezeTimer = 10;

		FreezeBallz ();
	}


	void FreezeBallz()
	{
		for (int x = 0; x < ballz.Length; x++) 
		{	
			if (ballzObjects [x] != null)
			{
				ballzObjects [x].gameObject.GetComponent<Rigidbody2D> ().mass = 10;
				ballzObjects [x].gameObject.GetComponent<BallController> ().SetFrozenIcon (true);
			}
			
		}
	}


	void DisableFreeze()
	{
		Debug.Log ("Disable Freeze");

		isFrozen = false;

		for (int x = 0; x < ballz.Length; x++) 
		{	
			if (ballzObjects [x] != null)
			{
				ballzObjects [x].gameObject.GetComponent<Rigidbody2D> ().mass = 1;
				ballzObjects [x].gameObject.GetComponent<BallController> ().SetFrozenIcon (false);
			}
		}
	}


	public void AddBonusTime(int t)
	{
		bonusTime = t;
		bonusTimeSlider.maxValue = bonusTime;
		bonusTimeSlider.value = bonusTime;

		bonusTimeSlider.gameObject.SetActive (true);
	}


	void ShuffleArray<T>(T[] arr) 
	{
		for (int i = arr.Length - 1; i > 0; i--) 
		{
			int r = Random.Range(0, i + 1);
			T tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
	}


	// Used by the power ups when they spawn, assigns a random face sprite
//	public Sprite GetRandomFace()
//	{
//		return ballFaces[Random.Range(0, ballFaces.Length)];
//	}
}