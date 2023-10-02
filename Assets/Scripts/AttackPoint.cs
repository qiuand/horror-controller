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

    bool repairing = false;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip shatter;

    GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
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
            maxHealth = gameManager.GetComponent<GameManager>().houseHealth -= gameManager.GetComponent<GameManager>().monsterDamage;
            health -= gameManager.GetComponent<GameManager>().monsterDamage;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        print(1);
        if (collision.gameObject.tag == "Player")
        {
            repairing = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            repairing = false;
        }
    }
    private void Repair()
    {
        if (repairing && timeUntilCanRepair <= 0 && health < maxHealth)
        {
            timeUntilCanRepair = repairCooldownTime;
            health += repairAmount;
            gameManager.GetComponent<GameManager>().houseHealth += repairAmount;
        }
    }
}
