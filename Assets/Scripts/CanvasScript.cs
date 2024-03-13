using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{

    [SerializeField] Image readyBarFill;

    [SerializeField] Animator tutorialSlideAnimator;

    float chipSpeed = 2f;
    float lerpTimer;
    [SerializeField] Image petrifyBar;
    [SerializeField] Image petrifyBarBack;

    public int canvasID;

    [SerializeField] Animator healthAnimator;

    [SerializeField] Animator repairAnimator;

    [SerializeField] GameObject repairObject;

    [SerializeField] GameObject tutorialWarning;

    public TextMeshProUGUI timer;
    public GameObject buttonBig;

    [SerializeField] RawImage[] strengthBar;

    [SerializeField] TextMeshProUGUI separateWeakPointText;

    [SerializeField] TextMeshProUGUI practiceText;

    [SerializeField] TextMeshProUGUI winText;
    [SerializeField] TextMeshProUGUI monsterAttackText;

    [SerializeField] RawImage stabImage;

    [SerializeField] Image repairBar;

    [SerializeField] Image monsterHealthBar;
    [SerializeField] Image healthBar;
    [SerializeField] Image chargeBar;
    [SerializeField] RawImage speedoNeedle;

    [SerializeField] TextMeshProUGUI integrityText;
    [SerializeField] TextMeshProUGUI petrifyText;
    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] GameObject MenuObject;
    [SerializeField] GameObject GameUI;

    TutorialManagerNew tutManager;

    [SerializeField] GameObject BlackoutInfo;
    [SerializeField] TextMeshProUGUI infoTitle, infoBody, infoTip;

    float needleCounter = 60;
    float needleDecrementRate = 1.0f; // 1 unit per second

    GameManager gameManagerScript;

    GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        tutManager = GetComponent<TutorialManagerNew>();
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
        readyBarFill.fillAmount = gameManagerScript.buttonTimer / gameManagerScript.buttonBufferTime_Long;
        timer.text = ""+System.Math.Round(gameManagerScript.countdownTimer, 0);

        for (int i = 0; i < strengthBar.Length; i++)
        {
            strengthBar[i].gameObject.SetActive(false);
        }
        /*        for (int i=0; i<gameManagerScript.monsterDamage; i++)
                {
                    strengthBar[i].gameObject.SetActive(true);
                }*/
        for (int i = 0; i < gameManagerScript.monsterHealth; i++)
        {
            strengthBar[i].gameObject.SetActive(true);
        }
        SetNeedle();

        if (gameManagerScript.gameTimer <=0 && gameManagerScript.nightCounter>gameManagerScript.maxNights)
        {
            winText.enabled = true;
            winText.text = "THE HUMAN SURVIVED";
        }
        else if(gameManagerScript.petrifyTimer>= gameManagerScript.timeToPetrify && !gameManagerScript.paused && !gameManagerScript.inTutorial)
        {
            winText.enabled = true;
            winText.text = "THE HUMAN WAS KILLED";
        }
        else if (gameManagerScript.CalculateHouseDestroyed() && !gameManagerScript.paused && !gameManagerScript.inTutorial)
        {
            winText.enabled = true;
            winText.text = "THE HOUSE WAS DESTROYED";
        }
        else
        {
            winText.enabled = false;
        }
        repairBar.fillAmount = gameManagerScript.repairTimer / gameManagerScript.timeUntilCanRepair;
        chargeBar.fillAmount=gameManagerScript.monsterDamage/gameManagerScript.maxMonsterDamage;

        float petrifyRatio = gameManagerScript.petrifyTimer/gameManagerScript.timeToPetrify;
/*        petrifyBar.fillAmount= (gameManagerScript.timeToPetrify-gameManagerScript.petrifyTimer)/gameManagerScript.timeToPetrify;
*/        
        monsterHealthBar.fillAmount = gameManagerScript.monsterHealth / gameManagerScript.monsterMaxHealth;
        monsterAttackText.text = "Attack Strength: " + gameManagerScript.monsterDamage;
        integrityText.text = "House Integrity: " + gameManagerScript.houseHealth / gameManagerScript.maxHouseHealth+"%";

        timerText.text = ""+System.Math.Round(gameManagerScript.gameTimer, 0) /*+ "<br><color=red>" +gameManagerScript.monsterAttackCooldownTimer*//*+"<br><size=15>Night " +gameManagerScript.nightCounter+"/"+gameManagerScript.maxNights+"</size><br><size=10>"+(gameManagerScript.monsterAttackOriginalCooldown)+" Monster Attack Recover<br>" +(gameManagerScript.monsterPetrifyIncrement+" Gaze Power")*/;

        healthBar.fillAmount = (gameManagerScript.houseHealth / gameManagerScript.maxHouseHealth);

        UpdateHealthBar();

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
    public void UpdateTutorialSlides()
    {
        if (gameManagerScript.inTutorial && gameManagerScript.tutorialIndex < tutManager.monsterTutorialArray.Length)
        {
            tutManager.DisplaySlide(gameManagerScript.tutorialIndex, canvasID);
        }
    }
    public void FadeInfo(string title, string body, string tip, bool fadeIn)
    {
        if (!fadeIn)
        {
            BlackoutInfo.SetActive(false);
        }
        else
        {
            BlackoutInfo.SetActive(true);
        }
        infoTitle.text = title;
        infoBody.text = body;
        infoTip.text = tip;
    }
    public void FadeMenu(bool fadeIn)
    {
        if (!fadeIn)
        {
            MenuObject.SetActive(false);
        }
        else
        {
            MenuObject.SetActive(true);
        }
    }
    public void FadeGameUI(bool fadeIn)
    {
        if (!fadeIn)
        {
            GameUI.SetActive(false);
        }
        else
        {
            GameUI.SetActive(true); ;
        }
    }

    public void FadeTutorialWarning(bool fadeIn)
    {
        if (fadeIn)
        {
            tutorialWarning.GetComponent<Animator>().SetBool("Active", true);
        }
        else
        {
            tutorialWarning.GetComponent<Animator>().SetBool("Active", false);
        }
    }
    public void TakeDamage()
    {
        lerpTimer = 0;
        healthAnimator.Play("HealthFlash");

    }
    public void UpdateHealthBar()
    {
        float fillF = petrifyBar.fillAmount;
        float fillB = petrifyBarBack.fillAmount;
        float healthFraction = (float)gameManagerScript.monsterHealth / (float)gameManagerScript.maxMonsterHealth;

        if (fillB > healthFraction || fillB < healthFraction)
        {
            petrifyBar.fillAmount = healthFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            petrifyBarBack.fillAmount = Mathf.Lerp(fillB, healthFraction, percentComplete);
        }
        if (fillF < healthFraction)
        {
            petrifyBarBack.fillAmount = healthFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            petrifyBar.fillAmount = Mathf.Lerp(fillF, petrifyBarBack.fillAmount, percentComplete);
        }
    }

    public void fadeRepair(bool fadeIn)
    {
        if (fadeIn)
        {
            repairAnimator.SetBool("active", true);
        }
        else
        {
            repairAnimator.SetBool("active", false);
        }
    }
    public void FadeTutorialSlide(bool fadeIn)
    {
        if (fadeIn)
        {
            tutorialSlideAnimator.SetBool("SlideIn", true);
        }
        else
        {
            tutorialSlideAnimator.SetBool("SlideIn", false);
        }
    }
    public void ChangePracticeText(string message)
    {
        practiceText.text = message;
    }
}
