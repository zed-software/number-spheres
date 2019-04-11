using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * This class is used to control the main menu and whatever options it will have
 **/
public class MenuController : MonoBehaviour {

	public GameObject Music;
	public GameObject ClickAudio;
	public GameObject MusicToggleOnButton;
	public GameObject MusicToggleOffButton;
	public GameObject AudioToggleOnButton;
	public GameObject AudioToggleOffButton;

	private int isMuteMusic;
	private int isMuteSoundEffects;
	private float musicVolume;

	private AudioSource musicAudioSource;
	private AudioSource clickAudioSource;

	// Use this for initialization
	void Start () 
	{
		musicAudioSource = Music.GetComponent<AudioSource> ();
		musicVolume = musicAudioSource.volume;

		clickAudioSource = ClickAudio.GetComponent<AudioSource> ();


		if (PlayerPrefs.HasKey ("isMuteMusic"))
			isMuteMusic = PlayerPrefs.GetInt ("isMuteMusic");
		else
		{
			PlayerPrefs.SetInt ("isMuteMusic", 0);
			isMuteMusic = PlayerPrefs.GetInt ("isMuteMusic");
		}

		if (PlayerPrefs.HasKey ("isMuteSoundEffects"))
			isMuteSoundEffects = PlayerPrefs.GetInt ("isMuteSoundEffects");
		else
		{
			PlayerPrefs.SetInt ("isMuteSoundEffects", 0);
			isMuteSoundEffects = PlayerPrefs.GetInt ("isMuteSoundEffects");
		}


		if (isMuteMusic == 1) 
		{
			musicAudioSource.volume = 0;

			MusicToggleOffButton.SetActive (false);
			MusicToggleOnButton.SetActive (true);
		}

		if (isMuteSoundEffects == 1) 
		{
			clickAudioSource.volume = 0;

			AudioToggleOffButton.SetActive (false);
			AudioToggleOnButton.SetActive (true);
		}

	}

	public void MuteMusic()
	{
//		isMuteMusic = !isMuteMusic;

		isMuteMusic = 1;
		PlayerPrefs.SetInt ("isMuteMusic", isMuteMusic);

//		AudioListener.volume = 0;
		musicAudioSource.volume = 0;
	}

	public void UnMuteMusic()
	{
		isMuteMusic = 0;
		PlayerPrefs.SetInt ("isMuteMusic", isMuteMusic);

//		AudioListener.volume = 1;
		musicAudioSource.volume = musicVolume;
	}

	public void MuteSoundEffects()
	{
		isMuteSoundEffects = 1;
		PlayerPrefs.SetInt ("isMuteSoundEffects", isMuteSoundEffects);

		clickAudioSource.volume = 0;
	}

	public void UnMuteSoundEffects()
	{
		isMuteSoundEffects = 0;
		PlayerPrefs.SetInt ("isMuteSoundEffects", isMuteSoundEffects);

		clickAudioSource.volume = 1;
	}
}
