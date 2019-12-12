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

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        unit = transform.parent.GetComponent<CardDisplay>().unit;
        GetComponent<Button>().onClick.AddListener(delegate { gameManager.AddPower(unit); });
    }
}
