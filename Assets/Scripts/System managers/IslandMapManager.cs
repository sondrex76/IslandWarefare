using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;

public class IslandMapManager : MonoBehaviour
{
    [SerializeField]
    Camera camera;
    [SerializeField]
    AttackIsland attackIsland;


    //Enums for results of battle
    enum resultsBattle { AlreadyInBattle = 0, Success = 1 }

    private int islandID; 
    private long timeOfAttack = 0;
    private string timeOfAttackDHMS;
    [SerializeField]
    private TMPro.TextMeshProUGUI timeLeftTxt, islandIDTxt;
    [SerializeField]
    GameObject attackIslandPanel, timerPanel;

    void Start()
    {
        timeLeftTxt.text = "";
        attackIslandPanel.SetActive(false);
        timerPanel.SetActive(false);

        attackIsland.GetAttackTime(OnGetAttackTime);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.name == "Terrain")
                {
                    islandID = hit.collider.gameObject.GetComponent<IslandOwner>()._islandID;
                    islandIDTxt.text = "Island: " + islandID.ToString();
                    attackIslandPanel.SetActive(true);
                }
            }
        }
    }


    public void AttackPlayer()
    {
        attackIsland.AttackPlayer(islandID, OnAttack);
    }

    public void CloseAttackPanel()
    {
        attackIslandPanel.SetActive(false);
    }

    public void CancleAttack()
    {
        attackIsland.CancleAttack(OnAttackCancelled);
    
    }

    void OnAttackCancelled(ExecuteCloudScriptResult result)
    {
        attackIsland.GetAttackTime(OnGetAttackTime);
    }

    //Gets the result of the calcAttackTime cloudscript
    //Checks if value is an error or not and exectute next script
    void OnAttack(ExecuteCloudScriptResult result)
    {

        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        object canAttack;
        jsonResult.TryGetValue("result", out canAttack);

        Debug.Log(canAttack);

        if (System.Convert.ToInt32(canAttack) == (int)resultsBattle.Success)
        {
            attackIsland.GetAttackTime(OnGetAttackTime);
        }
    }




    //Gets the results of checkBattle cloudscript (UNIX time in millis until battle)
    void OnGetAttackTime(ExecuteCloudScriptResult result)
    {
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        object timeOfAttackObject;
        jsonResult.TryGetValue("timeOfAttack", out timeOfAttackObject);

        Debug.Log(timeOfAttack);

        timerPanel.SetActive(true);
        if (timeOfAttackObject.ToString() == "cancelled")
        {
            timeLeftTxt.text = "Attack cancelled";
            StopCoroutine("updateTime");
        }
        else
        {
            //Convert object to long
            timeOfAttack = System.Convert.ToInt64(timeOfAttackObject);
            StartCoroutine("updateTime");
        }
    }


    //Gets the result of the battle and displays it on screen
    void OnGetResult(ExecuteCloudScriptResult result)
    {
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        object resultOfBattle;
        jsonResult.TryGetValue("result", out resultOfBattle);

        timerPanel.SetActive(true);
        timeLeftTxt.text = resultOfBattle.ToString();
    }



    //Count down the amount of time left before attack is over
    IEnumerator updateTime()
    {
        while (true)
        {

            timeOfAttack -= 1000;

            if (timeOfAttack > 0)
            {

                var diff = timeOfAttack / 1000;
                var secondsDiff = diff % 60;
                diff = diff / 60;
                var minutesDiff = diff % 60;
                diff = diff / 60;
                var hoursDiff = diff % 24;
                diff = diff / 24;
                var daysDiff = diff % 7;

                timeOfAttackDHMS = daysDiff.ToString() + ":" + hoursDiff.ToString() + ":" + minutesDiff.ToString() + ":" + secondsDiff.ToString();

                timeLeftTxt.text = timeOfAttackDHMS;

                yield return new WaitForSeconds(1f);
            }
            else
            {

                timeLeftTxt.text = "";
                timerPanel.SetActive(false);
                attackIsland.CalculateWinner(OnGetResult);
                break;
            };

        }
    }

}
