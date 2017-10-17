using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBallController : MonoBehaviour {

	public int speed = 225;

	private GameObject gameControllerObject;	// Used to get access to the GameController script and its public functions
	private Rigidbody2D rb;						// Will be set to the ballz rigidbody component
	private GameController gc;

	// Use this for initialization
	void Start () 
	{
		gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");// Finds the gamecontroller object
		gc = gameControllerObject.GetComponent<GameController> (); // sets it to out easy to use local variable

		rb = GetComponent<Rigidbody2D> ();	// Setting the ball rigidbody to our local variable
		Push();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

// Input detection for if the ballz' collider has been clicked on
//	void OnMouseDown()
//	{
//		BallTouched ();		
//	}

	void Push()
	{
//		Vector2 originVector = new Vector2(-1, 0);
//
//		rb.AddForce(originVector * speed); // Pushes the ball in a random direction

		int range = 100; // Nice even number
		Vector2 randomVector = new Vector2 (Random.Range(-range, range), Random.Range(-range, range)); // randomVector is used to pick a direction for the initial force on the ball
		randomVector.Normalize (); // Sets the x and y values to a magnitude of 1

		rb.AddForce(randomVector * speed); // Pushes the ball in a random direction
	}

	public void BallTouched()
	{
		gc.ResetHealth ();
		Destroy (this.gameObject);
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
}
