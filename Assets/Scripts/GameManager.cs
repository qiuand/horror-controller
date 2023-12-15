using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Material material;

    public GameObject monsterHUD;

    [SerializeField] GameObject monsterEyeModel;

    [SerializeField] AudioSource heartbeat;

    float elapsedLerpTime;


    public string timerString;

    string currentPlayerPosition = null;
    public static string whoWon = "";
    public static string cause = "";
    public GameObject tail;
    public GameObject sparks;
    public GameObject monsterExplosion;

    Vector3 startLerpPosition;
    Vector3 startLerpRotation;

/*    [SerializeField] GameObject monsterLight;*/

    GameObject attachedEyeWall;

    public bool houseDestroyed = false;

    public float stabTimerCooldown = 1.0f;
    public float stabTimer=0f;

    public float monsterMaxHealth = 100;
    public float monsterHealth;
    public float monsterAttackTimer = 0.5f;
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
    public float weakPointHealth = 20;

    public float maxHouseHealth;
    public float houseHealth;

    public float monsterDamage;

    public float minMonsterDamage = 0;
    public float maxMonsterDamage = 10;

    float temporaryHealthCalculation=0;

    public bool playerIsVisible = false;

    public GameObject[] monsterEyepositions= new GameObject[4];
    public  GameObject[] monsterAttackPositions;

    float originalTimeUntilNextAttack=0.5f;
    float timeUntilNextAttack;

    [SerializeField] GameObject defaultBarricadePoint;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        material = player.GetComponent<MeshRenderer>().sharedMaterial;

        timeUntilNextAttack = originalTimeUntilNextAttack;

        monsterDamage = minMonsterDamage;
        monsterHealth = monsterMaxHealth;

        monsterAttackCooldownTimer = monsterAttackTimer;

        MoveEyeCameraToLocation(monsterEyepositions[monsterEyepositions.Length-1]);
        maxHouseHealth = weakPointHealth * numberOfWeakPoints;
        houseHealth = maxHouseHealth;
        petrifyTimer = 0;

/*        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
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

        if (timeUntilNextAttack <= 0)
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
        if (monsterDamage < maxMonsterDamage)
        {
            monsterDamage += Time.deltaTime*2;
        }
        stabTimer -= Time.deltaTime;
        monsterAttackCooldownTimer -= Time.deltaTime * 1f;
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
    float CalculateHouseHealth()
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
        if (playerIsVisible)
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
    void MoveHumanRepair(GameObject reference)
    {
        player.GetComponent<Player>().Move();
        for (int i = 0; i < monsterAttackPositions.Length; i++)
        {
            monsterAttackPositions[i].GetComponent<AttackPoint>().isDefended = false;
        }
        reference.GetComponent<AttackPoint>().isDefended = true;

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
            if (Input.GetKeyDown("5"))
            {
                monsterHUD.SetActive(true);
                changeLerpTarget(monsterEyepositions[0]);
/*              MoveEyeCameraToLocation(monsterEyepositions[0]);*/
            }
            else if (Input.GetKeyDown("9"))
            {
                monsterHUD.SetActive(true);
                changeLerpTarget(monsterEyepositions[1]);
                /*MoveEyeCameraToLocation(monsterEyepositions[1]);*/
            }
            else if (Input.GetKeyDown("7"))
            {
                monsterHUD.SetActive(true);
                changeLerpTarget(monsterEyepositions[2]);
                /*MoveEyeCameraToLocation(monsterEyepositions[2]);*/
            }
            else if (Input.GetKeyDown("6"))
            {
                monsterHUD.SetActive(true);
                changeLerpTarget(monsterEyepositions[3]);
            /*                MoveEyeCameraToLocation(monsterEyepositions[3]);*/
            }
            else if (Input.GetKeyDown("8"))
            {
                monsterHUD.SetActive(true);
                changeLerpTarget(monsterEyepositions[4]);
            /*                MoveEyeCameraToLocation(monsterEyepositions[4]);*/
            }
        }
    void MonsterAttackInput()
    {
        if (/*monsterAttackCooldownTimer <= 0*/ true)
        {
            if (Input.GetKeyDown("2"))
            {
                MoveMonsterTail(monsterAttackPositions[0]);
                monsterAttackPositions[0].GetComponent<AttackPoint>().OnHit();
            }
            else if (Input.GetKeyDown("1"))
            {
                MoveMonsterTail(monsterAttackPositions[1]);
                monsterAttackPositions[1].GetComponent<AttackPoint>().OnHit();
            }
            else if (Input.GetKeyDown("3"))
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
        tail.transform.position = reference.transform.position;
        tail.transform.rotation = reference.transform.rotation;
    }

    //Human block
    void HumanBlockInput()
    {
        if (Input.GetKeyDown("q"))
        {
            MoveHumanBlock(monsterEyepositions[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            MoveHumanBlock(monsterEyepositions[1]);
        }
        else if (Input.GetKeyDown("w"))
        {
            MoveHumanBlock(monsterEyepositions[2]);
        }
        else if (Input.GetKeyDown("e"))
        {
            MoveHumanBlock(monsterEyepositions[3]);
        }
        else if (Input.GetKeyDown("r"))
        {
            for (int i = 0; i < monsterEyepositions.Length; i++)
            {
                monsterEyepositions[i].GetComponent<Window>().isBoarded = false;
            }
            board.gameObject.transform.position = defaultBarricadePoint.transform.position;
            board.gameObject.transform.rotation = defaultBarricadePoint.transform.rotation;
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
        if (Input.GetKeyDown("u") && ValidatePlayerPosition("u"))
        {
            MoveHumanRepair(monsterAttackPositions[0]);
        }
        else if (Input.GetKeyDown("t") && ValidatePlayerPosition("t"))
        {
            MoveHumanRepair(monsterAttackPositions[1]);
        }
        else if (Input.GetKeyDown("y") && ValidatePlayerPosition("y"))
        {
            MoveHumanRepair(monsterAttackPositions[2]);
        }
        else if (Input.GetKeyDown("i"))
        {

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
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
}
