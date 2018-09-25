using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Core;
using Facebook.Unity;
using GameSparks.Api.Requests;
using UnityEngine.SceneManagement;

public class LoginUI : MonoBehaviour
{

    public Text connectionInfoField;
    public Text userNameField;

    public Button FacebookLoginButton;

    // Use this for initialization
    void Start()
    {
        GS.GameSparksAvailable += OnGameSparksConnected;
    }

    private void OnGameSparksConnected(bool _isConnected)
    {
        if (_isConnected)
        {
            connectionInfoField.text = "GameSparks Connected...";
            FacebookLoginButton.gameObject.SetActive(true);
        }
        else
        {
            connectionInfoField.text = "GameSparks Not Connected...";
        }
    }


    public void FacebookConnect_bttn()
    {
        Debug.Log("Connecting Facebook With GameSparks...");// first check if FB is ready, and then login //
                                                            // if it's not ready we just init FB and use the login method as the callback for the init method //
        if (!FB.IsInitialized)
        {
            Debug.Log("Initializing Facebook...");
            FB.Init(ConnectGameSparksToGameSparks, null);
        }
        else
        {
            FB.ActivateApp();
            ConnectGameSparksToGameSparks();
        }
    }



    ///<summary>
    ///This method is used as the delegate for FB initialization. It logs in FB
    /// </summary>
    private void ConnectGameSparksToGameSparks()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            Debug.Log("Logging Into Facebook...");
            var perms = new List<string>() { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(perms, (result) =>
            {
                if (FB.IsLoggedIn)
                {
                    Debug.Log("Logged In, Connecting GS via FB..");
                     new GameSparks.Api.Requests.FacebookConnectRequest()
                    .SetAccessToken(AccessToken.CurrentAccessToken.TokenString)
                    .SetDoNotLinkToCurrentPlayer(false)// we don't want to create a new account so link to the player that is currently logged in
                    .SetSwitchIfPossible(true)//this will switch to the player with this FB account id they already have an account from a separate login
                    .Send((fbauth_response) =>
                    {
                        if (!fbauth_response.HasErrors)
                        {
                            connectionInfoField.text = "GameSparks Authenticated With Facebook...";
                            Debug.Log("fbauth_response:" + fbauth_response.JSONString);
                            userNameField.text = fbauth_response.DisplayName;
                            Invoke("LoadMenu", 3f);
                            //SceneManager.LoadScene(1);
                        }
                        else
                        {
                            Debug.LogWarning(fbauth_response.Errors.JSON);//if we have errors, print them out
                        }
                    });
                }
                else
                {
                    Debug.LogWarning("Facebook Login Failed:" + result.Error);
                }
            });// lastly call another method to login, and when logged in we have a callback
        }
        else
        {
            FacebookConnect_bttn();// if we are still not connected, then try to process again
        }
    }

    void LoadMenu()
    {
        SceneManager.LoadScene(1);
    }

}
