using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour  
{
	private float score;
	private float[] highScoreList = new float[6];


	public void loadScores ()
	{
		//Loads in High scores to class array. If no score found, sets to 0
		if (PlayerPrefs.HasKey ( "HS00" ))
			highScoreList [0] = PlayerPrefs.GetFloat ( "HS00" );
		else
		{
			PlayerPrefs.SetFloat ( "HS00" , 0);
			highScoreList[0] = 0;
		}

		if (PlayerPrefs.HasKey ( "HS01" ))
			highScoreList [1] = PlayerPrefs.GetFloat ( "HS01" );
		else
		{
			PlayerPrefs.SetFloat ( "HS01" , 0);
			highScoreList[1] = 0;
		}

		if (PlayerPrefs.HasKey ( "HS02" ))
			highScoreList [2] = PlayerPrefs.GetFloat ( "HS02" );
		else
		{
			PlayerPrefs.SetFloat ( "HS02" , 0);
			highScoreList[2] = 0;
		}

		if (PlayerPrefs.HasKey ( "HS03" ))
			highScoreList [3] = PlayerPrefs.GetFloat ( "HS03" );
		else
		{
			PlayerPrefs.SetFloat ( "HS03" , 0);
			highScoreList[3] = 0;
		}

		if (PlayerPrefs.HasKey ( "HS04" ))
			highScoreList [4] = PlayerPrefs.GetFloat ( "HS04" );
		else
		{
			PlayerPrefs.SetFloat ( "HS04" , 0);
			highScoreList[4] = 0;
		}
		PlayerPrefs.SetFloat ( "HS05" , 0);
		highScoreList [5] = 0;
	}

	public void SetScore( float currentScore )
	{
		loadScores ();
		score = currentScore;
		UpdateHighScore ();
		SaveScores ();
		ShowScores ();
	}

	public void UpdateHighScore ()
	{
		float tempValue, tempValue2;
		for (int x = 0; x < 5; x++) {
			if (score > highScoreList [x]) {
				//insert sort logic here. assign to x and push rest down.
				tempValue = highScoreList[x+1];
				highScoreList [x + 1] = highScoreList [x];
				highScoreList [x] = score;
				while (x < 4) {
					x++;
					tempValue2 = highScoreList [x + 1];
					highScoreList [x + 1] = tempValue;
					tempValue = tempValue2;
				}
				break;

			}
		}
	}

	private void SaveScores ()
	{
		PlayerPrefs.SetFloat ("HS00", highScoreList [0]);
		PlayerPrefs.SetFloat ("HS01", highScoreList [1]);
		PlayerPrefs.SetFloat ("HS02", highScoreList [2]);
		PlayerPrefs.SetFloat ("HS03", highScoreList [3]);
		PlayerPrefs.SetFloat ("HS04", highScoreList [4]);
	}

	private void ShowScores()
	{
		for(int x = 0; x < 5; x++)
			Debug.Log(highScoreList[x]);
	}


}

//HS00, HS01, HS02, HS03, HS04
