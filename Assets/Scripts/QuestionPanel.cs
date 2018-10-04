using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.RT;
using UnityEngine.SceneManagement;

public class QuestionPanel : MonoBehaviour
{
    public GameObject AnswerPanel;

    public Text playerOneName;
    public Text playerTwoName;

    public Text QuestionText;

    public Image TimerImage;

    private void Awake()
    {
        Debug.Log("QUESTION PANEL AWAKE !!");

    }


    // Use this for initialization
    void Start()
    {
        Debug.Log("QUESTION PANEL START !!");
        playerOneName.text = OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[0].displayName;
        playerTwoName.text = OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[1].displayName;
        Debug.Log("QUESTION INDEX: " + OneandOneManager.oneAndOne.questionIndex);
        Debug.Log("QUESTION LENGTH: " + OneandOneManager.oneAndOne.questions.questions.Length);
        if (OneandOneManager.oneAndOne.questionIndex < OneandOneManager.oneAndOne.questions.questions.Length)
        {
            Debug.Log("QUESTION INDEX: " + OneandOneManager.oneAndOne.questionIndex);
            StartCoroutine(WaitForCountDown());
        }
        else
        {
            Debug.Log("Challange is end !!");
        }
    }

    IEnumerator WaitForCountDown()
    {
        yield return new WaitForSeconds(3f);
        GetQuestion();
        AnswerPanel.gameObject.SetActive(true);
        TimerImage.gameObject.SetActive(true);
    }

    void GetQuestion()
    {
        Debug.Log("Getting Question !!");
        var question = OneandOneManager.oneAndOne.questions.questions[OneandOneManager.oneAndOne.questionIndex].title;
        QuestionText.text = question;
    }


}
