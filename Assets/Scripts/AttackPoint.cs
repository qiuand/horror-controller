using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    float timeUntilCanAttack= 0f;
    float attackCooldownTime = 1f;

    float timeUntilCanRepair = 0f;
    float repairCooldownTime = 2f;

    float repairAmount=5;

    float maxHealth;
    float health;

    public bool repairing = false;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip shatter;

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
        if (timeUntilCanAttack <= 0 && health>0)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        timeUntilCanAttack -= Time.deltaTime;
        timeUntilCanRepair -= Time.deltaTime;
        Repair();
    }
    public void OnHit()
    {
        if (timeUntilCanAttack <= 0 && health>0)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            source.PlayOneShot(shatter);
            timeUntilCanAttack = attackCooldownTime;
            gameManager.GetComponent<GameManager>().houseHealth -= gameManager.GetComponent<GameManager>().monsterDamage;
            health -= gameManager.GetComponent<GameManager>().monsterDamage;
        }
    }
    private void Repair()
    {
        if (repairing && timeUntilCanRepair <= 0 && health < maxHealth)
        {
            repairSource.enabled = true;
            timeUntilCanRepair = repairCooldownTime;
            health += repairAmount;
            gameManager.GetComponent<GameManager>().houseHealth += repairAmount;
            print(health + " out of " + maxHealth);
        }
        else if (health==maxHealth)
        {
            repairSource.enabled = false;
        }
    }
}
