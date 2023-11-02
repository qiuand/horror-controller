using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEye : MonoBehaviour
{
    GameObject gameManager;

    GameObject player;
    MeshRenderer playerRenderer;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager");
        player = GameObject.FindGameObjectWithTag("Player");
        playerRenderer = player.gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
        if(Physics.Raycast(transform.position, player.transform.position-transform.position, out hit))
        {
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
}
