using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkUpdater : MonoBehaviour
{

    NetworkHelpers networkhelper = new NetworkHelpers();

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    private void Start()
    {
        GetPlayersLogin();
    }


    public async Task GetPlayersLogin()
    {
        await networkhelper.GetAllPlayerSegments(setNumberOfPlayers);
    }



    void setNumberOfPlayers(int numberOfPlayers)
    {
        PlayerPrefs.SetInt("NumberOfPlayers", 100);
    }


}
