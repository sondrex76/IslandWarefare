using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ChangeBuilding : MonoBehaviour
{

    [SerializeField]
    List<GameObject> buildings;
    [SerializeField]
    RoadPlacer roadPlacer;
    [SerializeField]
    ObjectPlacer objectPlacer;
    [SerializeField]
    GameObject panel;


    public void SetPanel()
    {
        panel.SetActive(!panel.activeSelf);
    }

    public void SetPlaceRoads()
    {
        objectPlacer.enabled = false;
        roadPlacer.enabled = true;
    }

    public void SetForge()
    {
        roadPlacer.enabled = false;
        objectPlacer.enabled = true;
        objectPlacer.objectToPlace = buildings[0];
    }


}
