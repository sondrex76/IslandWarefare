using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] Text moneyLabel;
    [SerializeField] Text populationLabel;
    [SerializeField] Text happyLabel;
    [SerializeField] GameManager gameManager;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //moneyLabel.text = gameManager.moneyAmount + "";
        //populationLabel.text = gameManager.population + "";
        //happyLabel.text = gameManager.happiness + "";
        
        moneyLabel.text = "$" + System.String.Format("{0:n}", gameManager.moneyAmount);
        populationLabel.text = "P: " + System.String.Format("{0:n}", gameManager.population);
        happyLabel.text = "H: " + System.String.Format("{0:n}", gameManager.happiness);
        
        // happyLabel.text = gameManager.happiness + "%";
    }
}
