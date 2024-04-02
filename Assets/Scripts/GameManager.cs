using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Rendering.UI;

public class GameManager : MonoBehaviour
{
    bool inMenu = true;

    bool practiceAdvancable = false;

    [SerializeField] Animator healthAnimator;

    [SerializeField] Animator humanAnimator;

    public float buttonTimer = 0f;
    float buttonBufferTime = 0.050f;

    float bufferTime_Failed_Min = 0.3f;
    float bufferTime_Failed_Max = 0.8f;

    public float buttonBufferTime_Long;

    [SerializeField] AudioSource timer;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip gong;
    [SerializeField] AudioClip sting, buttonClick;

    bool tutorialGrace = true;

    int tutorialIndex_Macro = 0;

    public int monsterHealth;
    public int maxMonsterHealth=3;

    int stateProgressionTracker = 0;

    float healthRegenMultiplier = 0.5f;
    float boardedDebuff = 0.3f;

    float startingChargeSpeed=0.25f;
    float startingPetrifySpeed = 1f;

    public int attackPerHit = 1;

    bool isButtonDown=false;

    public Shake shaker1, shaker2;

    float originalCountdown=9f;
    public float countdownTimer;

    bool gameWon = false;
    bool countDownEnabled = false;

    public float monsterStrengthIncrement=0.15f;
    public float monsterPetrifyIncrement = 1.25f;

    public int nightCounter = 1;
    public int maxNights = 6;

    public int tutorialIndex=0;

    public bool paused = false;
    public bool inTutorial = false;
    public bool gameLocked = true;
    public bool tutorialCompleted = false;
    public bool introSlideVisible = false;

    bool serialFlag = true;

    public bool playerAbsent = true;

    [SerializeField] AudioSource scream;

    Material material;

    public GameObject hiddenPlayerPosition;

    public float timeUntilCanRepair;
    public float repairTimer;

    public GameObject monsterHUD;

    [SerializeField] GameObject monsterEyeModel;

    [SerializeField] AudioSource heartbeat;

    float elapsedLerpTime;

    byte[] validatedIncomingManager = new byte[9];

    public string timerString;

    string currentPlayerPosition = null;
    public static string whoWon = "";
    public static string cause = "";
/*    public GameObject tail;
*/    public GameObject sparks;
    public GameObject monsterExplosion;

    Vector3 startLerpPosition;
    Vector3 startLerpRotation;

    int currentEyePosition;
    int previousEyePosition;

    int currentBoardPosition;
    int previousBoardPosition;

    int currentHumanPosition;
    int previousHumanPosition;

    /*    [SerializeField] GameObject monsterLight;*/

    GameObject attachedEyeWall;

    public bool houseDestroyed = false;

    public float stabTimerCooldown = 1.0f;
    public float stabTimer=0f;

    public float monsterMaxHealth = 100;

    public float monsterAttackTimer = 0.5f;

    public float monsterAttackOriginalCooldown = 0.1f;
    public float monsterAttackCooldownTimer;

    bool visibleFlag;

    float originalGameTimer = 90f;
    public float gameTimer;

    [SerializeField] TextMeshProUGUI petrifyText;

    public float timeToPetrify=20f;
    public float petrifyTimer;

    public GameObject board;
    public GameObject player;
    public GameObject monsterEyeCamera;

    int numberOfWeakPoints = 3;
    public int weakPointHealth = 20;

    public int maxHouseHealth;
    public int houseHealth;

    public int monsterDamage;

    public int minMonsterDamage = 0;
    public int maxMonsterDamage = 10;

    int temporaryHealthCalculation=0;

    public bool playerIsVisible = false;

    public GameObject[] monsterEyepositions= new GameObject[4];
    public  GameObject[] monsterAttackPositions;

    float originalTimeUntilNextAttack=0.25f;

    float timeUntilNextAttack = 0.25f;
    float attackTimer;

    [SerializeField] AudioSource collapse;

    AudioSource GameManagerSource;

    [SerializeField] GameObject defaultBarricadePoint;

    [SerializeField] GameObject monsterCanvas, humanCanvas;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        buttonBufferTime_Long = .75f;

        maxMonsterHealth = 4;
        monsterHealth = maxMonsterHealth;

        monsterAttackOriginalCooldown = startingChargeSpeed;
        monsterPetrifyIncrement = startingPetrifySpeed;

        attackTimer = timeUntilNextAttack;

        countdownTimer = originalCountdown;

        gameTimer = originalGameTimer;

        GameManagerSource = GetComponent<AudioSource>();

        repairTimer = timeUntilCanRepair;

        material = player.GetComponent<MeshRenderer>().sharedMaterial;

        timeUntilNextAttack = originalTimeUntilNextAttack;

        monsterHealth = maxMonsterHealth;

        monsterAttackCooldownTimer = monsterAttackTimer;

        MoveEyeCameraToLocation(monsterEyepositions[monsterEyepositions.Length-1]);
        changeLerpTarget(monsterEyepositions[4]);

        maxHouseHealth = weakPointHealth * numberOfWeakPoints;
        houseHealth = maxHouseHealth;
        petrifyTimer = 0;

        player.GetComponent<BoxCollider>().enabled = false;
        player.GetComponent<MeshRenderer>().enabled = false;

        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
           
