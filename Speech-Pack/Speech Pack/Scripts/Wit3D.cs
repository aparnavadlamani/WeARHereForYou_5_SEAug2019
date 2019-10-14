using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.UI;
 
public partial class Wit3D : MonoBehaviour {

	// Class Variables

	// Audio variables
	public AudioClip commandClip;
	int samplerate;

	// API access parameters
	string url = "https://api.wit.ai/speech?v=20191006";
	string token = "UTCWG4GUARBITZGUNJMG565YUDW3I2KJ";

	//Custom 1
 	// GameObject to use as a default spawn point
 	private bool isRecording = false;
	private bool pressedButton = false;
	public Text myResultBox;
	
	// Use this for initialization
	void Start () {
		samplerate = 16000;
	}

	//Custom 2
	public void startStopRecord(){
		if (isRecording == true) {
			pressedButton = true;
			isRecording = false;
 		} else if (isRecording == false) {
			isRecording = true;
			pressedButton = true;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (pressedButton == true) {
			pressedButton = false;
			if (isRecording) {
				myResultBox.text = "Listening for command";
				commandClip = Microphone.Start (null, false, 5, samplerate);  //Start recording (rewriting older recordings)
			}

			//Custom 5
			if (!isRecording) {
				myResultBox.text = null;
				myResultBox.text = "Saving Voice Request";
				// Save the audio file
				Microphone.End (null);
				if (SavWav.Save ("sample", commandClip)) {
					myResultBox.text = "Sending audio to AI...";
				} else {
					myResultBox.text = "FAILED";
				}

				// At this point, we can delete the existing audio clip
				commandClip = null;

 				//Start a coroutine called "WaitForRequest" with that WWW variable passed in as an argument
				StartCoroutine(SendRequestToWitAi());

			}
		}

	}

 	public IEnumerator SendRequestToWitAi(){
		//Custom 6
		string file = Application.persistentDataPath + "/sample.wav";
 		string API_KEY = token;

		FileStream filestream = new FileStream (file, FileMode.Open, FileAccess.Read);
		BinaryReader filereader = new BinaryReader (filestream);
		byte[] postData = filereader.ReadBytes ((Int32)filestream.Length);
		filestream.Close ();
		filereader.Close ();

		//Custom 7
		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers["Content-Type"] = "audio/wav";
		headers["Authorization"] = "Bearer " + API_KEY;

		float timeSent = Time.time;
		WWW www = new WWW(url, postData, headers);
 		yield return www;

		while (!www.isDone) {
			myResultBox.text = "Thinking and deciding ...";
 			yield return null;
		}
		float duration = Time.time - timeSent;

		if (www.error != null && www.error.Length > 0) {
			UnityEngine.Debug.Log("Error: " + www.error + " (" + duration + " secs)");
			yield break;
		}
		UnityEngine.Debug.Log("Success (" + duration + " secs)");
		UnityEngine.Debug.Log("Result: " + www.text);
		Handle (www.text);

	}



}