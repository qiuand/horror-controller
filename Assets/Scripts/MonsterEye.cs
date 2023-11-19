using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEye : MonoBehaviour
{
    AudioSource src;
    [SerializeField] AudioClip move;

    float bufferTimerOriginal = 0.5f;
    float bufferTimer;

    GameObject gameManager;

    GameObject player;
    MeshRenderer playerRenderer;
    // Start is called before the first frame update
    void Start()
    {
        bufferTimer = bufferTimerOriginal;
        src = GetComponent<AudioSource>();
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        player = GameObject.FindGameObjectWithTag("Player");
        playerRenderer = player.gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        bufferTimer -= Time.deltaTime;
        RaycastHit hit;
        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
        if(Physics.Raycast(transform.position, player.transform.position-transform.position, out hit))
        {
            print(hit.collider.gameObject);
            if (hit.collider.gameObject.tag == "Player")
            {
                gameManager.GetComponent<GameManager>().playerIsVisible = true; 
            }
            else
            {
                gameManager.GetComponent<GameManager>().playerIsVisible = false;
            }
        }
    }

    public void playSound()
    {
        if (bufferTimer <= 0)
        {
            bufferTimer = bufferTimerOriginal;
            src.PlayOneShot(move);
        }
    }
    void GoToTarget()
    {

    }
}
