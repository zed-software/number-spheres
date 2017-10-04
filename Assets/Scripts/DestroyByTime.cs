using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour 
{
	public float lifetime; // Amount of time before the object is destroyed

	// Used for particles after they play
	void Start () 
	{
		Destroy (gameObject, lifetime);
	}
	

}
