using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

	public int speed; 							// Speed of ballz
	public GameObject explosion;				// The explosion prefab goes here
	public int score;							// Score value for the ball

	private GameObject gameControllerObject;	// Used to get access to the GameController script and its public functions
	private Rigidbody2D rb;						// Will be set to the ballz rigidbody component
	private GameController gc;					// Used to easily call the gameController once its set
	private TextMesh tm;						// Number text on top of the mathball
	private int value;							// The assigned value of the mathball
	private int isCorrect;						// 0 if incorrect value was assigned, 1 if correct


	void Start () 
	{
		gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");// Finds the gamecontroller object
		gc = gameControllerObject.GetComponent<GameController> (); // sets it to out easy to use local variable

		tm = GetComponentInChildren<TextMesh> (); // Setting the number text to out local variable

		rb = GetComponent<Rigidbody2D> ();	// Setting the ball rigidbody to out local variable

		SetValue (); // Gets number value assigned to it from the gamecontroller 
		Push (); // Pushes the ball in a random direction

	}



	// Update is called once per frame
	void Update () 
	{
		
	}


	// Called when ballz hit other ballz or boundry colliders
	void OnCollisionEnter2D (Collision2D coll)
	{

		Vector2 direction = coll.contacts[0].point - new Vector2 (transform.position.x, transform.position.y); // Calculates angle between the collision point and the object
		direction = direction.normalized * -1; // Sets the magnitude of the angle to 1 and flips it

		// Sometimes the ballz spawn already colliding with another object and this function is called before Start() can set the rigidbody component, so this checks for that and avoids an error message
		if(rb == null)
			rb = GetComponent<Rigidbody2D> ();

		rb.AddForce (direction * speed); // Adds force in the new direction, pushing away our ball

	}


	// Pushes the ball in a random direction
	void Push()
	{
		int range = 100; // Nice even number
		Vector2 randomVector = new Vector2 (Random.Range(-range, range), Random.Range(-range, range)); // randomVector is used to pick a direction for the initial force on the ball
		randomVector.Normalize (); // Sets the x and y values to a magnitude of 1

		rb.AddForce (randomVector * speed); // Pushes the ball in a random direction
	}
		

	// Calls the game controller to get assigned its correct or incorrect value
	void SetValue()
	{
		int[] valueArray; // needed cause GetBallValue() returns an array with 2 slots, 1st slot has the number value, second slot tells if its the correct answer to the problem 
		valueArray = gc.GetBallValue (); // Gets the value assignment from gamecontroller

		value = valueArray [0];	
		isCorrect = valueArray [1];

		tm.text = value.ToString(); // Sets the text on the ball to its assigned value
	}
		

	// Input detection for if the ballz' collider has been clicked on
	void OnMouseDown()
	{
		Instantiate (explosion, transform.position, transform.rotation); // Spawns the explosion particle effect when clicked

		if (isCorrect == 1) // If its the correct ball
		{
			gc.AddProgress();		// Counts up how many times a correct ball has been clicked, called before the score is added so the speedbonus can be calculated
 			gc.AddScore (score); 	// The score value is multiplied by the combo and speed here
			gc.AddCombo ();			// Counts up the amount of correct balls clicked in a row, called afer the score is added so the new multiplier isn't factored in
			gc.ResetBallz (); 		// If this is the correct mathball, the gamecontroller will reset the game
		}
		else
		{
			gc.ResetCombo ();			// Resets correct answer combo multiplier to 1x
//			gc.AddScore (-score);		// If incorrect the score goes down by this value
			Destroy (this.gameObject); 	// If this is an incorrect mathball it just gets destroyed when pressed
			gc.LoseHealth();
		}
	}
		
}
