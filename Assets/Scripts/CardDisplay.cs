using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{

    public TextMeshProUGUI supplyTxt, defenceTxt, attackTxt, nameTxt, descTxt, costTxt;
    public Image image;
    

    public MilitaryUnit unit;

    // Set the text to the unit data
    void Start()
    {
        nameTxt.text = unit.name;
        descTxt.text = unit.decription;
        attackTxt.text = unit.attackPower.ToString();
        defenceTxt.text = unit.defencePower.ToString();
        supplyTxt.text = unit.supplyPower.ToString();
        costTxt.text = unit.cost.ToString()+"$";
        image.sprite = unit.img;
    }

}
