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

        public string title, body, body2, tippie;
        public VideoClip video, video2;
        public Texture illustration;

        public TutorialObject(string tit, string bod, string bod2, string tip, VideoClip vid, VideoClip vid2, Texture illus)
        {
            this.title = tit;
            this.body = bod;
            this.body2 = bod2;
            this.tippie = tip;
            this.video = vid;
            this.video2 = vid2;
            this.illustration = illus;
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
            "Raze this cabin to the ground.",
            "Wait to gather strength, then smash your tail against weak points repeatedly to damage them. If any of them fall to 0, the cabin will be destroyed.",
            "But the puny human can move to a point to repair it. What's more, you cannot damage a weak point they are standing at.",
            "Practice destroying weak points now.",
            monsterVideoArray[0],
            humanVideoArray[0],
            monsterImageArray[0]
            );
        monsterTutorialArray[1] = new TutorialObject
            (
            "Tear the human apart with your lethal gaze.",
            "Peer through windows with your eye. If spotted, the human will gradually take damage until they are killed.",
            "But the human can also barricade a window to avoid being spotted.",
            "Practice spotting the human now.",
            monsterVideoArray[1],
            humanVideoArray[1],
            monsterImageArray[1]
            );
        monsterTutorialArray[2] = new TutorialObject
            (
            "Practice",
            "You must completely destroy one weak point or kill the human before the timer runs out. Try using both of your abilities to corner the human into defeat!",
            " ",
            "",
            monsterVideoArray[2],
            humanVideoArray[2],
            monsterImageArray[2]
            );

        humanTutorialArray[0] = new TutorialObject
            (
            "A vicious monster wants you dead.",
            "It will try and break down the weak points to your house. If any of them are destroyed, the house will be breached.",
            "Move your human to a weak point and start repairing it. The monster also cannot attack a point you are standing at.",
            "Try repairing damage to the weak points.",
            humanVideoArray[0],
            monsterVideoArray[0],
            humanImageArray[0]
            );
        humanTutorialArray[1] = new TutorialObject
            (
            "You feel a monstrous glare.",
            "The monster can peer through your windows. If spotted, you will take constant damage until you are dead.",
            "Either move your human off the weak point, or barricade the window to block the eye.",
            "Practice avoiding the monster's gaze now.",
            humanVideoArray[1],
            monsterVideoArray[1],
            humanImageArray[1]
            );
        humanTutorialArray[2] = new TutorialObject
            (
            "Practice",
            "Keep the house from being destroyed, while avoiding the monster's lethal gaze. Survive until the timer runs out!",
            " ",
            "",
            humanVideoArray[2],
            monsterVideoArray[1],
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
                bodyUI2.text = monsterTutorialArray[index].body2;
                tipUI.text = monsterTutorialArray[index].tippie;
                videoUI.GetComponent<VideoPlayer>().clip = monsterTutorialArray[index].video;
                videoUI2.GetComponent<VideoPlayer>().clip = humanTutorialArray[index].video;
                /*                illusUI.GetComponent<RawImage>().texture = monsterTutorialArray[index].illustration;*/
                break;
            case 1:
                titleUI.text = humanTutorialArray[index].title;
                bodyUI.text = humanTutorialArray[index].body;
                bodyUI2.text = humanTutorialArray[index].body2;
                tipUI.text = humanTutorialArray[index].tippie;
                videoUI.GetComponent<VideoPlayer>().clip = humanTutorialArray[index].video;
                videoUI2.GetComponent<VideoPlayer>().clip = monsterTutorialArray[index].video;
                /*                illusUI.GetComponent<RawImage>().texture = humanTutorialArray[index].illustration;*/
                break;

        }
    }
}
