using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;


public class MainMenu : MonoBehaviour
{

    public VideoPlayer videoPlayer;

    public void PlayGame() {
        SceneManager.LoadScene(1);
    }

    public void PlayPremise() {
        videoPlayer.Play();
    }

}
