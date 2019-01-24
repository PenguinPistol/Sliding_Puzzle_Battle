using System.Collections;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Firebase.Auth;

public class GooglePlayGamesLogin : MonoBehaviour
{
    private FirebaseAuth auth;

    private void Start()
    {
        Init();
    }

    public void OnApplicationQuit()
    {
        PlayGamesPlatform.Instance.SignOut();
    }

    public void Init(bool _debug = false)
    {
        auth = FirebaseAuth.DefaultInstance;

        var config = new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = _debug;
        PlayGamesPlatform.Activate();

        Debug.Log("## Play Game Plaform Initialized!");

        Login();
    }

    public bool Login()
    {
        try
        {
            Social.localUser.Authenticate((bool _success) =>
            {
                if(_success)
                {
                    StartCoroutine(PlayServiceLogin());
                }
            });
        }
        catch (System.Exception e)
        {
            Debug.LogError("## Google Play Service Login Error : " + e.Message);

            return false;
        }

        return true;
    }

    private IEnumerator PlayServiceLogin()
    {
        while(System.String.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
        {
            Init();

            Debug.Log("## Id Token is Null!! ##");

            yield return null;
        }

        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
        string accessToken = null;

        Credential credential = GoogleAuthProvider.GetCredential(idToken, accessToken);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            var user = task.Result;

            Debug.LogFormat("## User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
        });
    }
}
