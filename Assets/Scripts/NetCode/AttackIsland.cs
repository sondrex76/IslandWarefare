using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class AttackIsland : MonoBehaviour
{



    //Gets the time of attack in UNIX millisecods
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



    //Calculates the winner 
    public void CalculateWinner(Action<ExecuteCloudScriptResult> onComplete)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "calcWinner", 
            GeneratePlayStreamEvent = true, 
        }, onComplete, OnErrorShared);
    }


    public void CancleAttack(Action<ExecuteCloudScriptResult> onComplete)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "cancleAttack",
            GeneratePlayStreamEvent = true,
        }, onComplete, OnErrorShared);
    }

    //Log error if any cloudscript fails
    private static void OnErrorShared(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

}
