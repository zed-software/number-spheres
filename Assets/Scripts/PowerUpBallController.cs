using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBallController : MonoBehaviour {

	public int speed = 225;
	[Tooltip("1 for healthball; 2 for shield")]
	public int powerUpID; 		// Set by the prefab of power up, 1 for health;

	private GameObject gameControllerObject;	// Used to get access to the GameController script and its public functions
	private Rigidbody2D rb;						// Will be set to the ballz rigidbody component
	private GameController gc;
	private bool isInBoundry;					// Used to check if the ball should react to collisions yet

	// Use this for initialization
	void Start () 
	{
		gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");// Finds the gamecontroller object
		gc = gameControllerObject.GetComponent<GameController> (); // sets it to out easy to use local variable

		rb = GetComponent<Rigidbody2D> ();	// Setting the ball rigidbody to our local variable

		isInBoundry = false;				// The gamecontroller should spawn power ups outside the boundary
		this.GetComponent<Collider2D> ().isTrigger = true;	// Sets the power up collider to a trigger, so that it will go through the boundry

		Push();			// Pushes power up towards the game center
	}
	
	// Update is called once per frame
	void Update () {
		
	}

// Input detection for if the ballz' collider has been clicked on
	void OnMouseDown()
	{
		BallTouched ();		
	}


	// Because power ups spawn outside the boundry, the push function here always pushes them towards the center of the screen
	void Push()
	{
		Vector3 centerScreen = Camera.main.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 10f)); // Finds the center of the screen
		Vector2 center = new Vector2 (centerScreen.x, centerScreen.y);							// Converts to a vector2

		Vector2 direction = center - (new Vector2(transform.position.x, transform.position.y)); // Subtracts centor coordinates from ball coordinates to find the direction of the push

		rb.AddForce(direction * speed); // Pushes the ball towards the center
	}


	// Fills the players health
	// Function needs to be changed when there are more power ups
	public void BallTouched()
	{
		switch (powerUpID)
		{
			case 1: // The health ball
				{
					HealthPowerUp ();
					break;
				}
			case 2: // The shield ball
				{
					ShieldPowerUp ();
					break;
				}
		}

		Destroy (this.gameObject);
	}


	// Called by the BallTouched function if this is a health power up
	// Calls game controller to refill player health to full
	public void HealthPowerUp ()
	{
		gc.ResetHealth ();
	}


	// Called by the BallTouched function if this is a Shield power up
	// Adds a 1 hit shield to the player health
	public void ShieldPowerUp ()
	{
		gc.AddShield ();
	}


	// Called when ballz hit other ballz or boundry colliders
	void OnCollisionEnter2D (Collision2D coll)
	{
//		Debug.Log(coll.gameObject.tag.ToString ());
//		if (coll.gameObject.CompareTag("Boundry") && !isInBoundry)
//		{
////			Debug.Log ("test");
//			Physics2D.IgnoreCollision(coll.collider, this.GetComponent<Collider2D>());
//			Push ();
//		}
//		else
//		{
			Vector2 direction = coll.contacts[0].point - new Vector2 (transform.position.x, transform.position.y); // Calculates angle between the collision point and the object
			direction = direction.normalized * -1; // Sets the magnitude of the angle to 1 and flips it

			// Sometimes the ballz spawn already colliding with another object and this function is called before Start() can set the rigidbody component, so this checks for that and avoids an error message
			if(rb == null)
				rb = GetComponent<Rigidbody2D> ();

			rb.AddForce (direction * speed); // Adds force in the new direction, pushing away our ball
//		}
	}


//	void OnCollisionExit2D (Collision2D coll)
//	{
//		if (coll.gameObject.CompareTag("Boundry") && !isInBoundry)
//		{
//			Debug.Log ("test");
//			this.GetComponent<Collider2D> ().isTrigger = false; 
////			Physics2D.IgnoreCollision(coll.collider, this.GetComponent<Collider2D>(), false);
//			isInBoundry = true;
//		}
//	}


	// When the ball detects it has stopped touching the boundary after passing into it, it stops being a trigger and collides off objects again
	void OnTriggerExit2D (Collider2D coll)
	{
		if (coll.gameObject.CompareTag("Boundry") && !isInBoundry)
		{
//			Debug.Log ("test");
			this.GetComponent<Collider2D> ().isTrigger = false; 
			isInBoundry = true;
		}
	}
}
