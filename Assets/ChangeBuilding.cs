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

    //Hides or shows the panel with buttons
    public void SetPanel()
    {
        panel.SetActive(!panel.activeSelf);
    }

    //Activates road placing
    public void SetPlaceRoads()
    {
        objectPlacer.enabled = false;
        roadPlacer.enabled = true;
        SetPanel();
    }

    //Activates forge placing
    public void SetForge()
    {
        roadPlacer.enabled = false;
        objectPlacer.enabled = true;
        objectPlacer.objectToPlace = buildings[0];
        objectPlacer.objectToPlaceTemp.SetActive(true);
        SetPanel();
    }
}
