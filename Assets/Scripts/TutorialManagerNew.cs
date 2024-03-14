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

    public VideoClip[] monsterVideoArray = new VideoClip[5];

    public VideoClip[] humanVideoArray = new VideoClip[5];

    public TutorialObject[] monsterTutorialArray = new TutorialObject[5];
    public TutorialObject[] humanTutorialArray = new TutorialObject[5];

    public TextMeshProUGUI bodyUI;
    public VideoPlayer videoUIHuman, videoUIMonster;

    // Start is called before the first frame update
    void Start()
    {

        gameManagerScript = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

        monsterTutorialArray[0] = new TutorialObject
            (
            "<b>You must destroy weak points by hitting the three <color=red>X</color>s.</b><br><br>" +
            "Your goal is to completely destroy two weak points <color=red>before you run out of nights.",
            monsterVideoArray[0]
            );
        monsterTutorialArray[1] = new TutorialObject
            (
            "<b>You have a blue monster eye piece that you can use to destroy weak points.</b>" +
            "<br><br>If you hold it against an undefended weak point, <color=green>you deal damage to that weak point.</color>"
            , monsterVideoArray[1]
            );
        monsterTutorialArray[2] = new TutorialObject
            (
            "However, if you hold your eye against a defended weak point, the <color=red>human inside will deal damage to you." +
            "<br><br><b>If the human kills you, you <color=red> waste the rest of the night to regenerate.</color></b>",
            monsterVideoArray[2]
            );
        monsterTutorialArray[3] = new TutorialObject
            (
            "Defended weak points also <color=red>regain 1 health every 4 seconds</color> — even ones you've completely destroyed."
            , monsterVideoArray[3]
            );
        monsterTutorialArray[4] = new TutorialObject
            (
            "<b>Use your eye piece to check for safe points to attack.</b>" +
            "<br><br>To check if a weak point is defended, stick your eye on a window pointing to it." +
            "<br><br>If it's empty, it's <color=green>undefended</color> and safe to attack." +
            "<br><br>If you see the human, it's <color=red> defended."
            ,
            monsterVideoArray[4]
            );
        monsterTutorialArray[5] = new TutorialObject
            (
            "But the human can also <color=red>block your view</color> with their board." +
            "<br><br><color=red><br>How frustrating..."
            ,
            monsterVideoArray[5]
            );
        monsterTutorialArray[6] = new TutorialObject
            (
            "You have three nights to completely destroy two weak points.<br><br><color=green><b>Good luck!",
            monsterVideoArray[6]
            );
        humanTutorialArray[0] = new TutorialObject
            (
            "<b>The monster will try to destroy your house by hitting your weak points.</b>" +
            "<br><br>If the monster completely destroys two weak points, <color=red>you lose.</color>",
            humanVideoArray[0]
            );
        humanTutorialArray[1] = new TutorialObject
            (
            "<b>You have a red human piece you can move to defend the house.</b>" +
            "<br><br>Move it to a weak point to <color=green>defend</color> it.",
            humanVideoArray[1]
            );
        humanTutorialArray[2] = new TutorialObject
            (
            "If a monster hits an <color=red>undefended</color> weak point, they will <color=red>damage that point.</color>" +
            "<br><br>But if the monster hits a <color=green>defended</color> weak point, you will <color=green>deal damage to the monster.</color>" +
            "<br><br>If you kill the monster,<color=green> it will waste a whole night to regenerate.</color>",
            humanVideoArray[2]
            );
        humanTutorialArray[3] = new TutorialObject
            (
            "Defended weak points also <color=green>regain 1 health every 4 seconds — even completely destroyed ones.</color>",
            humanVideoArray[3]
            );
        humanTutorialArray[4] = new TutorialObject
            (
            "<b>The monster can peer through the windows to check whether a point is defended.</b>" +
            "<br><br>It can use this to figure out which points are safe to attack." +
            "<br><br>But its vision is limited — it can only see where the red light can reach.",
            humanVideoArray[4]
            );
        humanTutorialArray[5] = new TutorialObject
             (
             "<b>You have a blue board piece you can use to block windows.</b>"+
             "<br><br>Block the monster’s vision to stop it from find``ing undefended weak points.",
             humanVideoArray[5]
             );
        humanTutorialArray[6] = new TutorialObject
             (
             "Defend the walls, block the windows, and try to survive for three nights.<br><br><color=green><b>Good luck!",
             humanVideoArray[6]
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
                if (index >= 0 && index < monsterTutorialArray.Length)
                {
                    bodyUI.text = monsterTutorialArray[index].body;
                    videoUIMonster.GetComponent<VideoPlayer>().clip = monsterTutorialArray[index].video;
                }
                break;
            case 1:
                if (index >= 0 && index < humanTutorialArray.Length)
                {
                    bodyUI.text = humanTutorialArray[index].body;
                    videoUIHuman.GetComponent<VideoPlayer>().clip = humanTutorialArray[index].video;
                }
                break;
        }
    }
}


/*If the monster destroys two walls, you lose.
Move your human to a wall to protect it, slowly fixing damage and hurting the monster if they try to attack it. 

Smash the walls to destroy them. If you destroy two walls, you win.
The human can stand at a wall to protect it, slowly fixing damage and hurting you if you try and attack it.

If the red light reaches you, the monster can see inside.
Block the monster’s vision with your barricade to stop it from finding unprotected walls.

Look through the windows and find unprotected walls to attack.
But the human might have a way to block the windows…

You have three nights to destroy two walls and crush the cabin. Good luck!
Survive three nights, or use your wits to slay the monster. Good luck!*/

