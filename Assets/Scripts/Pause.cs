using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

	bool paused = false;

	public GameObject PausePanel;

//	void Update()
//	{
//		if(Input.GetButtonDown("pauseButton"))
//			paused = togglePause();
//	}
//
//	void OnGUI()
//	{
//		if(paused)
//		{
//			GUILayout.Label("Game is paused!");
//			if(GUILayout.Button("Click me to unpause"))
//				paused = togglePause();
//		}
//	}

	public void TogglePause()
	{
//		Debug.Log ("PAUSE TEST");

		if(paused)
		{
			PausePanel.SetActive (false);
			Time.timeScale = 1f;
			paused = false;
			//return(false);
		}
		else
		{
			PausePanel.SetActive (true);
			Time.timeScale = 0f;
			paused = true;
			//return(true);    
		}
	}
}
