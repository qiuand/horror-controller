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

        public string body;
        public VideoClip video, video2;

        public TutorialObject(string bod, VideoClip vid)
        {
            this.body = bod;
            this.video = vid;
        }
    }

    GameManager gameManagerScript;

    public GameObject TutorialFrame;

    public VideoClip[] monsterVideoArray = new VideoClip[3];

    public VideoClip[] humanVideoArray = new VideoClip[3];

    public TutorialObject[] monsterTutorialArray = new TutorialObject[3];
    public TutorialObject[] humanTutorialArray = new TutorialObject[3];

    public TextMeshProUGUI bodyUI;
    public VideoPlayer videoUIHuman, videoUIMonster;

    // Start is called before the first frame update
    void Start()
    {

        gameManagerScript = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

        monsterTutorialArray[0] = new TutorialObject
            (
            "Destroy the walls by hitting the red <color=red>X</color>s." +
            "<br><br>The human inside can guard a wall to <color=red>fix damage.</color><br><color=red>You will also be badly injured if you attack a guarded wall."
            , monsterVideoArray[0]
            );
        monsterTutorialArray[1] = new TutorialObject
            (
            "Stick your piece on the windows to see if the walls are guarded."+
            "<br><br><color=red>But the human might have a way to block the windows…"
            ,
            monsterVideoArray[1]
            );
        monsterTutorialArray[2] = new TutorialObject
            (
            "You have three nights to destroy two walls and crush the cabin.<br><br><color=green>Good luck!",
            monsterVideoArray[2]
            );

        humanTutorialArray[0] = new TutorialObject
            (
            "<color=red>If the monster destroys two walls, you lose.</color>" +
            "<br><br>Move your human to a wall to guard it. You will slowly <color=green>fix damage</color> and <color=green>injure the monster</color> if they attack that wall. ",
            humanVideoArray[0]
            );
        humanTutorialArray[1] = new TutorialObject
            (
            "<color=red>The monster can see where the red light shines.</color>" +
            "<br><br>Block the monster’s vision with your board to stop it from finding unguarded walls.",
            humanVideoArray[1]
            );
        humanTutorialArray[2] = new TutorialObject
            (
            "Survive three nights, or use your wits to slay the monster.<br><br><color=green>Good luck!",
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
                if (index < monsterTutorialArray.Length)
                {
                    bodyUI.text = monsterTutorialArray[index].body;
                    videoUIMonster.GetComponent<VideoPlayer>().clip = monsterTutorialArray[index].video;
                }
                break;
            case 1:
                if (index < humanTutorialArray.Length)
                {
                    bodyUI.text = humanTutorialArray[index].body;
                    videoUIHuman.GetComponent<VideoPlayer>().clip = humanTutorialArray[index].video;
                }
                break;
        }
    }
}
