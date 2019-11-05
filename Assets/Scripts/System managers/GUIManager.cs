using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] Text moneyLabel;
    [SerializeField] Text populationLabel;
    [SerializeField] Text happyLabel;
    [SerializeField] GameManager gameManager;
    [SerializeField] Image happinessLevelsImage;


    [SerializeField] Sprite[] sprites;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        //moneyLabel.text = gameManager.moneyAmount + "";
        //populationLabel.text = gameManager.population + "";
        //happyLabel.text = gameManager.happiness + "";
        
        moneyLabel.text = "$" + System.String.Format("{0:n}", gameManager.moneyAmount);
        populationLabel.text = System.String.Format("{0:n}", gameManager.population);
        happyLabel.text = System.String.Format("{0:n}", gameManager.happiness);

        if (gameManager.happiness >= 90)        // Happy
        {
            happinessLevelsImage.sprite = sprites[0];
        } else if (gameManager.happiness >= 66) // Content
        {
            happinessLevelsImage.sprite = sprites[1];
        }
        else if (gameManager.happiness >= 33)   // Middeling
        {
            happinessLevelsImage.sprite = sprites[2];
        }
        else if (gameManager.happiness >= 15)   // Somewhat displeased
        {
            happinessLevelsImage.sprite = sprites[3];
        } else                                  // Unhappy
        {
            happinessLevelsImage.sprite = sprites[4];
        }

        
        // happyLabel.text = gameManager.happiness + "%";
    }
}
