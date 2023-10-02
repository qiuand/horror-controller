using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI petrifyText;

    float timeToPetrify=10f;
    float petrifyTimer;

    [SerializeField] Slider healthBar;

    public GameObject board;
    public GameObject player;
    public GameObject monsterEyeCamera;

    int numberOfWeakPoints = 3;
    public float weakPointHealth = 20;

    public float maxHouseHealth;
    public float houseHealth;

    public float monsterDamage=5;

    public bool playerIsVisible = false;

    [SerializeField] GameObject[] monsterEyepositions= new GameObject[4];
    [SerializeField] GameObject[] monsterAttackPositions;

    [SerializeField] GameObject defaultBarricadePoint;

    // Start is called before the first frame update
    void Start()
    {
        MoveEyeCameraToLocation(monsterEyepositions[monsterEyepositions.Length-1]);
        maxHouseHealth = weakPointHealth * numberOfWeakPoints;
        houseHealth = maxHouseHealth;
        petrifyTimer = timeToPetrify;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = houseHealth / maxHouseHealth;
        MonsterInput();
        HumanBlockInput();
        MonsterAttackInput();
        PlayerRepairInput();
        PetrifyTimer();
    }

    void PetrifyTimer()
    {
        petrifyText.text = "Time to petrification: " + System.Math.Round(petrifyTimer, 2) + " seconds";
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
        monsterEyeCamera.gameObject.transform.position = reference.transform.position;
        monsterEyeCamera.gameObject.transform.rotation = reference.transform.rotation;
    }
    void MoveHumanBlock(GameObject reference)
    {
        GameObject boardPoint = reference.transform.Find("Board Point").gameObject;
        board.gameObject.transform.position = boardPoint.transform.position;
        board.gameObject.transform.rotation = boardPoint.transform.rotation;
    }
    void MoveHumanRepair(GameObject reference)
    {
        GameObject repairPoint = reference.transform.Find("Repair Point").gameObject;
        player.gameObject.transform.position = 
            new Vector3(
                repairPoint.transform.position.x,
                repairPoint.transform.position.y+0.5f,
                repairPoint.transform.position.z);
        board.gameObject.transform.rotation =
            Quaternion.Euler(
                repairPoint.transform.rotation.x,
                repairPoint.transform.rotation.y - 180,
                repairPoint.transform.rotation.z);
    }

    //Monster eye
    void MonsterInput()
    {
        if (Input.GetKeyDown("5"))
        {
            MoveEyeCameraToLocation(monsterEyepositions[0]);
        }
        else if (Input.GetKeyDown("6"))
        {
            MoveEyeCameraToLocation(monsterEyepositions[1]);
        }
        else if (Input.GetKeyDown("7"))
        {
            MoveEyeCameraToLocation(monsterEyepositions[2]);
        }
        else if (Input.GetKeyDown("8"))
        {
            MoveEyeCameraToLocation(monsterEyepositions[3]);
        }
    }
    void MonsterAttackInput()
    {
        if (Input.GetKeyDown("1"))
        {
            monsterAttackPositions[0].GetComponent<AttackPoint>().OnHit();
        }
        else if (Input.GetKeyDown("2"))
        {
            monsterAttackPositions[1].GetComponent<AttackPoint>().OnHit();
        }
        else if (Input.GetKeyDown("3"))
        {
            monsterAttackPositions[2].GetComponent<AttackPoint>().OnHit();
        }
        else if (Input.GetKeyDown("4"))
        {

        }
    }

    //Human block
    void HumanBlockInput()
    {
        if (Input.GetKeyDown("q"))
        {
            MoveHumanBlock(monsterEyepositions[0]);
        }
        else if (Input.GetKeyDown("w"))
        {
            MoveHumanBlock(monsterEyepositions[1]);
        }
        else if (Input.GetKeyDown("e"))
        {
            MoveHumanBlock(monsterEyepositions[2]);
        }
        else if (Input.GetKeyDown("r"))
        {
            board.gameObject.transform.position = defaultBarricadePoint.transform.position;
            board.gameObject.transform.rotation = defaultBarricadePoint.transform.rotation;
        }
    }
    //Player repair
    void PlayerRepairInput()
    {
        if (Input.GetKeyDown("t"))
        {
            MoveHumanRepair(monsterAttackPositions[0]);
        }
        else if (Input.GetKeyDown("y"))
        {
            MoveHumanRepair(monsterAttackPositions[1]);
        }
        else if (Input.GetKeyDown("u"))
        {
            MoveHumanRepair(monsterAttackPositions[2]);
        }
        else if (Input.GetKeyDown("i"))
        {

        }
    }
}
