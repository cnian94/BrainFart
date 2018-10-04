using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OneAndOneController : MonoBehaviour
{

    public Text WaitingText;

    public GameObject PlayerPrefab;
    private GameObject PlayerOne;
    private GameObject PlayerTwo;

    public GameObject platform;



    // Use this for initialization
    void Start()
    {
        OneandOneManager.oneAndOne.FartTimeEvent.AddListener(FartTime);
    }

    void FartTime()
    {
        WaitingText.gameObject.SetActive(false);
        Vector3 platformPos = platform.transform.position;
        platformPos.x = Screen.width / 2;
        platformPos.y = Screen.height / 2;
        platform = Instantiate(platform, platformPos, Quaternion.identity);
        //Debug.Log("PLAYER 1 ANSWER:" + OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[0].answer);
        //Debug.Log("PLAYER 2 ANSWER:" + OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[1].answer);
        //Debug.Log("QUESTION INDEX: " + OneandOneManager.oneAndOne.questionIndex);
        //Debug.Log("TRUE ANSWER:" + answer);
        //Debug.Log("PLAYER 1 proximity:" + Mathf.Abs((float)(answer - OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[0].answer)));
        //Debug.Log("PLAYER 2 proximity:" + Mathf.Abs((float)(answer - OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[1].answer)));
        float answer = OneandOneManager.oneAndOne.questions.questions[OneandOneManager.oneAndOne.questionIndex].answer;

        float platformWidth = platform.GetComponent<Renderer>().bounds.size.x;

        if(OneandOneManager.oneAndOne.answered && OneandOneManager.oneAndOne.oppenentAnswered) 
        {
            float playerOne_proximity = Mathf.Abs((float)(answer - OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[0].answer));
            float playerTwo_proximity = Mathf.Abs((float)(answer - OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[1].answer));

            if (playerOne_proximity < playerTwo_proximity)
            {
                Debug.Log("Player 1 will fart " + (playerTwo_proximity - playerOne_proximity) + " unit more !!");
                OneandOneManager.oneAndOne.questionIndex++;
                StartCoroutine(WaitForFartAnimation());
            }

            else
            {
                Debug.Log("Player 2 will fart " + (playerOne_proximity - playerTwo_proximity) + " unit more !!");
                OneandOneManager.oneAndOne.questionIndex++;
                StartCoroutine(WaitForFartAnimation());
            }
        }

        else
        {
            if (OneandOneManager.oneAndOne.answered)
            {
                Debug.Log("Player 1 win");
                OneandOneManager.oneAndOne.questionIndex++;
                StartCoroutine(WaitForFartAnimation());
            }

            else
            {
                Debug.Log("Player 2 win");
                OneandOneManager.oneAndOne.questionIndex++;
                StartCoroutine(WaitForFartAnimation());
            }
        }

    }

    IEnumerator WaitForFartAnimation()
    {
        yield return new WaitForSeconds(5f);
        OneandOneManager.oneAndOne.answered = false;
        OneandOneManager.oneAndOne.oppenentAnswered = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
