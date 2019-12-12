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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        unit = transform.parent.GetComponent<CardDisplay>().unit;
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { gameManager.AddPower(unit); });
    }


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
