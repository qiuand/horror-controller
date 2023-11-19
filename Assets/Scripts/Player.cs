using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float bufferTimer;
    float bufferTimerOriginal = 0.5f;

    AudioSource src;
    [SerializeField] AudioClip move;
    [SerializeField] AudioClip scream;
    // Start is called before the first frame update
    void Start()
    {
        bufferTimer = bufferTimerOriginal;
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bufferTimer -= Time.deltaTime;
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
}
