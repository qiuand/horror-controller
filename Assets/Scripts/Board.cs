using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    float bufferTime;
    float originalBufferTime = 0.5f;
    AudioSource src;
    [SerializeField] AudioClip thud;
    // Start is called before the first frame update
    void Start()
    {
        bufferTime = originalBufferTime;
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bufferTime -= Time.deltaTime;
    }
    public void PlaySound()
    {
        if (bufferTime <= 0)
        {
            bufferTime = originalBufferTime;
            src.PlayOneShot(thud);
        }
    }
}
