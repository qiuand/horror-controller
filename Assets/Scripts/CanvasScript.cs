using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI separateWeakPointText;

    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] TextMeshProUGUI monsterAttackText;

    [SerializeField] RawImage stabImage;


    [SerializeField] Image monsterHealthBar;
    [SerializeField] Image healthBar;
    [SerializeField] Image petrifyBar;
    [SerializeField] Image chargeBar;
    [SerializeField] RawImage speedoNeedle;

    [SerializeField] TextMeshProUGUI integrityText
        ;
    [SerializeField] TextMeshProUGUI petrifyText;
    [SerializeField] TextMeshProUGUI timerText;

    float needleCounter = 70;

    GameManager gameManagerScript;

    GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript =GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    void SetNeedle()
    {
    needleCounter -= 0.020f; // Increment the needle counter
    speedoNeedle.transform.eulerAngles = new Vector3(0, 0, needleCounter); //transfrom needle incrementally 
    }



    // Update is called once per frame
    void Update()
    {
        SetNeedle();
        separateWeakPointText.text =
            "Weak point 1: " + gameManagerScript.monsterAttackPositions[0].GetComponent<AttackPoint>().health / gameManagerScript.monsterAttackPositions[0].GetComponent<AttackPoint>().maxHealth+"<br>"+
            "Weak point 2: " + gameManagerScript.monsterAttackPositions[1].GetComponent<AttackPoint>().health / gameManagerScript.monsterAttackPositions[1].GetComponent<AttackPoint>().maxHealth+ "<br>"+
            "Weak point 2: " + gameManagerScript.monsterAttackPositions[2].GetComponent<AttackPoint>().health / gameManagerScript.monsterAttackPositions[2].GetComponent<AttackPoint>().maxHealth + "<br>";

        if (gameManagerScript.gameTimer >= 21600f)
        {
            winText.enabled = true;
            winText.text = "Human Won";
        }
        else if(gameManagerScript.petrifyTimer>= gameManagerScript.timeToPetrify || gameManagerScript.CalculateHouseDestroyed())
        {
            winText.enabled = true;
            winText.text = "Monster Won";
        }
        if (gameManagerScript.stabTimer > 0)
        {
            stabImage.enabled = true;
        }
        else
        {
            stabImage.enabled = false;
        }
        chargeBar.fillAmount=gameManagerScript.monsterDamage/gameManagerScript.maxMonsterDamage;
        petrifyBar.fillAmount=gameManagerScript.petrifyTimer/gameManagerScript.timeToPetrify;
        monsterHealthBar.fillAmount = gameManagerScript.monsterHealth / gameManagerScript.monsterMaxHealth;
        monsterAttackText.text = "Attack Strength: " + System.Math.Round(gameManagerScript.monsterDamage);
        integrityText.text = "House Integrity: " + System.Math.Round((gameManagerScript.houseHealth / gameManagerScript.maxHouseHealth)*100)+"%";

        timerText.text = gameManagerScript.timerString+"AM";
        if (gameManagerScript.gameTimer >= (gameManagerScript.gameTimerMax * (5/6f))){
            timerText.color = new Color(255, 0, 0);
        }
        else if(gameManagerScript.gameTimer>=(gameManagerScript.gameTimerMax/2)){
            timerText.color = new Color(255, 255, 0);
        }
        healthBar.fillAmount = (gameManagerScript.houseHealth / gameManagerScript.maxHouseHealth);

        if (gameManagerScript.playerIsVisible)
        {
            petrifyText.color = Color.red;
            petrifyText.text = "You are spotted! "+System.Math.Round(gameManagerScript.petrifyTimer) +" seconds to death!";
        }
        else
        {
            petrifyText.color = Color.white;
            petrifyText.text = "Not visible: " + System.Math.Round(gameManagerScript.petrifyTimer) + " seconds to death";
        }
/*        petrifyText.text = "Time to petrification: " + System.Math.Round(gameManagerScript.petrifyTimer, 2) + " seconds";*/
    }
}
