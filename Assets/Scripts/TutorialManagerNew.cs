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
            "You have a blue monster eye that can <color=green>peer through windows</color> and <color=green>destroy walls.</color>"
            , monsterVideoArray[0]
            );
        monsterTutorialArray[1] = new TutorialObject
            (
            "Your goal is to destroy weak points by hitting the <color=red>X</color>s." +
            "You must completely destroy two weak points to <color=green>win."
            , monsterVideoArray[1]
            );
        monsterTutorialArray[2] = new TutorialObject
            (
            "If you hit an undefended weak point, <color=red>you deal damage to it." +
            "<br>If you hit a defended weak point, the <color=red>human will deal damage to you." +
            "<br>If the human kills you, you <color=red>waste the rest of the night regenerating.</color>" +
            "<br>Defended weak points also <color=red>gain 1 health every 4 seconds</color> — even completely destroyed ones."
            , monsterVideoArray[2]
            );
        monsterTutorialArray[3] = new TutorialObject
            (
            "To check if a weak point is defended, stick your eye on a window pointing to it." +
            "<br>If it's empty, it's <color=green> undefended.</color>" +
            "<br>If you see the human, it's <color=red> defended."
            ,
            monsterVideoArray[3]
            );
        monsterTutorialArray[4] = new TutorialObject
            (
            "But the human can also <color=red>obstruct your view</color> by blocking a single window." +
            "<color=red>How frustrating..."
            ,
            monsterVideoArray[4]
            );
        monsterTutorialArray[5] = new TutorialObject
            (
            "You have three nights to destroy two weak points.<br><br><color=green>Good luck!",
            monsterVideoArray[5]
            );
        humanTutorialArray[0] = new TutorialObject
            (
            "You have a red human piece to defend weak points, and a blue board piece to block windows.",
            humanVideoArray[0]
            );
        humanTutorialArray[1] = new TutorialObject
            (
            "If the monster completely destroys two weak points, <color=red>you lose.</color>" +
            "You must survive for three nights to <color=green>win.",
            humanVideoArray[1]
            );
        humanTutorialArray[2] = new TutorialObject
            (
            "Move your human piece to a weak point to defend it." +
            "<br>If a monster hits an <color=red>undefended</color>weak point, the monster will deal damage to it." +
            "<br>If the monster hits a <color=green>defended</color> weak point, you will deal damage to the monster." +
            "<br>If you kill the monster,<color=green> it will waste the rest of the night regenerating.</color>" +
            "<br>Defended weak points <color=green>regain 1 health every 4 seconds.</color>",
            humanVideoArray[2]
            );
        humanTutorialArray[3] = new TutorialObject
            (
            "The monster can peer through the windows to check whether a point is defended." +
            "Its range of vision is limited — it can only see where the red light shines.",
            humanVideoArray[3]
            );
        humanTutorialArray[4] = new TutorialObject
             (
             "<br><br>Block the monster’s vision with your board to stop it from finding undefended weak points.",
             humanVideoArray[4]
             );
        humanTutorialArray[5] = new TutorialObject
             (
             "Defend the walls, block the windows, and try to survive all the nights.<br><br><color=green>Good luck!",
             humanVideoArray[5]
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
