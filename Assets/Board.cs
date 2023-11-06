using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    AudioSource src;
    [SerializeField] AudioClip thud;
    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySound()
    {
        src.PlayOneShot(thud);
    }
}
