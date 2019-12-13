using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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



    private void Start()
    {

        roadPlacer = GameObject.Find("Manager(Has to be at 0,0,0)").GetComponent<RoadPlacer>();
        objectPlacer = GameObject.Find("Manager(Has to be at 0,0,0)").GetComponent<ObjectPlacer>();

        //Loop through each building we can place and create UI for them
        foreach (GameObject building in buildings)
        {
            GameObject ImageButton = new GameObject();
            GameObject ImageText = new GameObject();
            ImageButton.transform.parent = panel.transform;
            //Get the image from the building
            Sprite image = building.GetComponent<AbstractBuilding>().clickableIcon;
            ImageButton.AddComponent<RectTransform>();
            ImageButton.AddComponent<Image>();
            ImageButton.AddComponent<Button>();
            //Set the image to the image from building
            ImageButton.GetComponent<Image>().sprite = image;


            ObjectSelectButton selectScript = ImageButton.AddComponent<ObjectSelectButton>();
            selectScript.building = building;
            selectScript.change = this;
            ImageButton.GetComponent<Button>().onClick.AddListener(selectScript.SetBuilding);


            ImageText.transform.parent = ImageButton.transform;
            RectTransform trans = ImageText.AddComponent<RectTransform>();


            trans.localPosition = new Vector2(0, -67);

            TMPro.TextMeshProUGUI text = ImageText.AddComponent<TMPro.TextMeshProUGUI>();
            text.text = building.transform.name;
            text.alignment = TextAlignmentOptions.Center;
            
            text.enableAutoSizing = true;
            text.fontSizeMin = 10;
            text.fontSizeMax = 18;
            text.fontSize = 20;


          

        }
    }

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

    //Activates building placing
    public void SetBuilding(GameObject building)
    {
        roadPlacer.enabled = false;
        objectPlacer.enabled = true;

        GameObject buildingShow = building;

        //Set a temp building to show where the bulding we are placing is going to be
        objectPlacer.objectToPlaceTemp = Instantiate(buildingShow);
        //This is done to counteract the script that spawns buildings under the ground
        Destroy(objectPlacer.objectToPlaceTemp.GetComponent<AbstractBuilding>());
        objectPlacer.objectToPlaceTemp.transform.GetChild(0).gameObject.transform.position = new Vector3(0,0,0);
        //Add a rigidbody so we can do collisions
        Rigidbody rig = objectPlacer.objectToPlaceTemp.AddComponent<Rigidbody>();
        rig.isKinematic = false;
        rig.constraints = RigidbodyConstraints.FreezeAll;

        //Set layer to tobeplaced so we don't raycast on it
        objectPlacer.objectToPlaceTemp.layer = 9;
        objectPlacer.objectToPlace = building;
        objectPlacer.objectToPlaceTemp.SetActive(true);
        SetPanel();
    }
}
