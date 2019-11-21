using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using TMPro;

public class AttackIsland : MonoBehaviour
{

    private long timeOfAttack = 0;
    private string timeOfAttackDHMS;
    public TMPro.TextMeshProUGUI text;

    // Build the request object and access the API
    void Start()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "checkBattleTime", // Arbitrary function name (must exist in your uploaded cloud.js file)
            FunctionParameter = new { islandID = "9999" }, // The parameter provided to your function
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        }, OnGetAttackTime, OnErrorShared);


    }


    void OnGetAttackTime(ExecuteCloudScriptResult result)
    {
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        object timeOfAttackObject;
        jsonResult.TryGetValue("timeOfAttack", out timeOfAttackObject); // note how "messageValue" directly corresponds to the JSON values set in Cloud Script

        timeOfAttack = System.Convert.ToInt64(timeOfAttackObject);

        StartCoroutine("updateTime");

    }


    void OnGetResult(ExecuteCloudScriptResult result)
    {
        JsonObject jsonResult = (JsonObject)result.FunctionResult;
        object resultOfBattle;
        jsonResult.TryGetValue("result", out resultOfBattle); // note how "messageValue" directly corresponds to the JSON values set in Cloud Script

        text.text = resultOfBattle.ToString();
    }

        private static void OnErrorShared(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    

    IEnumerator updateTime()
    {
        while (true)
        {

            timeOfAttack -= 1000;

            if (timeOfAttack > 0) {

                var diff = timeOfAttack / 1000;
                var secondsDiff = diff % 60;
                diff = diff / 60;
                var minutesDiff = diff % 60;
                diff = diff / 60;
                var hoursDiff = diff % 24;
                diff = diff / 24;
                var daysDiff = diff % 7;

                timeOfAttackDHMS = daysDiff.ToString() + ":" + hoursDiff.ToString() + ":" + minutesDiff.ToString() + ":" + secondsDiff.ToString();

                text.text = timeOfAttackDHMS;

                yield return new WaitForSeconds(1f);
            }
            else {

                text.text = "";
                PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
                {
                    FunctionName = "calcWinner", // Arbitrary function name (must exist in your uploaded cloud.js file)
                    GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
                }, OnGetResult, OnErrorShared);
                break;
            };

        }
    }

}