        {
            ResetGame();
        }

        if (countDownEnabled)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0)
            {
                monsterHealth = maxMonsterHealth;
                ResetStats();
                EmpowerMonster();
                countdownTimer = originalCountdown;
                countDownEnabled = false;
                gameLocked = false;
                paused = false;
                tutorialCompleted = true;
                humanCanvas.GetComponent<CanvasScript>().FadeInfo(null, null, null, false);
                monsterCanvas.GetComponent<CanvasScript>().FadeInfo(null, null, null, false);

                humanCanvas.GetComponent<CanvasScript>().FadeMenu(false);
                monsterCanvas.GetComponent<CanvasScript>().FadeMenu(false);
                humanCanvas.GetComponent<CanvasScript>().FadeGameUI(true);
                monsterCanvas.GetComponent<CanvasScript>().FadeGameUI(true);

                humanCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(false);
                monsterCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(false);
            }
        }


        CheckHouseSound();


        LightUpLEDHealth(3, monsterAttackPositions[1].GetComponent<AttackPoint>().health);
        LightUpLEDHealth(4, monsterAttackPositions[0].GetComponent<AttackPoint>().health);
        LightUpLEDHealth(6, monsterAttackPositions[2].GetComponent<AttackPoint>().health);

        LightUpLEDWindow(attachedEyeWall.GetComponent<Window>().windowID);

        if (serialFlag && SerialCommunications.communicationReadyFlag)
        {
            validatedIncomingManager = SerialCommunications.validatedIncoming;
            SerialCommunications.communicationReadyFlag = false;

            Debug.Log(validatedIncomingManager[7]);
            if (validatedIncomingManager[7] == 0)
            {
                isButtonDown = true;
            }
            else if (validatedIncomingManager[7] == 1 && buttonTimer>bufferTime_Failed_Min && buttonTimer < bufferTime_Failed_Max)
            {
                buttonTimer = 0f;
                isButtonDown = false;
            }
            else if (validatedIncomingManager[7] == 1 && buttonTimer >= buttonBufferTime_Long)
            {
                if (inTutorial)
                {
                    buttonTimer = 0f;
                    isButtonDown = false;
                    HandleGreenButton();
                }
                else if (inMenu)
                {
                    print("Yeah");
                    buttonTimer = 0f;
                    isButtonDown = false;
                    stateProgressionTracker = 5;
                    HandleGreenButton();
                }
            }

            else if (isButtonDown && validatedIncomingManager[7] == 1 && buttonTimer>=buttonBufferTime)
            {
                isButtonDown = false;

                if (inTutorial)
                {
                    AdvanceTutorialSlides();
                    buttonTimer = 0;
                    isButtonDown = false;
                }
                else
                {
                    HandleGreenButton();
                    buttonTimer = 0;
                    isButtonDown = false;
                }
            }
            else
            {
                buttonTimer = 0f;
                isButtonDown = false;
            }
        }
        if (isButtonDown)
        {
            buttonTimer += Time.deltaTime;
        }
        if (!serialFlag)
        {
            KeyboardControls();
        }
        if (Input.GetKeyDown("a"))
        {
            HandleGreenButton();
        }

        if (!gameLocked)
        {

            attackTimer -= Time.deltaTime;

            repairTimer += Time.deltaTime;

            monsterAttackCooldownTimer -= Time.deltaTime;

/*            material.SetFloat("_Float", 0 + (petrifyTimer / 5f - 0.3f));
*/            timeUntilNextAttack -= Time.deltaTime;

            if (!inTutorial && !paused)
            {
                UpdateTimer();
                CheckWin();
                houseDestroyed = CalculateHouseDestroyed();
                houseHealth = CalculateHouseHealth();
                if (monsterHealth <= 0)
                {
                    MonsterDied();
                }
            }

            CheckIfBoarded();

            ChargeMonsterDamage();

            MonsterInput();
            HumanBlockInput();
            PlayerRepairInput();

            LerpMonsterPosition();


            if (attackTimer<=0 && monsterDamage>0)
            {
                MonsterAttackInput();
                attackTimer = timeUntilNextAttack;
            }

            PetrifyTimer();

            if (Input.GetKeyDown("b"))
            {
                SceneManager.LoadScene(0);
            }

            if (!serialFlag)
            {
                validatedIncomingManager[5] = 0;
            }
        }

        if(!gameLocked && paused && monsterHealth <= 0)
        {
            monsterHealth = maxMonsterHealth;
        }

    }
    void MonsterDied()
    {
        if (nightCounter >= maxNights && !gameWon)
        {
            source.PlayOneShot(gong, 1);

            timer.enabled = false;
            humanCanvas.GetComponent<CanvasScript>().FadeInfo("You Have Slain the Monster.", "Game Over.", "Congratulations.", true);
            monsterCanvas.GetComponent<CanvasScript>().FadeInfo("You Have Been Slain by the Human.", "Game Over.", "", true);
            gameWon = true;
        }
        else
        {
            source.PlayOneShot(gong, 1);

            EnableButton(true);
            timer.enabled = false;
            nightCounter++;
            humanCanvas.GetComponent<CanvasScript>().FadeInfo("You Drove the Monster Back", "...For now. It slinks off to regenerate its lost limbs...", null, true);
            monsterCanvas.GetComponent<CanvasScript>().FadeInfo("You've Been Severely Wounded.", "Night 2/3", null, true);
            stateProgressionTracker = 5;
        }
        
        paused = true;
        gameLocked = true;
    }
    void CheckHouseSound()
    {
        for (int i = 0; i < monsterAttackPositions.Length; i++)
        {
            float greatestDamage = 0;
            if (8 - monsterAttackPositions[i].GetComponent<AttackPoint>().health > greatestDamage)
            {
                greatestDamage = 8 - monsterAttackPositions[i].GetComponent<AttackPoint>().health;
/*                Debug.Log(greatestDamage);*/
                GameManagerSource.volume = greatestDamage * 0.17f;
            }
        }
    }
    void UpdateTimer()
    {
        gameTimer -= Time.deltaTime;
/*        gameTimer+=Time.deltaTime*180f;
        int seconds = (int)(gameTimer % 60);
        int minutes = (int)(gameTimer / 60) % 60;
        int hours = (int)(gameTimer / 3600) % 24;
        timerString = string.Format("{0:0}:{1:00}", hours, minutes);*/

    }
    void CheckWin()
    {
        if (petrifyTimer >= timeToPetrify || CalculateHouseDestroyed())
        {
            whoWon = "Monster Victory";
            if (CalculateHouseDestroyed())
            {
                collapse.enabled = true;
                cause = "Cause: House Obliterated";
            }
            else
            {
                cause = "Cause: Lethal Gaze";
            }
            if (!gameWon)
            {
                timer.enabled = false;
                humanCanvas.GetComponent<CanvasScript>().FadeInfo("Killed By The Monster.", "You surived for " + nightCounter + " days.", "", true);
                monsterCanvas.GetComponent<CanvasScript>().FadeInfo("You killed the Human.", "It took " + nightCounter + " days of monstrous rage", "", true);
            }
            gameWon = true;
            scream.enabled = true;
            /*            StartCoroutine(LoadGameOver());*/
        }
        else if (gameTimer <= 0)
        {
/*            EnableButton(true);*/
            timer.enabled = false;

            nightCounter++;
            paused = true;
            gameLocked = true;
            if (nightCounter > maxNights && !gameWon)
            {
                source.PlayOneShot(gong, 1);
                gameWon = true;
                humanCanvas.GetComponent<CanvasScript>().FadeInfo("GAME OVER", "You have survived.", "", true);
                monsterCanvas.GetComponent<CanvasScript>().FadeInfo("GAME OVER", "The human has survived.", "", true);

            }
            else if (!gameWon)
            {
                source.PlayOneShot(gong, 1);
                humanCanvas.GetComponent<CanvasScript>().FadeInfo("You Live to See Another Day.", "Night " + nightCounter + "/" + maxNights, "But the monster will return...", true);
                monsterCanvas.GetComponent<CanvasScript>().FadeInfo("The Human Lives to See Another Day.", "Night " + nightCounter + "/" + maxNights, "But you return the next night...", true);
            }
            stateProgressionTracker = 5;
            paused = true;
/*            StartCoroutine(LoadGameOver());
*/        }
    }
    void CheckIfBoarded()
    {
        if (attachedEyeWall && attachedEyeWall.GetComponent<Window>().isBoarded)
        {
            attachedEyeWall.GetComponent<Window>().windowLight.SetActive(false);
        }
        else
        {
            attachedEyeWall.GetComponent<Window>().windowLight.SetActive(true);
        }
    }
    void ChargeMonsterDamage()
    {
        if (monsterAttackCooldownTimer<=0 && monsterDamage<maxMonsterDamage)
        {
            monsterDamage += 1;
            monsterAttackCooldownTimer = monsterAttackOriginalCooldown;
        }
    }
    public bool CalculateHouseDestroyed()
    {
        int destroyedPoints = 0;
        for (int i = 0; i < monsterAttackPositions.Length; i++)
        {
            if (monsterAttackPositions[i].GetComponent<AttackPoint>().health <= 0)
            {
                destroyedPoints++;
            }
        }
        if (destroyedPoints >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    int CalculateHouseHealth()
    {
        temporaryHealthCalculation = 0;
        for (int i = 0; i < monsterAttackPositions.Length; i++)
        {
            temporaryHealthCalculation += monsterAttackPositions[i].GetComponent<AttackPoint>().health;
        }
        return (temporaryHealthCalculation);
    }
    void PetrifyTimer()
    {
/*        if (playerIsVisible && !playerAbsent && attachedEyeWall && !attachedEyeWall.GetComponent<Window>().isBoarded)
        {
            heartbeat.enabled = true;
            petrifyTimer += Time.deltaTime * monsterPetrifyIncrement;

        }
        else if (playerIsVisible && !playerAbsent)
        {
            heartbeat.enabled = true;
            petrifyTimer += Time.deltaTime * monsterPetrifyIncrement * boardedDebuff;
        }
        else
        {
            petrifyTimer -= Time.deltaTime * healthRegenMultiplier;
            heartbeat.enabled = false;

        }
        if (petrifyTimer <= 0)
        {
            petrifyTimer = 0;
        }*/
    }
    void MoveEyeCameraToLocation(GameObject reference)
    {
        if (attachedEyeWall)
        {
            attachedEyeWall.GetComponent<Window>().windowLight.SetActive(false);
        }
        monsterEyeCamera.GetComponent<MonsterEye>().playSound();
        attachedEyeWall = reference;
        monsterEyeCamera.gameObject.transform.position = reference.transform.position;
        monsterEyeCamera.gameObject.transform.rotation = reference.transform.rotation;
    }
    void MoveHumanBlock(GameObject reference)
    {
        for (int i = 0; i < monsterEyepositions.Length; i++)
        {
            monsterEyepositions[i].GetComponent<Window>().isBoarded = false;
        }
        reference.GetComponent<Window>().isBoarded = true;
        GameObject boardPoint = reference.transform.Find("Board Point").gameObject;
        board.gameObject.transform.position = boardPoint.transform.position;
        board.gameObject.transform.rotation = boardPoint.transform.rotation;

        board.gameObject.GetComponent<Animator>().Play("BoardPlace");

/*        Instantiate(sparks, boardPoint.transform.position, boardPoint.transform.rotation);
*/        board.GetComponent<Board>().PlaySound();
    }
    void InvalidateAllDefendedPoints()
    {
        for (int i = 0; i < monsterAttackPositions.Length; i++)
        {
            monsterAttackPositions[i].GetComponent<AttackPoint>().repairing = false;
            monsterAttackPositions[i].GetComponent<AttackPoint>().isDefended = false;
        }
    }
    void MoveHumanRepair(GameObject reference)
    {
        humanCanvas.GetComponent<CanvasScript>().fadeRepair(true);
        repairTimer = 0;
        player.GetComponent<Player>().Move();

        InvalidateAllDefendedPoints();

        reference.GetComponent<AttackPoint>().isDefended = true;
        reference.GetComponent<AttackPoint>().repairing = true;


        GameObject repairPoint = reference.transform.Find("Repair Point").gameObject;
        player.gameObject.transform.position = 
            new Vector3(
                repairPoint.transform.position.x,
                repairPoint.transform.position.y+1.5f,
                repairPoint.transform.position.z);
        player.gameObject.transform.rotation =
            Quaternion.Euler(
                repairPoint.transform.rotation.x,
                repairPoint.transform.rotation.y,
                repairPoint.transform.rotation.z);
        player.GetComponent<Animator>().Play("HumanReplace");
    }

    //Monster eye
    void MonsterInput()
    {
        previousEyePosition = currentEyePosition;
        currentEyePosition = validatedIncomingManager[3];
/*        print("Previous: " + previousEyePosition+" Current: " + currentEyePosition);
*/        
        if (previousEyePosition != currentEyePosition)
            {
                if (validatedIncomingManager[3] == 1)
                {
                    monsterHUD.SetActive(true);
                    changeLerpTarget(monsterEyepositions[0]);
/*                    MoveEyeCameraToLocation(monsterEyepositions[0]);
*/            }
            else if (validatedIncomingManager[3] == 4)
            {
                monsterHUD.SetActive(true);
                changeLerpTarget(monsterEyepositions[1]);
                MoveEyeCameraToLocation(monsterEyepositions[1]);
            }
            else if (validatedIncomingManager[3] == 3)
            {
                monsterHUD.SetActive(true);
                changeLerpTarget(monsterEyepositions[2]);
                MoveEyeCameraToLocation(monsterEyepositions[2]);
            }
            else if (validatedIncomingManager[3] == 2)
            {
                monsterHUD.SetActive(true);
                changeLerpTarget(monsterEyepositions[3]);
                MoveEyeCameraToLocation(monsterEyepositions[3]);
            }
            else if (validatedIncomingManager[3] == 0)
                {
                    monsterHUD.SetActive(true);
                    changeLerpTarget(monsterEyepositions[4]);
                    /*                MoveEyeCameraToLocation(monsterEyepositions[4]);*/
                }
            }
        }
    void MonsterAttackInput()
    {
        if (true)
        {
            if (validatedIncomingManager[5] == 4)
            {
                MoveMonsterTail(monsterAttackPositions[0]);
                monsterAttackPositions[0].GetComponent<AttackPoint>().OnHit();
                shakeCameras(1f);
            }
            else if (validatedIncomingManager[5] == 2)
            {
                MoveMonsterTail(monsterAttackPositions[1]);
                monsterAttackPositions[1].GetComponent<AttackPoint>().OnHit();
                shakeCameras(1f);
            }
            else if (validatedIncomingManager[5] == 3)
            {
                MoveMonsterTail(monsterAttackPositions[2]);
                monsterAttackPositions[2].GetComponent<AttackPoint>().OnHit();
                shakeCameras(1f);     
            }
            else if (Input.GetKeyDown("4"))
            {

            }
        }
    }
    void MoveMonsterTail(GameObject reference)
    {
        timeUntilNextAttack = originalTimeUntilNextAttack;
/*        tail.transform.position = reference.transform.position;
        tail.transform.rotation = reference.transform.rotation;*/
    }

    //Human block
    void HumanBlockInput()
    {
        previousBoardPosition = currentBoardPosition;
        currentBoardPosition = validatedIncomingManager[4];
        if (currentBoardPosition != previousBoardPosition)
        {
            if (validatedIncomingManager[4] == 1)
            {
                MoveHumanBlock(monsterEyepositions[0]);
            }
            else if (validatedIncomingManager[4] == 4)
            {
                MoveHumanBlock(monsterEyepositions[1]);
            }
            else if (validatedIncomingManager[4] == 2)
            {
                MoveHumanBlock(monsterEyepositions[2]);
            }
            else if (validatedIncomingManager[4] == 3)
            {
                MoveHumanBlock(monsterEyepositions[3]);
            }
            else if (validatedIncomingManager[4] == 0)
            {
                for (int i = 0; i < monsterEyepositions.Length; i++)
                {
                    monsterEyepositions[i].GetComponent<Window>().isBoarded = false;
                }
/*                board.gameObject.transform.position = defaultBarricadePoint.transform.position;
                board.gameObject.transform.rotation = defaultBarricadePoint.transform.rotation;*/
                board.GetComponent<Board>().PlaySound();
                board.GetComponent<Animator>().Play("BoardRemove");

            }
        }
    }
    //Player repair
    bool ValidatePlayerPosition(string current)
    {
        if (currentPlayerPosition != current)
        {
            currentPlayerPosition = current;
            return true;
        }
        return false;
    }
    void PlayerRepairInput()
    {
        previousHumanPosition = currentHumanPosition;
        currentHumanPosition = validatedIncomingManager[6];
        if (validatedIncomingManager[6] != 0)
        {
            player.GetComponent<BoxCollider>().enabled = true;
            player.GetComponent<MeshRenderer>().enabled = true;
            playerAbsent = false;
        }

        if (currentHumanPosition != previousHumanPosition)
        {

            if (validatedIncomingManager[6] == 4)
            {
                MoveHumanRepair(monsterAttackPositions[0]);
            }
            else if (validatedIncomingManager[6] == 2)
            {
                MoveHumanRepair(monsterAttackPositions[1]);
            }
            else if (validatedIncomingManager[6] == 3)
            {
                MoveHumanRepair(monsterAttackPositions[2]);
            }
            else if (validatedIncomingManager[6] == 0)
            {
                humanCanvas.GetComponent<CanvasScript>().fadeRepair(false);
                playerAbsent = true;
                InvalidateAllDefendedPoints();
                player.GetComponent<Player>().Move();
                player.GetComponent<BoxCollider>().enabled=false;
                player.GetComponent<Animator>().Play("Human");
/*                player.GetComponent<MeshRenderer>().enabled = false;*/
                /*                MoveHumanRepair(hiddenPlayerPosition);*/
            }
        }
    }
    void changeLerpTarget(GameObject reference)
    {
        monsterEyeCamera.GetComponent<MonsterEye>().playSound();
        if (attachedEyeWall)
        {
            attachedEyeWall.GetComponent<Window>().windowLight.SetActive(false);
        }
        elapsedLerpTime = 0;
        startLerpPosition = monsterEyeCamera.transform.position;
        startLerpRotation = monsterEyeCamera.transform.eulerAngles;
        attachedEyeWall = reference;
        attachedEyeWall.GetComponent<Window>().windowLight.SetActive(true);
    }
    void LerpMonsterPosition()
    {
        elapsedLerpTime += Time.deltaTime;
        float percentageComplete = elapsedLerpTime / 0.25f;
        monsterEyeCamera.transform.position = Vector3.Lerp(startLerpPosition, attachedEyeWall.transform.position, (Mathf.SmoothStep(0, 1, percentageComplete)));
        monsterEyeCamera.transform.rotation = Quaternion.Lerp(Quaternion.Euler(startLerpRotation), attachedEyeWall.transform.rotation, (Mathf.SmoothStep(0, 1, percentageComplete)));

        monsterEyeModel.transform.position = Vector3.Lerp(startLerpPosition, attachedEyeWall.GetComponent<Window>().eyeModelPosition.transform.position, (Mathf.SmoothStep(0, 1, percentageComplete)));
        monsterEyeModel.transform.rotation = Quaternion.Lerp(Quaternion.Euler(startLerpRotation), attachedEyeWall.GetComponent<Window>().eyeModelPosition.transform.rotation, (Mathf.SmoothStep(0, 1, percentageComplete)));

    }
    IEnumerator LoadGameOver()
    {
        if (houseDestroyed)
        {
            collapse.enabled = true;

        }
        else
        {
            scream.enabled = true;
        }
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene(1);
    }

    void LightUpLEDHealth(int ID, int health)
    {
        SerialCommunications.outgoing[ID] = 0b00000000;
        for (int i=0; i<health; i++)
        {
            SerialCommunications.outgoing[ID] = (byte)(SerialCommunications.outgoing[ID] | 1<<i);
        }

    }
    void LightUpLEDWindow(int ID)
    {
        /*        SerialCommunications.outgoing[5] = (byte)(SerialCommunications.outgoing[5] & 0b00000000);
                if (ID == 99)
                {
                    return;
                }
                byte temporaryByte = SerialCommunications.outgoing[5];
                SerialCommunications.outgoing[5] = (byte)(temporaryByte | (1 << ID));*/
/*        Debug.Log(ID);
*/        if (attachedEyeWall)
        {
            switch (ID)
            {
                case 0:
                    SerialCommunications.outgoing[5] = 0b01000000;
                    break;
                case 1:
                    SerialCommunications.outgoing[5] = 0b00010000;
                    break;
                case 2:
                    SerialCommunications.outgoing[5] = 0b10000000;
                    break;
                case 3:
                    SerialCommunications.outgoing[5] = 0b00100000;
                    break;
                case 4:
                    SerialCommunications.outgoing[5] = 0b00000000;
                    break;
            }
        }
        else
        {
            SerialCommunications.outgoing[5] = 0b00000000;
        }
    }
    public void ResetStats()
    {
        monsterHealth = maxMonsterHealth;

        gameTimer = originalGameTimer;
        for(int i=0;i<monsterAttackPositions.Length; i++)
        {
/*            if (monsterAttackPositions[i].GetComponent<AttackPoint>().health > 0)
            {
                monsterAttackPositions[i].GetComponent<AttackPoint>().health = weakPointHealth;
            }*/
        }
        petrifyTimer = 0;
        monsterDamage = 0;
        petrifyTimer = 0;
        monsterAttackTimer = monsterAttackOriginalCooldown;

    }

    public void EmpowerMonster()
    {
        monsterAttackOriginalCooldown -= monsterStrengthIncrement;
        monsterPetrifyIncrement += 0.5f;
    }

    public void shakeCameras(float strength)
    {
        if (attachedEyeWall.tag=="Default Point")
        {
            shaker1.StartCoroutine(shaker1.ShakeCam(0.3f));
            shaker2.StartCoroutine(shaker1.ShakeCam(0.3f));
        }

    }
    public void HandleGreenButton()
    {
        bool playSound = true;
        if (gameWon)
        {
            ResetGame();
        }
        else if (!countDownEnabled && !paused && !gameLocked)
        {
            ResetGame();
        }
        else
        {
            DisableAllSlides();
            switch (stateProgressionTracker)
            {
                case 0:
                    inMenu = false;

                    humanCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(false);
                    monsterCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(false);

                    EnableTimer(false);
                    humanCanvas.GetComponent<CanvasScript>().FadeMenu(false);
                    monsterCanvas.GetComponent<CanvasScript>().FadeMenu(false);

                    humanCanvas.GetComponent<CanvasScript>().FadeGameUI(false);
                    humanCanvas.GetComponent<CanvasScript>().FadeInfo("You Are A <color=red>Desperate Human,</color>", "Trying to protect your home against a brutal monster.", "You must protect the house for three nights.", true);
                    monsterCanvas.GetComponent<CanvasScript>().FadeInfo("You Are A<br><color=red>Brutal Monster,</color>", "Hunting down a puny human in their home.", "You have three nights to destroy the house.", true);
                    EnableButton(true);
                    break;
                case 1:
                    humanCanvas.GetComponent<CanvasScript>().FadeGameUI(true);
                    monsterCanvas.GetComponent<CanvasScript>().FadeGameUI(true);

                    humanCanvas.GetComponent<CanvasScript>().FadeInfo(null, null, null, false);
                    monsterCanvas.GetComponent<CanvasScript>().FadeInfo(null, null, null, false);

/*                    humanCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(false);
                    monsterCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(false);*/
                    inTutorial = true;
                    gameLocked = false;


                    UpdateTutorial();

                    humanCanvas.GetComponent<CanvasScript>().ChangePracticeText("Practice defending and repairing now.");
                    monsterCanvas.GetComponent<CanvasScript>().ChangePracticeText("Practice destroying weak points now.");

                    break;

                case 2:
                    if (practiceAdvancable)
                    {
                        practiceAdvancable = false;
                        tutorialIndex_Macro++;
                        tutorialIndex = -1;
                        AdvanceTutorialSlides();
                        UpdateTutorial();

                        playSound = false;

                        monsterCanvas.GetComponent<CanvasScript>().ChangePracticeText("Practice finding and attacking undefended weak points now.");
                        humanCanvas.GetComponent<CanvasScript>().ChangePracticeText("Practice blocking the monster's vision now.");
                    }
                    break;
                case 3:
                    if (practiceAdvancable)
                    {
                        playSound = false;

                        practiceAdvancable = false;
                        tutorialIndex_Macro++;
                        tutorialIndex = -1;
                        AdvanceTutorialSlides();
                        UpdateTutorial();
                    }
                    break;
                case 4:
                    if (practiceAdvancable)
                    {
                        playSound = false;
                        practiceAdvancable = false;
                        tutorialIndex_Macro++;
                        tutorialIndex = -1;
                        AdvanceTutorialSlides();
                        UpdateTutorial();
                    }
                    break;

                case 5:

                    humanCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(false);
                    monsterCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(false);

                    EnableButton(false);

                    EnableTimer(true);
                    inTutorial = false;
                    timer.enabled = true;

                    if (tutorialGrace)
                    {
                        tutorialGrace = false;
                        for (int i = 0; i < monsterAttackPositions.Length; i++)
                        {
                            monsterAttackPositions[i].GetComponent<AttackPoint>().health = weakPointHealth;
                        }
                    }
/*                    humanCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(false);
                    monsterCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(false);*/

                    gameLocked = true;
                    countdownTimer = originalCountdown;
                    countDownEnabled = true;
                    if (nightCounter >= maxNights)
                    {
                        humanCanvas.GetComponent<CanvasScript>().FadeInfo("Final Night", "Hold out against the final attack.", null, true);
                        monsterCanvas.GetComponent<CanvasScript>().FadeInfo("Final Night", "Last chance to kill the human.", null, true);
                    }
                    else
                    {
                        humanCanvas.GetComponent<CanvasScript>().FadeInfo("Night " + nightCounter + "/" + maxNights, "If 2/3 walls are destroyed, you die.", null, true);
                        monsterCanvas.GetComponent<CanvasScript>().FadeInfo("Night " + nightCounter + "/" + maxNights, "Destroy 2/3 walls to win.", null, true);
                    }
                    EnableButton(false);

                    humanCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(false);
                    monsterCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(false);

                    break;
            }
            stateProgressionTracker++;

            if (playSound)
            {
                source.PlayOneShot(buttonClick);
            }
        }
    }
    public void DisableAllSlides()
    {

    }
    public void ResetGame()
    {
        inMenu = true;

        tutorialIndex_Macro = 0;

        countDownEnabled = false;
        countdownTimer = originalCountdown;

        for (int i = 0; i < monsterAttackPositions.Length; i++)
        {
            monsterAttackPositions[i].GetComponent<AttackPoint>().health = weakPointHealth;
        }

        tutorialGrace = true;

        timer.enabled = false;

        humanCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(true);
        monsterCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(true);

        monsterAttackOriginalCooldown = startingChargeSpeed;
        monsterPetrifyIncrement = startingPetrifySpeed;

        stateProgressionTracker = 0;
        countDownEnabled = false;

        changeLerpTarget(monsterEyepositions[4]);
        MoveEyeCameraToLocation(monsterEyepositions[monsterEyepositions.Length - 1]);

        gameWon = false;
        gameLocked = true;
        paused = true;
        introSlideVisible = false;
        inTutorial = false;
        tutorialCompleted = false;

        ResetStats();

        tutorialIndex = 0;
        nightCounter = 1;
        humanCanvas.GetComponent<CanvasScript>().FadeMenu(true);
        monsterCanvas.GetComponent<CanvasScript>().FadeMenu(true);
        humanCanvas.GetComponent<CanvasScript>().FadeInfo(null, null, null, false);
        monsterCanvas.GetComponent<CanvasScript>().FadeInfo(null, null, null, false);
        humanCanvas.GetComponent<CanvasScript>().FadeGameUI(false);
        monsterCanvas.GetComponent<CanvasScript>().FadeGameUI(false);


    }
    public void KeyboardControls()
    {
        if (Input.GetKeyDown("1"))
        {
            validatedIncomingManager[4] = 1;
        }
        else if (Input.GetKeyDown("2"))
        {
            validatedIncomingManager[4] = 2;
        }
        else if (Input.GetKeyDown("3"))
        {
            validatedIncomingManager[4] = 3;
        }
        else if (Input.GetKeyDown("4"))
        {
            validatedIncomingManager[4] = 4;
        }
        else if (Input.GetKeyDown("5"))
        {
            validatedIncomingManager[4] = 0;
        }
        if (Input.GetKeyDown("s"))
        {
            validatedIncomingManager[5] = 4;
        }
        else if (Input.GetKeyDown("d"))
        {
            validatedIncomingManager[5] = 2;
        }
        else if (Input.GetKeyDown("f"))
        {
            validatedIncomingManager[5] = 3;
        }
        else if (Input.GetKeyDown("6"))
        {
            validatedIncomingManager[6] = 4;
        }
        else if (Input.GetKeyDown("7"))
        {
            validatedIncomingManager[6] = 2;
        }
        else if (Input.GetKeyDown("8"))
        {
            validatedIncomingManager[6] = 3;
        }
        else if (Input.GetKeyDown("9"))
        {
            validatedIncomingManager[6] = 0;
        }
        else if (Input.GetKeyDown("h"))
        {
            validatedIncomingManager[3] = 1;
        }
        else if (Input.GetKeyDown("j"))
        {
            validatedIncomingManager[3] = 2;
        }
        else if (Input.GetKeyDown("k"))
        {
            validatedIncomingManager[3] = 3;
        }
        else if (Input.GetKeyDown("l"))
        {
            validatedIncomingManager[3] = 4;
        }
        else if (Input.GetKeyDown("g"))
        {
            validatedIncomingManager[3] = 0;
        }

    }
    public void TakeDamage()
    {
        humanCanvas.GetComponent<CanvasScript>().TakeDamage();
        monsterCanvas.GetComponent<CanvasScript>().TakeDamage();
    }
    public void UpdateTutorial()
    {
        humanCanvas.GetComponent<CanvasScript>().UpdateTutorialSlides();
        monsterCanvas.GetComponent<CanvasScript>().UpdateTutorialSlides();
    }
    public void EnableTimer(bool isenabled)
    {
        if (isenabled)
        {
            humanCanvas.GetComponent<CanvasScript>().timer.enabled = true;
            monsterCanvas.GetComponent<CanvasScript>().timer.enabled = true;
        }
        else
        {
            humanCanvas.GetComponent<CanvasScript>().timer.enabled = false;
            monsterCanvas.GetComponent<CanvasScript>().timer.enabled = false;
        }
    }
    public void EnableButton(bool isenabled)
    {
        if (isenabled)
        {
            humanCanvas.GetComponent<CanvasScript>().buttonBig.GetComponent<Animator>().SetBool("enabled", true);
            monsterCanvas.GetComponent<CanvasScript>().buttonBig.GetComponent<Animator>().SetBool("enabled", true);
        }
        else
        {
            humanCanvas.GetComponent<CanvasScript>().buttonBig.GetComponent<Animator>().SetBool("enabled", false);
            monsterCanvas.GetComponent<CanvasScript>().buttonBig.GetComponent<Animator>().SetBool("enabled", false);
        }
        print(monsterCanvas.GetComponent<CanvasScript>().buttonBig.GetComponent<Animator>().GetBool("enabled"));
    }
    public void AdvanceTutorialSlides()
    {
        bool playSound=true;

        int temp_index_min = 0;
        int temp_index_max = 0;

        switch (tutorialIndex_Macro)
        {
            case 0:
                temp_index_min = 0;
                temp_index_max = 3;
                break;
            case 1:
                temp_index_min = 4;
                temp_index_max = 5;
                break;
            case 2:
                temp_index_min = 6;
                temp_index_max = 6;
                break;
        }



        if (temp_index_min <= tutorialIndex && tutorialIndex <=temp_index_max)
        {
            tutorialIndex++;
        }
        else
        {
            tutorialIndex = temp_index_min;
        }

        if (tutorialIndex_Macro >= 2 && tutorialIndex >= temp_index_max)
        {
            playSound = false;
            FadeTutorialSlides(false);
            HandleGreenButton();
        }

        if (tutorialIndex == temp_index_min)
        {
            humanCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(false);
            monsterCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(false);

            practiceAdvancable = false;
            FadeTutorialSlides(true);
        }

        else if (tutorialIndex > temp_index_max && tutorialIndex_Macro<2)
        {
            humanCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(true);
            monsterCanvas.GetComponent<CanvasScript>().FadeTutorialWarning(true);

            practiceAdvancable = true;
            FadeTutorialSlides(false);
            tutorialIndex = temp_index_min-1;
        }

        print("Macro" + tutorialIndex_Macro + "index" + tutorialIndex);

        if (tutorialIndex >= temp_index_min)
        {
            UpdateTutorial();
        }
        if (playSound)
        {
            source.PlayOneShot(buttonClick);
        }


    }
    public void FadeTutorialSlides(bool fadeIn)
    {
        humanCanvas.GetComponent<CanvasScript>().FadeTutorialSlide(fadeIn);
        monsterCanvas.GetComponent<CanvasScript>().FadeTutorialSlide(fadeIn);
    }
    public void MonsterDamageFlash(Vector3 colour, string ID)
    {
        if (ID == "monster")
        {
            monsterCanvas.GetComponent<CanvasScript>().FlashScreen(new Vector3(256,0,0));
        }
        else
        {
            humanCanvas.GetComponent<CanvasScript>().FlashScreen(colour);
        }
    }
}

//Old Green Button Code
/*            if (inTutorial && tutorialIndex < 2)
            {
                tutorialIndex++;
            }
 * else if (inTutorial && tutorialIndex >= 2)
{
    ResetStats();
    inTutorial = false;
    introSlideVisible = true;
    tutorialCompleted = true;
    humanCanvas.GetComponent<CanvasScript>().FadeInfo("Night " + nightCounter + "/" + maxNights, "Survive.", null);
    monsterCanvas.GetComponent<CanvasScript>().FadeInfo("Night " + nightCounter + "/" + maxNights, "Kill.", null);
}
if (paused && !inTutorial && !tutorialCompleted && !introSlideVisible)
{
    tutorialIndex = 0;
    gameLocked = false;
    inTutorial = true;
    humanCanvas.GetComponent<CanvasScript>().FadeInfo(null, null, null);
    monsterCanvas.GetComponent<CanvasScript>().FadeInfo(null, null, null);
    humanCanvas.GetComponent<CanvasScript>().FadeGameUI();
    monsterCanvas.GetComponent<CanvasScript>().FadeGameUI();
}

if (introSlideVisible)
{
    introSlideVisible = false;
    countDownEnabled = true;
}
if (paused && tutorialCompleted)
{
    ResetStats();
    countDownEnabled = true;
}

if (gameLocked && !inTutorial && !tutorialCompleted)
{
    paused = true;

    humanCanvas.GetComponent<CanvasScript>().FadeMenu();
    monsterCanvas.GetComponent<CanvasScript>().FadeMenu();

    humanCanvas.GetComponent<CanvasScript>().FadeInfo("You Are A <color=red>Desperate Human</color>,", "Trying to protect your home against a brutal monster.", "Survive for the next 6 nights.");
    monsterCanvas.GetComponent<CanvasScript>().FadeInfo("You Are A <color=red>Brutal Monster</color>,", "Hunting down a puny human in their home.", "Kill the human in the next 6 nights.");

}*/
