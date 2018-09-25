using GameSparks.RT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AnswerPanelCtrl : MonoBehaviour
{

    public string[] numbers = { "1", "2", "3", "4", "5", "6", "7", "8", "9", ".", "0", "x" };

    public GameObject ButtonPanel;

    public Text PlayerInput;

    public string GetPlayerInput()
    {
        return PlayerInput.text;
    }

    public bool answered = false;


    public Button keyboardBtnPrefab;
    private Button keyboardBtn;
    public Button sendButton;

    // Use this for initialization
    void Start()
    {

        for (int i = 0; i < numbers.Length; i++)
        {
            keyboardBtn = Instantiate(keyboardBtnPrefab, ButtonPanel.transform);
            keyboardBtn.name = numbers[i];
            keyboardBtn.GetComponentInChildren<Text>().text = numbers[i];

            var num = numbers[i];
            keyboardBtn.onClick.AddListener(delegate { KeyboardBtnClicked(num); });
        }
        sendButton.onClick.AddListener(delegate { BroadcastAnswer(); });
        OneandOneManager.oneAndOne.answerEvent.AddListener(RecieveOpAnswer);

    }

    void RecieveOpAnswer(string answer)
    {
        Debug.Log("Oppenent Answer: " + answer);
        if (!answered)
        {
            // wait until timer
        }
        else
        {
            //send answers
        }
    }

    void KeyboardBtnClicked(string num)
    {
        Debug.Log(num + " clicked !!");
        if (num == "x" && PlayerInput.text.Length > 0)
        {
            PlayerInput.text = PlayerInput.text.Substring(0, PlayerInput.text.Length - 1);
        }
        else
        {
            PlayerInput.text += num;
        }

    }

    public void BroadcastAnswer()
    {
        answered = true;
        using (RTData data = RTData.Get())
        { // construct a packet from the hit details
            data.SetString(1, GetPlayerInput());
            OneandOneManager.oneAndOne.GetRTSession().SendData(1, GameSparks.RT.GameSparksRT.DeliveryIntent.UNRELIABLE, data);
        }
    }

}
