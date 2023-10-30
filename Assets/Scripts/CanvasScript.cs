using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] RawImage stabImage;

    [SerializeField] Image monsterHealthBar;
    [SerializeField] Image healthBar;

    [SerializeField] TextMeshProUGUI integrityText
        ;
    [SerializeField] TextMeshProUGUI petrifyText;
    [SerializeField] TextMeshProUGUI timerText;

    GameManager gameManagerScript;

    GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript =GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManagerScript.stabTimer > 0)
        {
            stabImage.enabled = true;
        }
        else
        {
            stabImage.enabled = false;
        }
        monsterHealthBar.fillAmount = gameManagerScript.monsterHealth / gameManagerScript.monsterMaxHealth;
        integrityText.text = "House Integrity: " + System.Math.Round((gameManagerScript.houseHealth / gameManagerScript.maxHouseHealth)*100)+"%";
        timerText.text = "" + System.Math.Round(gameManagerScript.gameTimer, 2)+" seconds left!";
        healthBar.fillAmount = gameManagerScript.houseHealth / gameManagerScript.maxHouseHealth;

        if (gameManagerScript.playerIsVisible)
        {
            petrifyText.color = Color.red;
            petrifyText.text = "You are spotted!";
        }
        else
        {
            petrifyText.color = Color.white;
            petrifyText.text = "Not visible";
        }
/*        petrifyText.text = "Time to petrification: " + System.Math.Round(gameManagerScript.petrifyTimer, 2) + " seconds";*/
    }
}
