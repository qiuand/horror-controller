using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public class TutorialObject : MonoBehaviour
    {

        public string title, body, tippie;
        public VideoClip video;
        public Texture illustration;

        public TutorialObject(string tit, string bod, string tip, VideoClip vid, Texture illus)
        {
            this.title = tit;
            this.body = bod;
            this.tippie = tip;
            this.video = vid;
            this.illustration = illus;
        }
    }

    GameManager gameManagerScript;

    public GameObject TutorialFrame;

    public VideoClip[] monsterVideoArray = new VideoClip[3];
    public Texture[] monsterImageArray = new Texture[3];

    public VideoClip[] humanVideoArray = new VideoClip[3];
    public Texture[] humanImageArray = new Texture[3];

    public TutorialObject[] monsterTutorialArray= new TutorialObject[3];
    public TutorialObject[] humanTutorialArray = new TutorialObject[3];

    public TextMeshProUGUI titleUI, bodyUI, tipUI;
    public VideoPlayer videoUI;
    public RawImage illusUI;

    // Start is called before the first frame update
    void Start()
    {

        gameManagerScript = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();

        monsterTutorialArray[0] = new TutorialObject
            (
            "Attacking",
            "Smash your tail against weak points to damage the cabin. The human will move to the weak points to repair them.",
            "Note: You cannot damage weak points the human is.",
            monsterVideoArray[0],
            monsterImageArray[0]
            );
        monsterTutorialArray[1] = new TutorialObject
            (
            "Peering",
            "Stick your eye on windows to peer through them, and try to kill the human with your lethal gaze. The human can barricade any one window to block your vision.",
            "Note: You can also poke the monster eye off the window.",
            monsterVideoArray[1],
            monsterImageArray[1]
            );
        monsterTutorialArray[2] = new TutorialObject
            (
            "Practice",
            "You must completely destroy one weak point or kill the human before the timer runs out. Press the big button when ready!",
            " ",
            monsterVideoArray[2],
            monsterImageArray[2]
            );

        humanTutorialArray[0] = new TutorialObject
            (
            "Defending",
            "The monster will try and smash in the weak points to your cabin. Move your human to damaged weak points and repair them.",
            "Note: The monster cannot damage weak points you are standing at.",
            humanVideoArray[0],
            humanImageArray[0]
            );
        humanTutorialArray[1] = new TutorialObject
            (
            "Lethal Gaze",
            "The monster can peer through your windows, and slowly kill you with their lethal gaze.\r\nPlace your barricade piece on a window to block the monster’s vision.",
            "Note: You can also poke the eye off the window.",
            humanVideoArray[1],
            humanImageArray[1]
            );
        humanTutorialArray[2] = new TutorialObject
            (
            "Practice",
            "You must avoid the monster’s lethal gaze while keeping all weak points from being completely destroyed. Survive until the timer runs out!",
            " ",
            humanVideoArray[2],
            humanImageArray[2]
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
                titleUI.text = monsterTutorialArray[index].title;
                bodyUI.text = monsterTutorialArray[index].body;
                tipUI.text = monsterTutorialArray[index].tippie;
/*                videoUI.GetComponent<VideoPlayer>().clip = monsterTutorialArray[index].video;
                illusUI.GetComponent<RawImage>().texture = monsterTutorialArray[index].illustration;*/
                break;
            case 1:
                titleUI.text = humanTutorialArray[index].title;
                bodyUI.text = humanTutorialArray[index].body;
                tipUI.text = humanTutorialArray[index].tippie;
/*                videoUI.GetComponent<VideoPlayer>().clip = humanTutorialArray[index].video;
                illusUI.GetComponent<RawImage>().texture = humanTutorialArray[index].illustration;*/
                break;

        }
    }
}
