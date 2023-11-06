using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject sparks;
    public GameObject monsterExplosion;

    [SerializeField] GameObject monsterLight;

    GameObject attachedEyeWall;

    public bool houseDestroyed = false;

    public float stabTimerCooldown = 1.0f;
    public float stabTimer=0f;

    public float monsterMaxHealth = 100;
    public float monsterHealth;
    public float monsterAttackTimer = 2.0f;
    public float monsterAttackCooldownTimer;

    public float gameTimer = 120f;

    [SerializeField] TextMeshProUGUI petrifyText;

    float timeToPetrify=5f;
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
    float maxMonsterDamage = 10;

    float temporaryHealthCalculation=0;

    public bool playerIsVisible = false;

    public GameObject[] monsterEyepositions= new GameObject[4];
    public  GameObject[] monsterAttackPositions;

    [SerializeField] GameObject defaultBarricadePoint;

    // Start is called before the first frame update
    void Start()
    {
        monsterDamage = minMonsterDamage;
        monsterHealth = monsterMaxHealth;

        monsterAttackCooldownTimer = monsterAttackTimer;

        MoveEyeCameraToLocation(monsterEyepositions[monsterEyepositions.Length-1]);
        maxHouseHealth = weakPointHealth * numberOfWeakPoints;
        houseHealth = maxHouseHealth;
        petrifyTimer = timeToPetrify;

        for (int i = 1; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        houseDestroyed = CalculateHouseDestroyed();
        houseHealth = CalculateHouseHealth();

        CheckIfBoarded();
        ChargeMonsterDamage();
        MonsterInput();
        HumanBlockInput();
        MonsterAttackInput();
        PlayerRepairInput();
        PetrifyTimer();

        if (Input.GetKeyDown("b"))
        {
            SceneManager.LoadScene(0);
        }
    }

    void CheckIfBoarded()
    {
        if (attachedEyeWall && attachedEyeWall.GetComponent<Window>().isBoarded)
        {
            monsterLight.SetActive(false);
        }
        else
        {
            monsterLight.SetActive(true);
        }
    }
    void ChargeMonsterDamage()
    {
        if (monsterDamage < maxMonsterDamage)
        {
            monsterDamage += Time.deltaTime;
        }
        stabTimer -= Time.deltaTime;
        gameTimer -= Time.deltaTime;
        monsterAttackCooldownTimer -= Time.deltaTime * 1.5f;
    }
    bool CalculateHouseDestroyed()
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
            petrifyTimer -= Time.deltaTime;
        }
        else if (petrifyTimer < timeToPetrify)
        {
            petrifyTimer += Time.deltaTime;
        }
        if (petrifyTimer <= 0)
        {
            petrifyTimer = 0;
        }
    }
    void MoveEyeCameraToLocation(GameObject reference)
    {
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
        for (int i = 0; i < monsterAttackPositions.Length; i++)
        {
            monsterAttackPositions[i].GetComponent<AttackPoint>().isDefended = false;
        }
        reference.GetComponent<AttackPoint>().isDefended = true;

        GameObject repairPoint = reference.transform.Find("Repair Point").gameObject;
        player.gameObject.transform.position = 
            new Vector3(
                repairPoint.transform.position.x,
                repairPoint.transform.position.y+1f,
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
                MoveEyeCameraToLocation(monsterEyepositions[0]);
            }
            else if (Input.GetKeyDown("7"))
            {
                MoveEyeCameraToLocation(monsterEyepositions[1]);
            }
            else if (Input.GetKeyDown("6"))
            {
                MoveEyeCameraToLocation(monsterEyepositions[2]);
            }
            else if (Input.GetKeyDown("8"))
            {
                MoveEyeCameraToLocation(monsterEyepositions[3]);
            }
            else if (Input.GetKeyDown("9"))
            {
                MoveEyeCameraToLocation(monsterEyepositions[4]);
            }
    }
    void MonsterAttackInput()
    {
        if (/*monsterAttackCooldownTimer <= 0*/ true)
        {
            if (Input.GetKeyDown("2"))
            {
                monsterAttackPositions[0].GetComponent<AttackPoint>().OnHit();
            }
            else if (Input.GetKeyDown("3"))
            {
                monsterAttackPositions[1].GetComponent<AttackPoint>().OnHit();
            }
            else if (Input.GetKeyDown("1"))
            {
                monsterAttackPositions[2].GetComponent<AttackPoint>().OnHit();
            }
            else if (Input.GetKeyDown("4"))
            {

            }
        }
    }

    //Human block
    void HumanBlockInput()
    {
        if (Input.GetKeyDown("w"))
        {
            MoveHumanBlock(monsterEyepositions[0]);
        }
        else if (Input.GetKeyDown("q"))
        {
            MoveHumanBlock(monsterEyepositions[1]);
        }
        else if (Input.GetKeyDown("e"))
        {
            MoveHumanBlock(monsterEyepositions[2]);
        }
        else if (Input.GetKeyDown("r"))
        {
            MoveHumanBlock(monsterEyepositions[3]);
        }
        else if (Input.GetKeyDown("a"))
        {
            board.gameObject.transform.position = defaultBarricadePoint.transform.position;
            board.gameObject.transform.rotation = defaultBarricadePoint.transform.rotation;
        }
    }
    //Player repair
    void PlayerRepairInput()
    {
        if (Input.GetKeyDown("y"))
        {
            MoveHumanRepair(monsterAttackPositions[0]);
        }
        else if (Input.GetKeyDown("u"))
        {
            MoveHumanRepair(monsterAttackPositions[1]);
        }
        else if (Input.GetKeyDown("t"))
        {
            MoveHumanRepair(monsterAttackPositions[2]);
        }
        else if (Input.GetKeyDown("i"))
        {

        }
    }
}
