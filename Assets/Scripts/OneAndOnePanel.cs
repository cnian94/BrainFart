using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.RT;

public class OneAndOnePanel : MonoBehaviour
{

    public Text playerOneName;
    public Text playerTwoName;

    public Text QuestionText;

    public int questionIndex;


    // Use this for initialization
    void Start()
    {
        playerOneName.text = OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[0].displayName;
        playerTwoName.text = OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[1].displayName;
        questionIndex = 0;
        StartCoroutine(WaitForCountDown());
    }

    IEnumerator WaitForCountDown()
    {
        yield return new WaitForSeconds(3f);
        GetQuestion();
    }

    void GetQuestion()
    {
        var question = OneandOneManager.oneAndOne.questions.questions[questionIndex].title;
        QuestionText.text = question;
        questionIndex++;
    }

    IEnumerator WaitForAnswers()
    {
        yield return new WaitForSeconds(10f);

    }
}
