using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	private int answerValue;			// Variable holding the correct answer, is reset to 0 when the correct answer ball is clicked
	private int[] incorrectValues;		// Array of incorrect values
	private int [] allValues;			// Array of the correct and incorrect values, correct is the first value
	private GameController gc;			// Connects to the GameController script
	private int level;					// Used to generate a problem based on this variable
	private Vector2 answerRange;		// Vector 2 that will hold the range of possible answers for each level
	private int min = 2, max = 10; 		// The minimum and mazimum values for the problem variables



	// Use this for initialization
	void Start () 
	{
		gc = GetComponent<GameController> ();
	}



	// Update is called once per frame
	void Update () {
		
	}



	// Public function called by the GameController to set the level
	public void SetLevel (int l)
	{
		level = l;
	}



	// Public function called by the GameController to create a problem, depending on the level
	// Parameter is the size of answer values it will need to generate
	public void GenerateProblem(int length)
	{
		allValues = new int[length];
		answerValue = 0;
//		int min = 1, max = 10; // The minimum and mazimum values for the problem variables
		int num1 = 0, num2 = 0;// num3 = 0; // Set to 0 to aviod some error

		// Switch statement for the different level problem generations
		// Each case loops until the problems answer isn't 0
		// 0 is avoided because the default values of the int array with the incorrect values is 0, not null, 
		// and a loop below uses the 0 to know if the spot on the array has been filled yet
		switch (level) 
		{
			case 1: // Addition
			{
				while (answerValue == 0)
				{	
					num1 = Random.Range (min, max);
					num2 = Random.Range (min, max);
		
					answerValue = num1 + num2;
					answerRange = new Vector2 ((min + min), (max + max));
					gc.UpdateProblem (num1 + " + " + num2 + " =");
				}

				break;
			}
			case 2: // Subtraction
			{
				while (answerValue == 0)
				{	
					num1 = Random.Range (min, max);
					num2 = Random.Range (min, max);

					answerValue = num1 - num2;
					answerRange = new Vector2 ((min - max), (max - min));
					gc.UpdateProblem (num1 + " - " + num2 + " =");
				}

				break;
			}
			case 3: // Multiplication
			{
				int maxMultiply = 15;

				if ( max < maxMultiply ) 
					maxMultiply = max ;

				while (answerValue == 0)
				{	
					num1 = Random.Range (min, maxMultiply);
					num2 = Random.Range (min, maxMultiply);

					answerValue = num1 * num2;
					answerRange = new Vector2 ((min * min), (maxMultiply * maxMultiply));
					gc.UpdateProblem (num1 + " * " + num2 + " =");
				}

				break;
			}
			case 4: // Division
			{
				while (answerValue == 0)
				{	
					num1 = Random.Range (min, max); // This number will be used as the answer
					num2 = Random.Range (min, max);	// This will be used as the denominator

					int multipliedVariable = num1 * num2; // Multiplies the 2 randomly generated numbers, this will be used as the numerator

					answerValue = num1;
					answerRange = new Vector2 (min, max);
					gc.UpdateProblem (multipliedVariable + " / " + num2 + " =");

					// Addition and subtraction code
//					num1 = Random.Range (min, max);
//					num2 = Random.Range (min, max);
//					num3 = Random.Range (min, max);
//
//					answerValue = num1 + num2 - num3;
//					answerRange = new Vector2 ((min + min - max), (max + max - min));
//					gc.UpdateProblem (num1 + " + " + num2 + " - " + num3 + " =");
				}
				break;
			}
//			case 5: // ALL
//			{
//				while (answerValue == 0)
//				{	
//
//
//					// Addition and multiplication code
////					num1 = Random.Range (min, max);
////					num2 = Random.Range (min, max);
////					num3 = Random.Range (min, max);
////
////					answerValue = num1 + num2 * num3;
////					answerRange = new Vector2 ((min + min * min), (max + max * max));
////					gc.UpdateProblem (num1 + " + " + num2 + " * " + num3 + " =");
//				}
//				break;
//			}
		}

		allValues [0] = answerValue; // Sets the first value of the array to the correct answer generated here

		GenerateIncorrectValues (length - 1); // Generates all the incorrect values
	}



	// This function could use work, feels weirdly complicated for right now
	// The function fills out the incorrectValues array with wrong answers
	// It avoids repeating wrong answers and makes sure it doesn't randomly generate the right answer
	// Parameter is how many incorrect it will generate
	void GenerateIncorrectValues(int length)
	{
		incorrectValues = new int[length];
		int random = 0;	// I think this was to avoid some error but I dont even think I need it anymore

		// First loop runs through every spot of the array
		for (int x = 0; x < incorrectValues.Length; x ++)
		{
			// Second loop will run until the default value in the array slot is changed from 0 and also isn't the correct answer
			while ( (incorrectValues[x] == 0) || (incorrectValues[x] == answerValue)) 
			{
				bool repeatValueCheck = false; // Boolean used to check if random number generated has already been used in the array

				// Third loop will run until the fourth loop nested inside runs all the way through without breaking (failing a repetition check)
				while (!repeatValueCheck) 
				{
					// New random number is generated at the begining of the loop and whenever the next for loop breaks
					random = Random.Range ((int)answerRange.x, (int)answerRange.y); // 

					// Fourth loop runs through the incorrectValues array and checks if random value generated for this slot has been used before
					for (int y = 0; y < incorrectValues.Length; y++) 
					{
						// If there is a match, the repeatValueCheck is set to false and the loop breaks back to the third loop 
						if (random == incorrectValues [y]) 
						{
							repeatValueCheck = false;
							break;
						}

						repeatValueCheck = true; // If the fourth loop doesn't break, it ends with repeatValueCheck on true and the third loop also ends
					}
				}

				incorrectValues[x] = random; // array slot is set to the random value thats been checked for repetition, second loop will end if this value isn't the same as the correct answer
			}

			//			Debug.Log (incorrectValues[x]);
		}
	}



	// Public function that returns an array of ints that hold all the ball values generated, the first value is 0
	// The rest of the values are then set to the incorrect values in our other array
	// Called by GameController when it spawns the ballz
	public int[] GetValues()
	{
//		int [] allValues = new int[incorrectValues.Length + 1];

		for (int x = 1; x < allValues.Length; x++) 
		{
			allValues [x] = incorrectValues [x - 1];
		}

		return allValues;
	}


	// Public function called by the GameController when there is a level up past level 4
	// Raises the maximum range of values generated for the problem and answers
	public void RaiseMaxProblemValues(int raise)
	{
		max += raise;
	}
}
