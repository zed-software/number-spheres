using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	private int answerValue;			// Variable holding the correct answer, is reset to 0 when the correct answer ball is clicked
	private int[] incorrectValues;		// Array of incorrect values
	private int [] allValues;			// Array of the correct and incorrect values, correct is the first value
	private GameController gc;			// Connects to the GameController script
	private int level;					// Used to generate a problem based on this variable
	private int challenge = 7;
	private Vector2 answerRange;		// Vector 2 that will hold the range of possible answers for each level
	public static int levelSwitch;
	private int additionAnswered, subtractionAnswered, multiplicationAnswered, divisionAnswered;

	private int addMin = 2, addMax = 10; 		// The minimum and mazimum values for the addition problem variables
	private int subMin = 2, subMax = 12; 		// The minimum and mazimum values for the subtraction problem variables
	private int mulMin = 2, mulMax = 12; 		// The minimum and mazimum values for the multiplication problem variables
	private int divMin = 2, divMax = 11; 		// The minimum and mazimum values for the division problem variables


	// Use this for initialization
	void Start () 
	{
		additionAnswered = 0; 
		subtractionAnswered = 0; 
		multiplicationAnswered = 0; 
		divisionAnswered = 0;
		levelSwitch = 0;
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
	// Parameter is the length of answer values it will need to generate
	public void GenerateProblem(int length)
	{
		allValues = new int[length];
		answerValue = 0;

//		levelSwitch = level;
//		int min = 1, max = 10; // The minimum and mazimum values for the problem variables
		int num1 = 0, num2 = 0;// num3 = 0; // Set to 0 to aviod some error

		levelSwitch = level % 5;
		if (level % 5 == 0) // If level 5 is reached, a random level is picked from the switch statement below
		{
//			if (level == challenge)
//			{
//				levelSwitch = 5;
//			}
//			else
				levelSwitch = Random.Range (1, 5); // 5 as the max random range becuase max is apparantly exclusive in Random.Range for ints
		}



		// Switch statement for the different level problem generations
		// Each case loops until the problems answer isn't 0
		// 0 is avoided because the default values of the int array with the incorrect values is 0, not null, 
		// and a loop below uses the 0 to know if the spot on the array has been filled yet
		switch (levelSwitch) 
		{
			case 1: // Addition
			{
				while (answerValue == 0)
				{	
//					min = 2;
//					max = 10;

					num1 = Random.Range (addMin, addMax);
					num2 = Random.Range (addMin, addMax);
	
					answerValue = num1 + num2;
					answerRange = new Vector2 ((addMin + addMin), (addMax + addMax));

					gc.UpdateProblem (num1 + " + " + num2 + " =");
					additionAnswered++;

					if (additionAnswered % 5 == 0) 
					{		
						addMin += (additionAnswered - 4);
						addMax += (additionAnswered - 2);
					}

//					if (additionAnswered >= 5) {
//						if (additionAnswered % 5 == 0) {		
//							min += (additionAnswered - 4);
//							max += (additionAnswered - 4);
//						}
//							
//						num1 = Random.Range (min, max);
//						num2 = Random.Range (min, max);
//
//						answerValue = num1 + num2;
//						answerRange = new Vector2 ((min + min), (max + max));
//						gc.UpdateProblem (num1 + " + " + num2 + " =");
//						additionAnswered++;
//					}

//					Debug.Log ("additionAnswered = " + additionAnswered);
//					Debug.Log ("Min = " + addMin + " Max = " + addMax);

				}

				break;
			}
			case 2: // Subtraction
			{
//				subMin = 2;
//				subMax = 12;
				while (answerValue == 0)
				{	
					num1 = Random.Range (subMin, subMax);
					num2 = Random.Range (subMin, subMax);

					if (level >= 5) // Allows negatives for grab bag levels
					{
						if (subtractionAnswered % 5 == 0) {		
							subMin += (subtractionAnswered - 4);
							subMax += (subtractionAnswered - 4);
						}

						answerValue = num1 - num2;
						answerRange = new Vector2 ((subMin - subMax), (subMax - subMin));

						gc.UpdateProblem (num1 + " - " + num2 + " =");
						subtractionAnswered++;
					} 
					else
					{
						if (num1 >= num2)
						{
							answerValue = num1 - num2;
							gc.UpdateProblem (num1 + " - " + num2 + " =");
							subtractionAnswered++;
						} else if (num1 < num2)
						{
							answerValue = num2 - num1;
							gc.UpdateProblem (num2 + " - " + num1 + " =");
							subtractionAnswered++;
						}

						answerRange = new Vector2 (subMin, (subMax - subMin));
					}
				}

				break;
			}
			case 3: // Multiplication
			{
				if (multiplicationAnswered < 5) {
//					mulMin = 2;
//					mulMax = 12;
					while (answerValue == 0) {
						num1 = Random.Range (mulMin, mulMax);
						num2 = Random.Range (2, 5);

						answerValue = num1 * num2;
						answerRange = new Vector2 ((mulMin * mulMin), (mulMax * mulMax));
						gc.UpdateProblem (num1 + " * " + num2 + " =");
					}
					multiplicationAnswered++;
				}

				if (multiplicationAnswered >= 5) {
					mulMin = 2 + (multiplicationAnswered / 5);
					mulMax = 12 + (multiplicationAnswered / 5);
					int maxMultiply = 5 + (multiplicationAnswered / 5);
					while (answerValue == 0) {
						num1 = Random.Range (mulMin, mulMax);
						num2 = Random.Range (2, maxMultiply);

						answerValue = num1 * num2;
						answerRange = new Vector2 ((mulMin * mulMin), (mulMax * mulMax));
						gc.UpdateProblem (num1 + " * " + num2 + " =");
					}
							multiplicationAnswered++;
				}
//				int maxMultiply = 15;
//
//				if ( max < maxMultiply ) 
//					maxMultiply = max ;
//
//				while (answerValue == 0)
//				{	
//					num1 = Random.Range (min, maxMultiply);
//					num2 = Random.Range (min, maxMultiply);
//
//					answerValue = num1 * num2;
//					answerRange = new Vector2 ((min * min), (maxMultiply * maxMultiply));
//					gc.UpdateProblem (num1 + " * " + num2 + " =");
//				}

				break;
			}
			case 4: // Division
			{
//				divMin = 2;
//				divMax = 11;
				int maxDenom = 5; 
				int minDenom = 2;
				while (answerValue == 0)
				{	
					if (divisionAnswered % 5 == 0) {
						maxDenom += (divisionAnswered / 5);
						divMax+= (divisionAnswered / 5);
						divMin += (divisionAnswered / 5);
					}
					num1 = Random.Range (divMin, divMax); // This number will be used as the answer
					num2 = Random.Range (minDenom, maxDenom);	// This will be used as the denominator

					int multipliedVariable = num1 * num2; // Multiplies the 2 randomly generated numbers, this will be used as the numerator

					answerValue = num1;
					answerRange = new Vector2 (divMin, divMax);
					gc.UpdateProblem (multipliedVariable + " / " + num2 + " =");
					divisionAnswered++;

						

					// Old level 4 addition and subtraction code
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
				bool repeatValueCheck = false;	// Boolean used to check if random number generated has already been used in the array

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
		for (int x = 1; x < allValues.Length; x++) 
		{
			allValues [x] = incorrectValues [x - 1];
		}

		return allValues;
	}


	// Public function called by the GameController when there is a level up past level 4
	// Raises the maximum range of values generated for the problem and answers
//	public void RaiseMaxProblemValues(int raise)
//	{
//		max += raise;
//	}
}
