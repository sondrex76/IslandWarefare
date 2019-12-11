using System.Collections;
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
        StartCoroutine(updateTimer());
    }


    public async Task GetPlayersLogin()
    {
        await networkhelper.GetAllPlayerSegments(setNumberOfPlayers);
    }



    void setNumberOfPlayers(int numberOfPlayers)
    {
        PlayerPrefs.SetInt("NumberOfPlayers", numberOfPlayers);
    }



    IEnumerator updateTimer()
    {
        for (; ; )
        {
            GetPlayersLogin();
            yield return new WaitForSeconds(120f);
        } 
    }


}
