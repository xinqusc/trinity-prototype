using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
public class SendToGoogle : MonoBehaviour
{


    private string URL_MX1 = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSf2kGtw3hJ3SFHG_DOrPxSWfZJdaQ1ZrBrBjZY0Yp2CHEV1zA/formResponse";

    private string URL_MX2 = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSdLztQlbfYbbxGSMqvbKeBdBDc8OHCthihR7YNa_XP8arGY7A/formResponse";

    private string URL_MX3 = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSc1mBiFRJaKCCfws0KNUB4DVJ16JldVVshjg3S_41j1xbB41w/formResponse";

    private string URL_MX4 = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeEzHgxjpqBoyEot6PUlrw3up1ISi4DLLavK2LIZPQSIri-IA/formResponse";

    private string URL_MX5 = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSf0TWlv15XbhbF5PsAkbB6y1YpulvTrGBAQcdO-ep-aAtTnvQ/formResponse";

    private long sessionID_MX1;
    private long sessionID_MX2;
    private long sessionID_MX3;
    private long sessionID_MX4;
    private long sessionID_MX5;



    private void Start()
    {
        // Assign sessionID to identify playtests
        sessionID_MX1 = DateTime.Now.Ticks;
        sessionID_MX2 = DateTime.Now.Ticks;
        sessionID_MX3 = DateTime.Now.Ticks;
        sessionID_MX4 = DateTime.Now.Ticks;
        sessionID_MX5 = DateTime.Now.Ticks;
    }
    //Matrix 1

    public void SendMatrix1(string levelName, string DamageType)
    {
        StartCoroutine(PostMatrix1(sessionID_MX1.ToString(), levelName.ToString(), DamageType.ToString()));
    }


    private IEnumerator PostMatrix1(string sessionID_MX1, string levelName, string DamageType)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1477553278", sessionID_MX1);
        form.AddField("entry.1570433801", levelName);//Level Name
        form.AddField("entry.916383983", DamageType);// type of the source of damage

        using (UnityWebRequest www = UnityWebRequest.Post(URL_MX1, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }


    //Matrix 2
    public void SendMatrix2(float TrainSpeed)
    {
        StartCoroutine(PostMatrix2(sessionID_MX2.ToString(), TrainSpeed.ToString()));
    }


    private IEnumerator PostMatrix2(string sessionID_MX2, string TrainSpeed)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.366340186", sessionID_MX2);
        form.AddField("entry.582082116", TrainSpeed);

        using (UnityWebRequest www = UnityWebRequest.Post(URL_MX2, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }


    //Matrix 3
    public void SendMatrix3(string LevelName, int TrainDash, int SuperCH_count, bool isTrainDash, bool isSuperCH)// change type as per data
    {
        StartCoroutine(PostMatrix3(sessionID_MX3.ToString(), LevelName.ToString(), TrainDash.ToString(), SuperCH_count.ToString(), isTrainDash.ToString(), isSuperCH.ToString()));

    }


    private IEnumerator PostMatrix3(string sessionID_MX3, string LevelName, string TrainDash, string SuperCH_count, string isTrainDash, string isSuperCH)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.366340186", sessionID_MX3);
        form.AddField("entry.1848486491", LevelName);
        form.AddField("entry.465445524", TrainDash);
        form.AddField("entry.1833994225", SuperCH_count); // add 2 booleans where they have this active skills or not
        form.AddField("entry.1698635392", isTrainDash);
        form.AddField("entry.1177681711", isSuperCH);

        using (UnityWebRequest www = UnityWebRequest.Post(URL_MX3, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }



    //Matrix 4
    public void SendMatrix4(string ChoiceName)// type change
    {
        StartCoroutine(PostMatrix4(sessionID_MX4.ToString(), ChoiceName.ToString()));
    }
    private IEnumerator PostMatrix4(string sessionID_MX4, string ChoiceName)
    {
        WWWForm form = new WWWForm();

        form.AddField("entry.366340186", sessionID_MX4);
        form.AddField("entry.1210868527", ChoiceName);

        using (UnityWebRequest www = UnityWebRequest.Post(URL_MX4, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }


    //Matrix 5
    public void SendMatrix5(float currentScore)// type change
    {
        StartCoroutine(PostMatrix5(sessionID_MX5.ToString(), currentScore.ToString()));
    }
    private IEnumerator PostMatrix5(string sessionID_MX5, string currentScore)
    {
        WWWForm form = new WWWForm();

        form.AddField("entry.1776650237", sessionID_MX5);
        form.AddField("entry.575622577", currentScore);

        using (UnityWebRequest www = UnityWebRequest.Post(URL_MX5, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log(www.result);
            }
        }


    }


    //Matrix 6 get score
    public void GetMatrix6()
    {
        StartCoroutine(GetMatrix6FromUrl());
    }
    private IEnumerator GetMatrix6FromUrl()
    {
        string url = "https://csci-526-trinity.wl.r.appspot.com/getScores";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Send the request and wait for a response
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error while fetching data: " + webRequest.error);
            }
            else
            {
                // Data has been successfully received
                string data = webRequest.downloadHandler.text;
                // Debug.Log("Received data: " + data);
                GameplayManager.SetScoresStrings(data);
                GameplayManager.UpdateScores();


                // You can process the 'data' string here or trigger other actions.
            }
        }


    }


}


