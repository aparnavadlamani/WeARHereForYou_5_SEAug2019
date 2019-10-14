using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

//Custom 8
public partial class Wit3D : MonoBehaviour
{

    public Text myHandleTextBox;
    private bool actionFound = false;
    public AudioSource _audio;

    void Handle(string jsonString)
    {

        if (jsonString != null)
        {

            RootObject theAction = new RootObject();
            Newtonsoft.Json.JsonConvert.PopulateObject(jsonString, theAction);

            if (theAction.entities.greetings != null)
            {
                foreach (Hello aPart in theAction.entities.greetings)
                {
                    Debug.Log(aPart.value);
                    myHandleTextBox.text = aPart.value;
                    _audio = gameObject.GetComponent<AudioSource>();
                    StartCoroutine(DownloadTheAudio("Hi"));
                    actionFound = true;
                }
            }
            if (theAction.entities.bye != null)
            {
                foreach (Bye aPart in theAction.entities.bye)
                {
                    Debug.Log(aPart.value);
                    myHandleTextBox.text = aPart.value;
                    _audio = gameObject.GetComponent<AudioSource>();
                    StartCoroutine(DownloadTheAudio("Bye"));
                    actionFound = true;
                }
            }

            if (!actionFound)
            {
                myHandleTextBox.text = "Request unknown, please ask a different way.";
            }
            else
            {
                actionFound = false;
            }

        }//END OF IF

    }//END OF HANDLE VOID
    IEnumerator DownloadTheAudio(string s)
    {
        if(s=="Hi")
        {
            string url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=HeyThere&tl=En-gb";
            WWW www = new WWW(url);
            yield return www;

            _audio.clip = www.GetAudioClip(false, true, AudioType.MPEG);
            _audio.Play();
        }
        if(s=="Bye")
        {
            string url = "https://translate.google.com/translate_tts?ie=UTF-8&total=1&idx=0&textlen=32&client=tw-ob&q=SeeYouSoon&tl=En-gb";
            WWW www = new WWW(url);
            yield return www;

            _audio.clip = www.GetAudioClip(false, true, AudioType.MPEG);
            _audio.Play();
        }
    }

}//END OF CLASS


//Custom 9
public class Hello
{
    public bool suggested { get; set; }
    public double confidence { get; set; }
    public string value { get; set; }
}

public class Bye
{
    public bool suggested { get; set; }
    public double confidence { get; set; }
    public string value { get; set; }
}

public class Entities
{
    public List<Hello> greetings { get; set; }
    public List<Bye> bye { get; set; }
}

public class RootObject
{
    public string _text { get; set; }
    public Entities entities { get; set; }
    public string msg_id { get; set; }
}