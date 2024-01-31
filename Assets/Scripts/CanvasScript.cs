using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    public int canvasID;

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

    [SerializeField] TextMeshProUGUI integrityText;
    [SerializeField] TextMeshProUGUI petrifyText;
    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] GameObject MenuObject;
    [SerializeField] GameObject GameUI;

    TutorialManager tutManager;

    [SerializeField] GameObject BlackoutInfo;
    [SerializeField] TextMeshProUGUI infoTitle, infoBody, infoTip;

    float needleCounter = 60;
    float needleDecrementRate = 1.0f; // 1 unit per second

    GameManager gameManagerScript;

    GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        tutManager = GetComponent<TutorialManager>();
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
        if (gameManagerScript.inTutorial && gameManagerScript.tutorialIndex != 99)
        {
            tutManager.DisplaySlide(gameManagerScript.tutorialIndex, canvasID);
        }

        for (int i = 0; i < strengthBar.Length; i++)
        {
            strengthBar[i].gameObject.SetActive(false);
        }
        for (int i=0; i<gameManagerScript.monsterDamage; i++)
        {
            strengthBar[i].gameObject.SetActive(true);
        }
        SetNeedle();

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

        repairBar.fillAmount = gameManagerScript.repairTimer / gameManagerScript.timeUntilCanRepair;
        chargeBar.fillAmount=gameManagerScript.monsterDamage/gameManagerScript.maxMonsterDamage;

        float petrifyRatio = gameManagerScript.petrifyTimer/gameManagerScript.timeToPetrify;
        petrifyBar.fillAmount= (gameManagerScript.timeToPetrify-gameManagerScript.petrifyTimer)/gameManagerScript.timeToPetrify;
        
        monsterHealthBar.fillAmount = gameManagerScript.monsterHealth / gameManagerScript.monsterMaxHealth;
        monsterAttackText.text = "Attack Strength: " + gameManagerScript.monsterDamage;
        integrityText.text = "House Integrity: " + gameManagerScript.houseHealth / gameManagerScript.maxHouseHealth+"%";

        timerText.text = "Night "+gameManagerScript.nightCounter+"/"+gameManagerScript.maxNights+"<br>"+System.Math.Round(gameManagerScript.gameTimer,2);

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
    }
    
    public void FadeInfo(string title, string body, string tip)
    {
        if (!BlackoutInfo.activeInHierarchy)
        {
            BlackoutInfo.SetActive(true);
        }
        else if (BlackoutInfo.activeInHierarchy)
        {
            BlackoutInfo.SetActive(false);
        }
        infoTitle.text = title;
        infoBody.text = body;
        infoTip.text = tip;
    }
    public void FadeMenu()
    {
        MenuObject.SetActive(false);
        GameUI.SetActive(true);
    }
}
