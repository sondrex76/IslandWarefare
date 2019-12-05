using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.DataModels;
using PlayFab.ProfilesModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System;

public class Login : MonoBehaviour
{

    private string email;
    private string password;
    private string username;
    public GameObject loginPanel;
    NetworkHelpers networkhelper = new NetworkHelpers();

    public void Start()
    {


        GetPlayersLogin();
        //Setting the title ID so playfab knows what endpoint to connect to just incase something buggs in editor
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "F5079";
        }

        //Check if user got credentials saved
        if (PlayerPrefs.HasKey("EMAIL"))
        {

            email = PlayerPrefs.GetString("EMAIL");
            password = PlayerPrefs.GetString("PASSWORD");

            var request = new LoginWithEmailAddressRequest { Email = email, Password = password };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFail);
        }


    }


    //Login is successfull and saves email and password
    void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login successfull");
        PlayerPrefs.SetString("EMAIL", email);
        PlayerPrefs.SetString("PASSWORD", password);
        loginPanel.SetActive(false);
        SceneManager.LoadScene("IslandMap");
    }


    void OnLoginFail(PlayFabError error)
    {
        //Register account when login fails, 
        var registerRequest = new RegisterPlayFabUserRequest { Email = email, Password = password, Username = username };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFail);
        Debug.Log(error.GenerateErrorReport());
    }

    //Registration is successfull and saves email and password
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        PlayerPrefs.SetString("EMAIL", email);
        //TODO Hash password
        PlayerPrefs.SetString("PASSWORD", password);
        GetPlayers();
        loginPanel.SetActive(false);
    }


 //   void OnCompleted(ExecuteCloudScriptResult result)
 //  {
 //  }

    private static void OnErrorShared(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    void OnRegisterFail(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }



    //Sets email to the email typed in the email box
    public void GetEmail(string emailText)
    {
        email = emailText;
    }

    //Sets username to the username typed in the username box
    public void GetUsername(string usernameText)
    {
        username = usernameText;
    }

    //Sets password to the password typed in the password box
    public void GetPassword(string passwordText)
    {
        password = passwordText;
    }


    //Starts the login and registration process
    public void LoginClicked()
    {

        

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


    public async Task GetPlayers()
    {
        await networkhelper.GetAllPlayerSegments(setPlayerData);
    }

    public async Task GetPlayersLogin()
    {
        await networkhelper.GetAllPlayerSegments(setNumberOfPlayers);
    }


    void setNumberOfPlayers(int numberOfPlayers)
    {
        PlayerPrefs.SetInt("NumberOfPlayers", numberOfPlayers);
    }


    //Set the player data
    void setPlayerData(int numberOfPlayers)
    {


        PlayerPrefs.SetInt("ISLANDID", numberOfPlayers);
        PlayerPrefs.SetInt("NumberOfPlayers", numberOfPlayers);

        //Set the playerdata in playfab
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
            {"IslandID", numberOfPlayers.ToString()}
            }
        },
            result => PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "addPlayerToDataBase",
                FunctionParameter = new { },
                GeneratePlayStreamEvent = true,
            }, null, OnErrorShared),

            error =>
            {
                Debug.Log("Error setting playerdata");
                Debug.Log(error.GenerateErrorReport());
            }
            );




       

    }





}
