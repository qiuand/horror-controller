using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool playerAbsent = true;

    Material material;

    public GameObject hiddenPlayerPosition;

    public float timeUntilCanRepair;
    public float repairTimer;

    public GameObject monsterHUD;

    [SerializeField] GameObject monsterEyeModel;

    [SerializeField] AudioSource heartbeat;

    float elapsedLerpTime;

    byte[] validatedIncomingManager = new byte[7];

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
    public float monsterHealth;
    public float monsterAttackTimer = 0.5f;

    float monsterAttackOriginalCooldown = 2f;
    public float monsterAttackCooldownTimer;

    bool visibleFlag;

    public float gameTimer = 0f;
    public float gameTimerMax=21600f;

    [SerializeField] TextMeshProUGUI petrifyText;

    public float timeToPetrify=5f;
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

    float originalTimeUntilNextAttack=0.5f;
    float timeUntilNextAttack;

    [SerializeField] AudioSource collapse;

    AudioSource GameManagerSource;

    [SerializeField] GameObject defaultBarricadePoint;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        GameManagerSource = GetComponent<AudioSource>();

        repairTimer = timeUntilCanRepair;

        material = player.GetComponent<MeshRenderer>().sharedMaterial;

        timeUntilNextAttack = originalTimeUntilNextAttack;

/*        monsterDamage = minMonsterDamage;
*/        monsterHealth = monsterMaxHealth;

        monsterAttackCooldownTimer = monsterAttackTimer;

        MoveEyeCameraToLocation(monsterEyepositions[monsterEyepositions.Length-1]);
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
        for(int i=0; i<monsterAttackPositions.Length; i++)
        {
            float greatestDamage = 0;
            if (8-monsterAttackPositions[i].GetComponent<AttackPoint>().health > greatestDamage)
            {
                greatestDamage = 8 - monsterAttackPositions[i].GetComponent<AttackPoint>().health;
                Debug.Log(greatestDamage);
                GameManagerSource.volume = greatestDamage*0.17f;
            }
        }
        repairTimer += Time.deltaTime;
