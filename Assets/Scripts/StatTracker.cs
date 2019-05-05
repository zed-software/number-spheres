using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class StatTracker : MonoBehaviour {

	private int streak;
	private int highStreak;

	float[] timeToAnswer;
	float[] timeToAnswerAddition;
	float[] timeToAnswerSubtraction;
	float[] timeToAnswerMultiplication;
	float[] timeToAnswerDivision;

	float percentCorrectOverall;
	float percentCorrectAddition;
	float percentCorrectSubtraction;
	float percentCorrectMultiplication;
	float percentCorrectDivision;

	private int totalAnswers;
	private int totalQuestions;

	int totalQuestionsAddition;
	int totalQuestionsSubtraction;
	int totalQuestionsMultiplication;
	int totalQuestionsDivision;

	private float averageSessionTime;
	private float averageSessionTimeAddition;
	private float averageSessionTimeSubtraction;
	private float averageSessionTimeMultiplication;
	private float averageSessionTimeDivision;

	private int totalAnswersAddition;
	private int totalAnswersSubtraction;
	private int totalAnswersMultiplication;
	private int totalAnswersDivision;
	private int lastOperator; //keeps track of the operator of the last question answered. helps to sort stats by operator.

	// Use this for initialization
	void Start () 
	{
		totalAnswers = 0;
		totalAnswersAddition = 0;
		totalAnswersSubtraction = 0;
		totalAnswersMultiplication = 0;
		totalAnswersDivision = 0;
		averageSessionTime = 0;
		averageSessionTimeAddition = 0;
		averageSessionTimeSubtraction = 0;
		averageSessionTimeMultiplication = 0;
		averageSessionTimeDivision = 0;
		totalAnswers = 0;
		totalQuestions = 0;
		streak = 0;
		highStreak = 0;
		totalQuestionsAddition = 0;
		totalQuestionsSubtraction = 0;
		totalQuestionsMultiplication = 0;
		totalQuestionsDivision = 0;
		timeToAnswer = new float[100]; //needs to be fixed. you can't dynamically resize arrays in c#. This will break the game if you get to question 101.
		timeToAnswerAddition = new float[100];
		timeToAnswerSubtraction = new float[100];
		timeToAnswerMultiplication = new float[100];
		timeToAnswerDivision = new float[100];
		percentCorrectOverall = 0.0f;
		percentCorrectAddition = 0.0f;
		percentCorrectSubtraction = 0.0f;
		percentCorrectMultiplication = 0.0f;
		percentCorrectDivision = 0.0f;
		if (PlayerPrefs.HasKey ( "highStreak" ))
			highStreak = PlayerPrefs.GetInt ( "highStreak" );
		else
		{
			PlayerPrefs.SetInt ( "highStreak" , 0);
			highStreak = 0;
		}
		GameController.goodTouch += correctAnswer;
		GameController.badTouch += incorrectAnswer;
		GameController.gameOverTouch += gameOver;
	}

	void GameController_gameOverTouch ()
	{
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//This handles everything that happens when a correct answer is pressed
	void correctAnswer ()
	{
		lastOperator = LevelController.levelSwitch; //matches to the variable in levelController. This is sloppy with dependencies probably.
		addStreak ();
		addTimeToAnswer ();
		totalAnswers += 1;
		totalQuestions += 1;
		if (lastOperator == 1) {
			totalQuestionsAddition++;
			totalAnswersAddition++;
		}
		if (lastOperator == 2) {
			totalQuestionsSubtraction++;
			totalAnswersSubtraction++;
		}
		if (lastOperator == 3) {
			totalQuestionsMultiplication++;
			totalAnswersMultiplication++;
		}
		if (lastOperator == 4) {
			totalQuestionsDivision++;
			totalAnswersDivision++;
		}
		Debug.Log ("Current Streak: " + streak);
		Debug.Log ("Time to Answer: " + timeToAnswer [totalQuestions - 1] + "s");
	}

	//this handles everything that happens when an incorrect answer is pressed
	void incorrectAnswer ()
	{
		lastOperator = LevelController.levelSwitch;
		resetStreak ();
		Debug.Log (streak);
		totalAnswers += 1;
		if (lastOperator == 1) {
			totalAnswersAddition++;
		}
		if (lastOperator == 2) {
			totalAnswersSubtraction++;
		}
		if (lastOperator == 3) {
			totalAnswersMultiplication++;
		}
		if (lastOperator == 4) {
			totalAnswersDivision++;
		}
	}

	//adds to your current streak
	void addStreak()
	{
		streak++;
	}

	//resets your streak and records it if it is a new high streak
	void resetStreak()
	{
		if (streak > highStreak)
		{
			highStreak = streak;
			PlayerPrefs.SetInt ("highStreak", highStreak);
			Debug.Log ("New high streak! " + highStreak);
		}
		streak = 0;
	}

	//adds the time it took to answer the current question to an array
	void addTimeToAnswer ()
	{
		if (totalQuestions == timeToAnswer.Length) {
			System.Array.Resize (ref timeToAnswer, timeToAnswer.Length + 100);
		}
		timeToAnswer[totalQuestions] = GameController.statTimer;

		//switch statement that determines the operator of the question that was just answered, and handles the time-to-answer stats appropriately by operator
		switch (lastOperator) {
			case 1:
			{
				if (totalQuestionsAddition == timeToAnswerAddition.Length)									//This if statement is to resize the array of session data if the array fills up. Fuck c# amirite?
					System.Array.Resize (ref timeToAnswerAddition, timeToAnswerAddition.Length + 100);
				timeToAnswerAddition [totalQuestionsAddition] = GameController.statTimer;
				break;
			}

			case 2:
			{
				if (totalQuestionsSubtraction == timeToAnswerSubtraction.Length)
					System.Array.Resize (ref timeToAnswerSubtraction, timeToAnswerSubtraction.Length + 100);
				timeToAnswerSubtraction [totalQuestionsSubtraction] = GameController.statTimer;
				break;
			}

			case 3:
			{
				if (totalQuestionsMultiplication == timeToAnswerMultiplication.Length)
					System.Array.Resize (ref timeToAnswerMultiplication, timeToAnswerMultiplication.Length + 100);
				timeToAnswerMultiplication [totalQuestionsMultiplication] = GameController.statTimer;
				break;
			}

			case 4:
			{
				if (totalQuestionsDivision == timeToAnswerDivision.Length)
					System.Array.Resize (ref timeToAnswerDivision, timeToAnswerDivision.Length + 100);
				timeToAnswerDivision [totalQuestionsDivision] = GameController.statTimer;
				break;
			}
		}
	}

	//handles everything that happens when a game over state is reached
	void gameOver()
	{
		displayAverageTimeToAnswer ();
		displayHighestStreak ();
		percentageCorrect ();
	}

	//calculates the average time to answer a question correctly for a gameplay session.
	void displayAverageTimeToAnswer()
	{
		//sets float variables to the calculated average time.
		averageSessionTime = averageTimeToAnswer (timeToAnswer, totalQuestions);
		averageSessionTimeAddition = averageTimeToAnswer (timeToAnswerAddition, totalQuestionsAddition);
		averageSessionTimeSubtraction = averageTimeToAnswer (timeToAnswerSubtraction, totalQuestionsSubtraction);
		averageSessionTimeMultiplication = averageTimeToAnswer (timeToAnswerMultiplication, totalQuestionsMultiplication);
		averageSessionTimeDivision = averageTimeToAnswer (timeToAnswerDivision, totalQuestionsDivision);



		Debug.Log ("Average time to answer all questions: " + averageSessionTime.ToString("0.00") + "s");
		Debug.Log ("Average time to answer addition questions: " + averageSessionTimeAddition.ToString("0.00") + "s");
		Debug.Log ("Average time to answer: subtraction questions" + averageSessionTimeSubtraction.ToString("0.00") + "s");
		Debug.Log ("Average time to answer multiplication questions: " + averageSessionTimeMultiplication.ToString("0.00") + "s");
		Debug.Log ("Average time to answer division questions: " + averageSessionTimeDivision.ToString("0.00") + "s");
	}

	//Displays your highest streak in the console. Called at Game Over
	void displayHighestStreak ()
	{
		Debug.Log ("Your high streak is: " + highStreak + " correct answers");
	}

	//Calculates and displays your percentage for correct answers out of total answers. Could be refactored.
	void percentageCorrect()
	{
		percentCorrectOverall = totalQuestions*100 / totalAnswers;
		if(totalAnswersAddition != 0)
			percentCorrectAddition = totalQuestionsAddition*100 / totalAnswersAddition;
		if (totalAnswersSubtraction  != 0)
			percentCorrectSubtraction = totalQuestionsSubtraction*100 / totalAnswersSubtraction;
		if (totalAnswersMultiplication != 0)
			percentCorrectMultiplication = totalQuestionsMultiplication*100 / totalAnswersMultiplication;
		if (totalAnswersDivision != 0)
			percentCorrectDivision = totalQuestionsDivision*100 / totalAnswersDivision;
		Debug.Log ("Percentage of all answers correct: " + percentCorrectOverall + "%");
		Debug.Log ("Percentage of addition answers correct: " + percentCorrectAddition + "%");
		Debug.Log ("Percentage of subtraction answers correct: " + percentCorrectSubtraction + "%");
		Debug.Log ("Percentage of multiplication answers correct: " + percentCorrectMultiplication + "%");
		Debug.Log ("Percentage of division answers correct: " + percentCorrectDivision + "%");
	}

	float averageTimeToAnswer(float[] times, int questionCount)
	{
		float average = 0.0f;
		for (int x = 0; x < questionCount; x++)
			average += times [x];
			
		return average / questionCount;
	}
}
