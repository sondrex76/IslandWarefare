using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetButtonOnClick : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    MilitaryUnit unit;

    Button button;

    // Add l istener to button so we can click it
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        unit = transform.parent.GetComponent<CardDisplay>().unit;
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { gameManager.AddPower(unit); });
    }

    //Checks if we can afford building, if not disable button
    private void Update()
    {
        if (gameManager.GetMoneyAmount() < unit.cost)
        {
            button.interactable = false;
        } else
        {
            button.interactable = true;
        }
    }


}
