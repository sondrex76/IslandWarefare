using System.Collections;
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

        foreach (GameObject building in buildings)
        {
            GameObject ImageButton = new GameObject();
            GameObject ImageText = new GameObject();
            ImageButton.transform.parent = panel.transform;
            Sprite image = building.GetComponent<AbstractBuilding>().clickableIcon;
            ImageButton.AddComponent<RectTransform>();
            ImageButton.AddComponent<Image>();
            ImageButton.AddComponent<Button>();
            ImageButton.GetComponent<Image>().sprite = image;

            ObjectSelectButton selectScript = ImageButton.AddComponent<ObjectSelectButton>();
            selectScript.building = building;
            selectScript.change = this;
            ImageButton.GetComponent<Button>().onClick.AddListener(selectScript.SetBuilding);


            ImageText.transform.parent = ImageButton.transform;
            RectTransform trans = ImageText.AddComponent<RectTransform>();
            //trans.anchorMin = new Vector2(0.5f, 0.5f);
            //trans.anchorMax = new Vector2(0.5f, 0.5f);


            // Rect rect = new Rect(0f, -67f, 75f, 45f);
            trans.localPosition = new Vector2(0, -67);
           // trans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 75f);
           // trans.sizeDelta = new Vector2(75, -23);

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

    //Activates forge placing
    public void SetBuilding(GameObject building)
    {
        roadPlacer.enabled = false;
        objectPlacer.enabled = true;
        objectPlacer.objectToPlace = building;
        objectPlacer.objectToPlaceTemp.SetActive(true);
        SetPanel();
    }
}
