using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] RawImage[] strengthBar;

    [SerializeField] TextMeshProUGUI separateWeakPointText;

    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] TextMeshProUGUI monsterAttackText;

    [SerializeField] RawImage stabImage;

    [SerializeField] Image repairBar;

    [SerializeField] Image monsterHealthBar;
    [SerializeField] Image healthBar;
    [SerializeField] Image petrifyBar;
    [SerializeField] Image chargeBar;
    [SerializeField] RawImage speedoNeedle;

    [SerializeField] TextMeshProUGUI integrityText
        ;
    [SerializeField] TextMeshProUGUI petrifyText;
    [SerializeField] TextMeshProUGUI timerText;

    float needleCounter = 60;
    float needleDecrementRate = 1.0f; // 1 unit per second

    GameManager gameManagerScript;

    GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript =GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    void SetNeedle()
    {
        needleCounter -= needleDecrementRate * Time.deltaTime;
        speedoNeedle.transform.eulerAngles = new Vector3(0, 0, needleCounter);
    }



    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < strengthBar.Length; i++)
        {
            strengthBar[i].gameObject.SetActive(false);
        }
        for (int i=0; i<gameManagerScript.monsterDamage; i++)
        {
            strengthBar[i].gameObject.SetActive(true);
        }
        SetNeedle();
        separateWeakPointText.text =
            "Weak point 1: " + gameManagerScript.monsterAttackPositions[0].GetComponent<AttackPoint>().health / gameManagerScript.monsterAttackPositions[0].GetComponent<AttackPoint>().maxHealth+"<br>"+
            "Weak point 2: " + gameManagerScript.monsterAttackPositions[1].GetComponent<AttackPoint>().health / gameManagerScript.monsterAttackPositions[1].GetComponent<AttackPoint>().maxHealth+ "<br>"+
            "Weak point 2: " + gameManagerScript.monsterAttackPositions[2].GetComponent<AttackPoint>().health / gameManagerScript.monsterAttackPositions[2].GetComponent<AttackPoint>().maxHealth + "<br>";

        if (gameManagerScript.gameTimer >= 21600f)
        {
            winText.enabled = true;
            winText.text = "THE HUMAN SURVIVED";
        }
        else if(gameManagerScript.petrifyTimer>= gameManagerScript.timeToPetrify || gameManagerScript.CalculateHouseDestroyed())
        {
            winText.enabled = true;
            winText.text = "THE HUMAN DIED";
        }
        if (gameManagerScript.stabTimer > 0)
        {
            stabImage.enabled = true;
        }
        else
        {
            stabImage.enabled = false;
        }
        repairBar.fillAmount = gameManagerScript.repairTimer / gameManagerScript.timeUntilCanRepair;
        chargeBar.fillAmount=gameManagerScript.monsterDamage/gameManagerScript.maxMonsterDamage;

        float petrifyRatio = gameManagerScript.petrifyTimer/gameManagerScript.timeToPetrify;

/*        petrifyBar.GetComponent<Image>(). = new Color(255f*petrifyRatio, 255f-255f* petrifyRatio*2, 0);
*/        petrifyBar.fillAmount= (gameManagerScript.timeToPetrify-gameManagerScript.petrifyTimer)/gameManagerScript.timeToPetrify;
        
        monsterHealthBar.fillAmount = gameManagerScript.monsterHealth / gameManagerScript.monsterMaxHealth;
        monsterAttackText.text = "Attack Strength: " + gameManagerScript.monsterDamage;
        integrityText.text = "House Integrity: " + gameManagerScript.houseHealth / gameManagerScript.maxHouseHealth+"%";

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
