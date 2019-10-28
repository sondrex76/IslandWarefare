using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.DataModels;
using PlayFab.ProfilesModels;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Threading.Tasks;

public class Login : MonoBehaviour
{

    private string email;
    private string password;
    private string username;
    public GameObject loginPanel;
    NetworkHelpers networkhelper = new NetworkHelpers();

    public void Start()
    {



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
        //Is this a safe way to store passwords?
        PlayerPrefs.SetString("PASSWORD", password);
        GetPlayers();
        loginPanel.SetActive(false);
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


    public async Task GetPlayers()
    {
        await networkhelper.GetAllPlayerSegments(setPlayerData);
    }



    //Set the player data
    void setPlayerData(int numberOfPlayers)
    {

        //Set the playerdata in playfab
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
            {"IslandID", numberOfPlayers.ToString()}
            }
        },
            result => Debug.Log("Yay you got an island"),
            error =>
            {
                Debug.Log("Error setting playerdata");
                Debug.Log(error.GenerateErrorReport());
            }
            );

    }





}
