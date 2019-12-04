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

            // Rect rect = new Rect(0f, -67f, 75f, 45f);
            trans.localPosition = new Vector2(0, -67);
            trans.sizeDelta.Set(75, 45);

            TMPro.TextMeshProUGUI text = ImageText.AddComponent<TMPro.TextMeshProUGUI>();
            text.text = building.transform.name;
            text.alignment = TextAlignmentOptions.Center;
            text.enableAutoSizing = true;
            text.fontSize = 30;
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
