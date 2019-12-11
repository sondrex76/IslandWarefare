using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandOwner : MonoBehaviour
{
    public int islandID;
    public string playerID;

    public void setStats(int ID, string player)
    {
        transform.name = "Island" + ID;
        transform.tag = "Island";
        islandID = ID;
        playerID = player;
    }
}
