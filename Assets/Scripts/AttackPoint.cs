using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackPoint : MonoBehaviour
{
    [SerializeField] Image healthBar;

    float monsterDamage=25;
    public float monsterHealth;

    public bool isDefended = false;

    float timeUntilCanAttack= 0f;
    float attackCooldownTime = 1f;

    float timeUntilCanRepair = 0f;
    float repairCooldownTime = 2f;

    float repairAmount=5;
    float repairAmountDebuffed = 5f;

    float maxHealth;
    float health;

    public bool repairing = false;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip shatter;
    [SerializeField] AudioClip roar;

    [SerializeField] GameObject sprite;

    public AudioSource repairSource;

    GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        repairSource.enabled = false;
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        maxHealth = gameManager.GetComponent<GameManager>().weakPointHealth;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = health / maxHealth;

        if (timeUntilCanAttack <= 0 && health>0)
        {
            sprite.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        timeUntilCanAttack -= Time.deltaTime;
        timeUntilCanRepair -= Time.deltaTime;
        Repair();
    }
    public void OnHit()
    {
        gameManager.GetComponent<GameManager>().monsterAttackCooldownTimer = gameManager.GetComponent<GameManager>().monsterAttackTimer;

        if (timeUntilCanAttack <= 0 && health>0)
        {
            if (isDefended)
            {
                gameManager.GetComponent<GameManager>().stabTimer = gameManager.GetComponent<GameManager>().stabTimerCooldown;
                source.PlayOneShot(roar);
                gameManager.GetComponent<GameManager>().monsterHealth -= monsterDamage;
                Debug.Log("hitted");
            }
            else
            {
                sprite.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                //gameObject.GetComponent<MeshRenderer>().enabled = false;
                source.PlayOneShot(shatter);
                timeUntilCanAttack = attackCooldownTime;
                gameManager.GetComponent<GameManager>().houseHealth -= gameManager.GetComponent<GameManager>().monsterDamage;
                health -= gameManager.GetComponent<GameManager>().monsterDamage;
            }
        }
    }
    private void Repair()
    {
        if (repairing && timeUntilCanRepair <= 0 && health < maxHealth)
        {
            repairSource.enabled = true;
            timeUntilCanRepair = repairCooldownTime;
            if (gameManager.GetComponent<GameManager>().playerIsVisible)
            {
                gameManager.GetComponent<GameManager>().houseHealth += repairAmountDebuffed;
                health += repairAmountDebuffed;
            }
            else
            {
                gameManager.GetComponent<GameManager>().houseHealth += repairAmount;
                health += repairAmount;
            }

            print(health + " out of " + maxHealth);
        }
        else if (health==maxHealth)
        {
            repairSource.enabled = false;
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}
