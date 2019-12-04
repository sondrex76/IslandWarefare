using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelectButton : MonoBehaviour
{
    public GameObject building;
    public ChangeBuilding change;

    public void SetBuilding()
    {
        change.SetBuilding(building);
    }


}
