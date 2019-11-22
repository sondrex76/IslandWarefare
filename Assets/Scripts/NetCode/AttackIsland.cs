using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using TMPro;
using System.Threading.Tasks;
using System;

public class AttackIsland : MonoBehaviour
{




    public void GetAttackTime(Action<ExecuteCloudScriptResult> onComplete)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "checkBattleTime",
            GeneratePlayStreamEvent = true,
        }, onComplete, OnErrorShared);
    }




    //Starts the attack player calcAttackTime cloudscript (Starting an attack on a diffrent Island)
    public void AttackPlayer(int islandID, Action<ExecuteCloudScriptResult> onComplete)
    {
       PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "calcAttackTime", 
            FunctionParameter = new { islandID = islandID },
            GeneratePlayStreamEvent = true,
        }, onComplete, OnErrorShared);
    }



        private static void OnErrorShared(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    public void CalculateWinner(Action<ExecuteCloudScriptResult> onComplete)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "calcWinner", // Arbitrary function name (must exist in your uploaded cloud.js file)
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        }, onComplete, OnErrorShared);
    }
    


}
