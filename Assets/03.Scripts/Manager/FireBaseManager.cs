using Firebase.Analytics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBaseManager : MonoBehaviour
{
    public static FireBaseManager Instance;

    bool isFireLogin = false;

    private void Awake()
    {
        Instance = this;

 
    }
    
    public void LogEvent(string MainTitle)
    {

        // Log an event with no parameters.
        FirebaseAnalytics.LogEvent(MainTitle);


    }

    public void LogEvent(string MainTitle,string SubTitle ,int val)
    {

        // Log an event with no parameters.
        FirebaseAnalytics.LogEvent(MainTitle, SubTitle, val);


    }

    public void FirebaseNullLogin()
    {

        if (!isFireLogin)
        {
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            auth.SignInAnonymouslyAsync().ContinueWith(task =>
            {
                if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                {
                    // User is now signed in.

                    Firebase.Auth.FirebaseUser newUser = task.Result;

                    isFireLogin = true;


                }
                else
                {
                    Debug.Log("failed");
                }
            });
        }


    }

    public void FireBaseGoogleLogin()
    {
        StartCoroutine("CoFireBaseGoogleLogin");
    }

    IEnumerator CoFireBaseGoogleLogin()
    {

        if (!isFireLogin)
        {

            yield return null;
            string idToken = SocialManager.Instance._IDtoken;


            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(idToken, null);
            auth.SignInWithCredentialAsync(credential).ContinueWith(
                task =>
                {
                    if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                    {
                        // User is now signed in.
                        Firebase.Auth.FirebaseUser newUser = task.Result;
                        Debug.Log(string.Format("FirebaseUser:{0}\nEmail:{1}", newUser.UserId, newUser.Email));
                        isFireLogin = true;

                    }
                    else
                    {

                    }
                });
        }
    }


    public void Add_Token()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        string gift_num = "";
        string value_num = "";

        int gift = -1;
        int value = -1;

        if (e.Message.Data.TryGetValue("Gift", out gift_num))
        {
            gift = int.Parse(gift_num);
        }

        if (e.Message.Data.TryGetValue("value", out value_num))
        {
            value = int.Parse(value_num);
        }

        if (gift != -1 && value != -1)
        {

            UIManager.Instance.PushgiftItem.Clear();

            List<int> star_item = new List<int>();
            star_item.Add(gift);
            star_item.Add(value);
            UIManager.Instance.PushgiftItem.Add(star_item);
            UIManager.Instance.Push_Gift();
            // ToastManager.instance.ShowToast(string.Format("gift = {0}.  val = {1}", gift, value));

        }
    }

    public void Get_Editor_Gift()
    {

#if UNITY_EDITOR

        int gift = 0;
        int value = 50;

        if (gift != -1 && value != -1)
        {

            UIManager.Instance.PushgiftItem.Clear();

            List<int> star_item = new List<int>();
            star_item.Add(gift);
            star_item.Add(value);
            UIManager.Instance.PushgiftItem.Add(star_item);
            UIManager.Instance.Push_Gift();
            // ToastManager.instance.ShowToast(string.Format("gift = {0}.  val = {1}", gift, value));

        }
#endif

    }
}
