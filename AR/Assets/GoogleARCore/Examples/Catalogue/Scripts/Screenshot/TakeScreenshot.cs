using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour {

	[SerializeField]
	GameObject blink;

	public void TakeAShot()
	{
		StartCoroutine ("CaptureIt");
	}

	IEnumerator CaptureIt()
	{
		yield return null;
		GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;

		// Wait for screen rendering to complete
		//yield return new WaitForEndOfFrame();
		string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
		string fileName = "Screenshot" + timeStamp + ".png";
		string pathToSave = fileName;
		yield return new WaitForEndOfFrame();
		ScreenCapture.CaptureScreenshot(pathToSave);
		
		GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
		Instantiate (blink, new Vector2(0f, 0f), Quaternion.identity);
	}

}