/*        Debug.Log(repairTimer);
*/
        LightUpLEDHealth(3, monsterAttackPositions[1].GetComponent<AttackPoint>().health);
        LightUpLEDHealth(4, monsterAttackPositions[0].GetComponent<AttackPoint>().health);
        LightUpLEDHealth(6, monsterAttackPositions[2].GetComponent<AttackPoint>().health);

        if (SerialCommunications.communicationReadyFlag)
        {
            validatedIncomingManager = SerialCommunications.validatedIncoming;
            SerialCommunications.communicationReadyFlag = false;
        }
        /*        Debug.Log("Game Manager: " + validatedIncomingManager[0] + " + " + validatedIncomingManager[1] + " + " + validatedIncomingManager[2] + " + " + validatedIncomingManager[3] + " + " + validatedIncomingManager[4] + " + " + validatedIncomingManager[5] + " + " + validatedIncomingManager[6]);
        */


        monsterAttackCooldownTimer -= Time.deltaTime;

        material.SetFloat("_Float", 0+(petrifyTimer / 5f-0.3f));
        timeUntilNextAttack -= Time.deltaTime;

        CheckWin();
        houseDestroyed = CalculateHouseDestroyed();
        houseHealth = CalculateHouseHealth();

        CheckIfBoarded();

        ChargeMonsterDamage();

        MonsterInput();
        HumanBlockInput();

        LerpMonsterPosition();
        UpdateTimer();

        LightUpLEDWindow(attachedEyeWall.GetComponent<Window>().windowID);


        if (monsterDamage>=1)
        {
            MonsterAttackInput();
        }
        PlayerRepairInput();
        PetrifyTimer();

        if (Input.GetKeyDown("b"))
        {
            SceneManager.LoadScene(0);
        }
    }
    void UpdateTimer()
    {
        gameTimer+=Time.deltaTime*180f;
        int seconds = (int)(gameTimer % 60);
        int minutes = (int)(gameTimer / 60) % 60;
        int hours = (int)(gameTimer / 3600) % 24;
        timerString = string.Format("{0:0}:{1:00}", hours, minutes);

    }
    void CheckWin()
    {
        if (petrifyTimer >= timeToPetrify || CalculateHouseDestroyed())
        {
            whoWon = "Monster Victory";
            if (CalculateHouseDestroyed())
            {
                cause = "Cause: House Obliterated";
            }
            else
            {
                cause = "Cause: Lethal Gaze";
            }
            StartCoroutine(LoadGameOver());
        }
        else if (gameTimer >= gameTimerMax)
        {
            cause = "Cause: Monster Ran Out of Time";
            whoWon = "Human Victory";
            StartCoroutine(LoadGameOver());
        }
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
        if (destroyedPoints >= 1)
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
        if (playerIsVisible && !playerAbsent)
        {
            heartbeat.enabled = true;
            /*            if (!visibleFlag)
                        {
                            player.GetComponent<Player>().playSound();
                        }
                        visibleFlag = true;*/
            petrifyTimer += Time.deltaTime;
        }
        else
        {
            heartbeat.enabled = false;
            /*            visibleFlag = false;*/
            petrifyTimer -= Time.deltaTime;
        }
        if (petrifyTimer <= 0)
        {
            petrifyTimer = 0;
        }
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
        Instantiate(sparks, boardPoint.transform.position, boardPoint.transform.rotation);
        board.GetComponent<Board>().PlaySound();
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
        repairTimer = 0;
        player.GetComponent<Player>().Move();

        InvalidateAllDefendedPoints();

        reference.GetComponent<AttackPoint>().isDefended = true;
        reference.GetComponent<AttackPoint>().repairing = true;


        GameObject repairPoint = reference.transform.Find("Repair Point").gameObject;
        player.gameObject.transform.position = 
            new Vector3(
                repairPoint.transform.position.x,
                repairPoint.transform.position.y+2.5f,
                repairPoint.transform.position.z);
        player.gameObject.transform.rotation =
            Quaternion.Euler(
                repairPoint.transform.rotation.x,
                repairPoint.transform.rotation.y,
                repairPoint.transform.rotation.z);
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
                if (Input.GetKeyDown("5") || validatedIncomingManager[3] == 1)
                {
                    monsterHUD.SetActive(true);
                    changeLerpTarget(monsterEyepositions[0]);
/*                    MoveEyeCameraToLocation(monsterEyepositions[0]);
*/            }
            else if (Input.GetKeyDown("9") || validatedIncomingManager[3] == 4)
            {
                monsterHUD.SetActive(true);
                changeLerpTarget(monsterEyepositions[1]);
                MoveEyeCameraToLocation(monsterEyepositions[1]);
            }
            else if (Input.GetKeyDown("7") || validatedIncomingManager[3] == 3)
            {
                monsterHUD.SetActive(true);
                changeLerpTarget(monsterEyepositions[2]);
                MoveEyeCameraToLocation(monsterEyepositions[2]);
            }
            else if (Input.GetKeyDown("6") || validatedIncomingManager[3] == 2)
            {
                monsterHUD.SetActive(true);
                changeLerpTarget(monsterEyepositions[3]);
                MoveEyeCameraToLocation(monsterEyepositions[3]);
            }
            else if (Input.GetKeyDown("8") || validatedIncomingManager[3] == 0)
                {
                    monsterHUD.SetActive(true);
                    changeLerpTarget(monsterEyepositions[4]);
                    /*                MoveEyeCameraToLocation(monsterEyepositions[4]);*/
                }
            }
        }
    void MonsterAttackInput()
    {
        if (/*monsterAttackCooldownTimer <= 0*/ true)
        {
            if (Input.GetKeyDown("2") || validatedIncomingManager[5] == 4)
            {
                MoveMonsterTail(monsterAttackPositions[0]);
                monsterAttackPositions[0].GetComponent<AttackPoint>().OnHit();
            }
            else if (Input.GetKeyDown("1") || validatedIncomingManager[5] == 2)
            {
                MoveMonsterTail(monsterAttackPositions[1]);
                monsterAttackPositions[1].GetComponent<AttackPoint>().OnHit();
            }
            else if (Input.GetKeyDown("3") || validatedIncomingManager[5] == 3)
            {
                MoveMonsterTail(monsterAttackPositions[2]);
                monsterAttackPositions[2].GetComponent<AttackPoint>().OnHit();
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
            if (Input.GetKeyDown("q") || validatedIncomingManager[4] == 1)
            {
                MoveHumanBlock(monsterEyepositions[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Tab) || validatedIncomingManager[4] == 4)
            {
                MoveHumanBlock(monsterEyepositions[1]);
            }
            else if (Input.GetKeyDown("w") || validatedIncomingManager[4] == 2)
            {
                MoveHumanBlock(monsterEyepositions[2]);
            }
            else if (Input.GetKeyDown("e") || validatedIncomingManager[4] == 3)
            {
                MoveHumanBlock(monsterEyepositions[3]);
            }
            else if (Input.GetKeyDown("r") || validatedIncomingManager[4] == 0)
            {
                for (int i = 0; i < monsterEyepositions.Length; i++)
                {
                    monsterEyepositions[i].GetComponent<Window>().isBoarded = false;
                }
                board.gameObject.transform.position = defaultBarricadePoint.transform.position;
                board.gameObject.transform.rotation = defaultBarricadePoint.transform.rotation;
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
        if (currentHumanPosition != previousHumanPosition)
        {
            if (validatedIncomingManager[6] != 0)
            {
                player.GetComponent<BoxCollider>().enabled = true;
                player.GetComponent<MeshRenderer>().enabled = true;
                playerAbsent = false;
            }

            if (Input.GetKeyDown("u") && ValidatePlayerPosition("u") || validatedIncomingManager[6] == 4)
            {
                MoveHumanRepair(monsterAttackPositions[0]);
            }
            else if (Input.GetKeyDown("t") && ValidatePlayerPosition("t") || validatedIncomingManager[6] == 2)
            {
                MoveHumanRepair(monsterAttackPositions[1]);
            }
            else if (Input.GetKeyDown("y") && ValidatePlayerPosition("y") || validatedIncomingManager[6] == 3)
            {
                MoveHumanRepair(monsterAttackPositions[2]);
            }
            else if (Input.GetKeyDown("i") || validatedIncomingManager[6] == 0)
            {
                playerAbsent = true;
                InvalidateAllDefendedPoints();
                player.GetComponent<Player>().Move();
                player.GetComponent<BoxCollider>().enabled=false;
                player.GetComponent<MeshRenderer>().enabled = false;
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
        collapse.enabled=true;
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
}
