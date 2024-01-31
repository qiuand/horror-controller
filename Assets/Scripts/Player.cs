using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    float bufferTimer;
    float bufferTimerOriginal = 0.3f;

    GameManager gameManagerScriptReference;
    [SerializeField] Image petrifyBar;

    AudioSource src;
    [SerializeField] AudioClip move;
    [SerializeField] AudioClip scream;
    // Start is called before the first frame update
    void Start()
    {
        gameManagerScriptReference =GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        bufferTimer = bufferTimerOriginal;
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bufferTimer -= Time.deltaTime;
        petrifyBar.fillAmount = (gameManagerScriptReference.timeToPetrify-gameManagerScriptReference.petrifyTimer) / gameManagerScriptReference.timeToPetrify;
    }
    public void playSound()
    {
        src.PlayOneShot(scream);
    }
    public void Move()
    {
        if (bufferTimer <= 0)
        {
            src.PlayOneShot(move);
            bufferTimer = bufferTimerOriginal;
        }

    }
    public void PlayMoveCue()
    {
 
    }
}
