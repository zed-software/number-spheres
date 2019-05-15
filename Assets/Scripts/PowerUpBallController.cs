using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBallController : MonoBehaviour {

	public int speed = 225;
	[Tooltip("1 for healthball; 2 for shield; 3 for double points; 4 for freeze")]
	public int powerUpID; 		// Set by the prefab of power up, 1 for health; 2 for shield; 3 for double points;

	private GameObject gameControllerObject;	// Used to get access to the GameController script and its public functions
	private Rigidbody2D rb;						// Will be set to the ballz rigidbody component
	private GameController gc;
	private bool isInBoundry;					// Used to check if the ball should react to collisions yet

	public GameObject audio_PowerUpTouched;		// Audio that plays when powerup is touched
	public GameObject anime_PowerUpTouched;		// Animation that plays when powerup is touched

	// Use this for initialization
	void Start () 
	{
		gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");// Finds the gamecontroller object
		gc = gameControllerObject.GetComponent<GameController> (); // sets it to out easy to use local variable

		rb = GetComponent<Rigidbody2D> ();	// Setting the ball rigidbody to our local variable

		isInBoundry = false;				// The gamecontroller should spawn power ups outside the boundary
		this.GetComponent<Collider2D> ().isTrigger = true;	// Sets the power up collider to a trigger, so that it will go through the boundry

//		SetFace ();
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


	// Called when the ball is touched or clicked
	// What it does depends on the type of power up it is, checks the powerUpID
	public void BallTouched()
	{
		Explode ();

		switch (powerUpID)
		{
			case 1: // The health ball
				{
					gc.ResetHealth (); // Calls game controller to refill player health to full
					break;
				}
			case 2: // The shield ball
				{
					gc.AddShield (); // Adds a 1 hit shield to the player health
					break;
				}
			case 3: // Double points ball
				{
					gc.EnabelDoublePoints (); // Enables a 2x multiplier for correct answers for 10 seconds
					break;
				}
			case 4: // Freeze ball
				{
					gc.EnableFreeze ();
					break;
				}
			case 5: // Extra time ball
				{
					gc.AddBonusTime (10);
					break;
				}
		}

		Destroy (this.gameObject); // This will probably be changed when there are explosion effects for power ups
	}

	public void Explode ()
	{
		if (PlayerPrefs.GetInt ("isMuteSoundEffects") != 1)
			Instantiate (audio_PowerUpTouched, transform.position, transform.rotation);
//			particleExplosion.GetComponent<AudioSource> ().playOnAwake = false;
		
		Instantiate (anime_PowerUpTouched, transform.position, transform.rotation);

	}



//	void SetFace()
//	{
//		GetComponentInChildren<SpriteRenderer> ().sprite = gc.GetRandomFace ();
//	}
		

	// Called when ballz hit other ballz or boundry colliders
	void OnCollisionEnter2D (Collision2D coll)
	{
		if(isInBoundry)
		{
			Vector2 direction = coll.contacts[0].point - new Vector2 (transform.position.x, transform.position.y); // Calculates angle between the collision point and the object
			direction = direction.normalized * -1; // Sets the magnitude of the angle to 1 and flips it

	//		Below commented out because power up ball will always spawn outside of boundry and never colliding with another object
	//		// Sometimes the ballz spawn already colliding with another object and this function is called before Start() can set the rigidbody component, so this checks for that and avoids an error message
	//		if(rb == null)
	//			rb = GetComponent<Rigidbody2D> ();

			rb.AddForce (direction * speed); // Adds force in the new direction, pushing away our ball
		}

	}


	// When the ball detects it has stopped touching the boundary after passing into it, it stops being a trigger and collides off objects again
	void OnTriggerExit2D (Collider2D coll)
	{
		if (coll.gameObject.CompareTag("Boundry") && !isInBoundry)
		{
			isInBoundry = true; 
			this.GetComponent<Collider2D> ().isTrigger = false; 
		}
	}
}
