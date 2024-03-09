using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class TutorialManagerNew : MonoBehaviour
{
    public class TutorialObject : MonoBehaviour
    {

        public string title, body, body2, tippie;
        public VideoClip video, video2;
        public Texture illustration;

        public TutorialObject(string bod, VideoClip vid)
        {
            this.body = bod;
            this.video = vid;
        }
    }

    GameManager gameManagerScript;

    public GameObject TutorialFrame;

    public VideoClip[] monsterVideoArray = new VideoClip[3];
    public Texture[] monsterImageArray = new Texture[3];

    public VideoClip[] humanVideoArray = new VideoClip[3];
    public Texture[] humanImageArray = new Texture[3];

    public TutorialObject[] monsterTutorialArray = new TutorialObject[3];
    public TutorialObject[] humanTutorialArray = new TutorialObject[3];

    public TextMeshProUGUI titleUI, bodyUI, bodyUI2, tipUI;
    public VideoPlayer videoUI, videoUI2;
    public RawImage illusUI;

    // Start is called before the first frame update
    void Start()
    {

        gameManagerScript = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

        monsterTutorialArray[0] = new TutorialObject
            (
            "Smash the walls to destroy them. If you destroy two walls, you win." +
            "But the human can stand at a wall to protect it, slowly fixing damage and hurting you if you try and attack it."
            , monsterVideoArray[0]
            );
        monsterTutorialArray[1] = new TutorialObject
            (
            "Look through the windows and find unprotected walls to attack."+
            "But the human might have a way to block the windows…"
            ,
            monsterVideoArray[1]
            );
        monsterTutorialArray[2] = new TutorialObject
            (
            "You have three nights to destroy two walls and crush the cabin.Good luck!",
            monsterVideoArray[2]
            );

        humanTutorialArray[0] = new TutorialObject
            (
            "If the monster destroys two walls, you lose." +
            "Move your human to a wall to protect it, slowly fixing damage and hurting the monster if they try to attack it.",
            humanVideoArray[0]
            );
        humanTutorialArray[1] = new TutorialObject
            (
            "If the red light reaches you, the monster can see inside." +
            "Block the monster’s vision with your barricade to stop it from finding unprotected walls.",
            humanVideoArray[1]
            );
        humanTutorialArray[2] = new TutorialObject
            (
            "Survive three nights, or use your wits to slay the monster. Good luck!",
            humanVideoArray[2]
            );

    }

    // Update is called once per frame
    void Update()
    {
        if (!TutorialFrame.activeInHierarchy && gameManagerScript.inTutorial)
        {
            TutorialFrame.SetActive(true);
        }
        else if (!gameManagerScript.inTutorial)
        {
            TutorialFrame.SetActive(false);
        }
    }

    public void DisplaySlide(int index, int canvasID)
    {
        switch (canvasID)
        {
            case 0:
                bodyUI.text = monsterTutorialArray[index].body;
                videoUI.GetComponent<VideoPlayer>().clip = monsterTutorialArray[index].video;
                break;
            case 1:
                bodyUI.text = humanTutorialArray[index].body;
                videoUI.GetComponent<VideoPlayer>().clip = humanTutorialArray[index].video;
                break;

        }
    }
}
