using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

//Script that updates the playercount in the background while playing the game
//Singelton since we don't want many updaters running at once
public class NetworkUpdater : MonoBehaviour
{

    NetworkHelpers networkhelper = new NetworkHelpers();

    private static NetworkUpdater instance;
    private static NetworkUpdater Instance { get { return instance; } }

    //Make sure that there is only one in existance and don't destory it when moving scenes
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    //Start corutine
    private void Start()
    {
        StartCoroutine(updateTimer());
    }

    //Get the amount of players
    public async Task GetPlayersLogin()
    {
        await networkhelper.GetAllPlayerSegments(setNumberOfPlayers);
    }


    //Save the amount of players
    void setNumberOfPlayers(int numberOfPlayers)
    {
        PlayerPrefs.SetInt("NumberOfPlayers", numberOfPlayers);
    }


    //Update playercount every 2 minutes
    IEnumerator updateTimer()
    {
        for (; ; )
        {
            GetPlayersLogin();
            yield return new WaitForSeconds(120f);
        } 
    }


}
