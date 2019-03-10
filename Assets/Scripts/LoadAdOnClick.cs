//Disabled ads 2/11/19


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Advertisements;
//
//public class LoadAdOnClick : MonoBehaviour 
//{
////	public void LoadAd()
////	{
////		Advertisement.Show ();
////	}
//
//	public LoadSceneOnClick ls;
//
//	public void ShowRewardedVideo ()
//	{
//		if (Advertisement.IsReady ("video")) 
//		{
//			ShowOptions options = new ShowOptions ();
//			options.resultCallback = HandleShowResult;
//
//			//Advertisement.Show("rewardedVideo", options);
//			Advertisement.Show ("video", options);
//		}
//	}
//
//	void HandleShowResult (ShowResult result)
//	{
//		if(result == ShowResult.Finished) {
//			Debug.Log("Video completed - Offer a reward to the player");
//			if (ls != null)
//				ls.LoadByIndex (1);
//
//		}else if(result == ShowResult.Skipped) {
//			Debug.LogWarning("Video was skipped - Do NOT reward the player");
//			if (ls != null)
//				ls.LoadByIndex (1);
//
//		}else if(result == ShowResult.Failed) {
//			Debug.LogError("Video failed to show");
//		}
//	}
//
//}
