using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackPoint : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] Image humanHealthBar;

    float monsterDamage=50;
    public float monsterHealth;

    public bool isDefended = false;

    float timeUntilCanAttack= 0f;
    float attackCooldownTime = 1f;

    float timeUntilCanRepair = 0f;
    float repairCooldownTime = 1f;

    int repairAmount=1;
    int repairAmountDebuffed = 5;

    public int maxHealth;
    public int health;

    public bool repairing = false;

    [SerializeField] AudioSource source;

    [SerializeField] AudioClip shatter;

    [SerializeField] AudioClip weak, normal, strong, veryStrong, error;

    [SerializeField] AudioClip roar;

    [SerializeField] GameObject sprite;

    public AudioSource repairSource;

    GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        repairSource.enabled = true;
/*        repairSource.enabled = false;
*/        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        maxHealth = gameManager.GetComponent<GameManager>().weakPointHealth;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthBar.fillAmount = (float)health / (float)maxHealth;
        humanHealthBar.fillAmount = (float)health / (float)maxHealth;

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

        if (/*timeUntilCanAttack <= 0 &&*/ health>0 && gameManager.GetComponent<GameManager>().monsterDamage>=1 && timeUntilCanAttack<=0)
        {
            Instantiate(gameManager.GetComponent<GameManager>().monsterExplosion, transform.position, transform.rotation);
/*            if (isDefended)
            {
                gameManager.GetComponent<GameManager>().monsterAttackCooldownTimer = 15f;
                gameManager.GetComponent<GameManager>().stabTimer = gameManager.GetComponent<GameManager>().stabTimerCooldown;
                source.PlayOneShot(roar);
 *//*               gameManager.GetComponent<GameManager>().monsterHealth -= monsterDamage;*//*
                Debug.Log("hitted");
            }
            else*/
            {
                if (isDefended)
                {
                    //Apply debuff
                    source.PlayOneShot(error);
                    gameManager.GetComponent<GameManager>().monsterHealth--;
                }
                else
                {
                    switch (gameManager.GetComponent<GameManager>().monsterDamage)
                    {
/*                        case 1:
                            source.PlayOneShot(weak, 0.3f);
                            break;
                        case 2:
                            source.PlayOneShot(strong, 0.2f);
                            break;
                        case 3:
                            source.PlayOneShot(normal);
                            break;
                        case 4:
                            source.PlayOneShot(veryStrong);
                            break;*/
                        default:
                            source.PlayOneShot(veryStrong);
                            break;
                    }
                    health -= gameManager.GetComponent<GameManager>().attackPerHit;
                }

                gameManager.GetComponent<GameManager>().monsterAttackCooldownTimer = gameManager.GetComponent<GameManager>().monsterAttackOriginalCooldown;
                sprite.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                //gameObject.GetComponent<MeshRenderer>().enabled = false;

                timeUntilCanAttack = attackCooldownTime;

                gameManager.GetComponent<GameManager>().monsterDamage -= gameManager.GetComponent<GameManager>().attackPerHit;
            }
        }
        if (health <= 0)
        {
            health=0;
        }
    }
    private void Repair()
    {
        if (repairing && health < maxHealth && health >= 0)
        {
/*            repairSource.enabled = true;
*/
            if (gameManager.GetComponent<GameManager>().repairTimer >= gameManager.GetComponent<GameManager>().timeUntilCanRepair)
            {
                repairSource.Play();
                Instantiate(gameManager.GetComponent<GameManager>().sparks, transform.position, transform.rotation);
                gameManager.GetComponent<GameManager>().repairTimer = 0;
                if (gameManager.GetComponent<GameManager>().playerIsVisible)
                {
                    gameManager.GetComponent<GameManager>().houseHealth += repairAmountDebuffed;
                    health += repairAmountDebuffed;
                }
                else
                {
/*                    repairSource.enabled = false;
*/                    gameManager.GetComponent<GameManager>().houseHealth += repairAmount;
                    health += repairAmount;
                }
            }

            print(health + " out of " + maxHealth);
        }
        else if (health == maxHealth)
        {
/*            repairSource.enabled = false;
*/        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}
