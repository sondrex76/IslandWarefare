using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.DataModels;
using PlayFab.ProfilesModels;

public class Login : MonoBehaviour
{

    private string email;
    private string password;
    private string username;
    public GameObject loginPanel;


    public void Start()
    {

        //Setting the title ID so playfab knows what endpoint to connect to just incase something buggs in editor
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "F5079";
        }


        if (PlayerPrefs.HasKey("EMAIL"))
        {

            email = PlayerPrefs.GetString("EMAIL");
            password = PlayerPrefs.GetString("PASSWORD");

            var request = new LoginWithEmailAddressRequest { Email = email, Password = password };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFail);
        }


    }



    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login successfull");
        PlayerPrefs.SetString("EMAIL", email);
        PlayerPrefs.SetString("PASSWORD", password);
        loginPanel.SetActive(false);
    }

    void OnLoginFail(PlayFabError error)
    {
        //Register account when login fails, 
        var registerRequest = new RegisterPlayFabUserRequest { Email = email, Password = password, Username = username };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFail);
        Debug.Log(error.GenerateErrorReport());
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        PlayerPrefs.SetString("EMAIL", email);
        //Is this a safe way to store passwords?
        PlayerPrefs.SetString("PASSWORD", password);
        loginPanel.SetActive(false);
    }

    void OnRegisterFail(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }



    public void GetEmail(string emailText)
    {
        email = emailText;
    }

    public void GetUsername(string usernameText)
    {
        username = usernameText;
    }

    public void GetPassword(string passwordText)
    {
        password = passwordText;
    }



    public void LoginClicked()
    {

        Debug.Log(email);
        Debug.Log(password);

        var request = new LoginWithEmailAddressRequest { Email = email, Password = password };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFail);
    }


    public void ActivateLoginPanel()
    {
        loginPanel.SetActive(true);
    }

    public void DisableLoginPanel()
    {
        loginPanel.SetActive(false);
    }
}
