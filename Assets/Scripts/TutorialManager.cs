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
        public RawImage illustration;

        public TutorialObject(string tit, string bod, string tip, VideoClip vid, RawImage illus)
        {
            this.title = tit;
            this.body = bod;
            this.tippie = tip;
            this.video = vid;
            this.illustration = illus;
        }
    }

    public VideoClip[] monsterVideoArray = new VideoClip[3];
    public RawImage[] monsterImageArray = new RawImage[3];

    public VideoClip[] humanVideoArray = new VideoClip[3];
    public RawImage[] humanImageArray = new RawImage[3];

    public TutorialObject[] monsterTutorialArray= new TutorialObject[3];
    public TutorialObject[] humanTutorialArray = new TutorialObject[3];

    public TextMeshProUGUI titleUI, bodyUI, tipUI;
    public VideoPlayer videoUI;
    public RawImage illusUI;

    public int canvasID;

    // Start is called before the first frame update
    void Start()
    {
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
            "The monster can peer through your windows, and slowly kill you with their lethal gaze.\r\nPlace your barricade piece on a window to block the monster’s vision.\r\nNote: You can also poke the barricade off the window.\r\n",
            "Note: You can also poke the barricade off the window.",
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
        
    }
}
