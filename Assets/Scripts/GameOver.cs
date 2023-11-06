using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject monsterPicture;
    [SerializeField] GameObject humanPicture;

    [SerializeField] TextMeshProUGUI whoWonText;
    // Start is called before the first frame update
    void Start()
    {
        whoWonText.text = GameManager.whoWon;
        if(string.Compare(whoWonText.text, "Monster Victory")==0){
            monsterPicture.SetActive(true);
        }
        else
        {
            humanPicture.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
