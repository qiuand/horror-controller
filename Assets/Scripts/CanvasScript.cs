using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] Image healthBar;

    [SerializeField] TextMeshProUGUI petrifyText;
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
        healthBar.fillAmount = gameManagerScript.houseHealth / gameManagerScript.maxHouseHealth;
        petrifyText.text = "Time to petrification: " + System.Math.Round(gameManagerScript.petrifyTimer, 2) + " seconds";
    }
}
