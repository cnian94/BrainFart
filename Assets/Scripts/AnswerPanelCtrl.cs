using GameSparks.RT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerPanelCtrl : MonoBehaviour
{

    public string[] numbers = { "1", "2", "3", "4", "5", "6", "7", "8", "9", ".", "0", "x" };

    public GameObject ButtonPanel;
    public Text WaitingText;

    public Text PlayerInput;

    public float GetPlayerInput()
    {
        return float.Parse(PlayerInput.text);
    }

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
        Timer.timer.timeIsUp.AddListener(TimeIsUp);
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


    void TimeIsUp()
    {
        using (RTData data = RTData.Get())
        { 
            data.SetInt(1, 0);
            OneandOneManager.oneAndOne.GetRTSession().SendData(2, GameSparks.RT.GameSparksRT.DeliveryIntent.UNRELIABLE, data);
            transform.parent.gameObject.SetActive(false);
            OneandOneManager.oneAndOne.FartTimeEvent.Invoke();
           
        }
    }

    public void BroadcastAnswer()
    {

        OneandOneManager.oneAndOne.answered = true;

        if (!OneandOneManager.oneAndOne.oppenentAnswered)
        {
            //Opponent did not answered yet

            OneandOneManager.oneAndOne.GetPlayerByID(OneandOneManager.oneAndOne.myID).answer = GetPlayerInput();
            using (RTData data = RTData.Get())
            { // construct a packet from the hit details
                float answer = GetPlayerInput();
                data.SetFloat(1, answer);
                OneandOneManager.oneAndOne.GetRTSession().SendData(1, GameSparks.RT.GameSparksRT.DeliveryIntent.UNRELIABLE, data);
                transform.parent.gameObject.SetActive(false);
                WaitingText.gameObject.SetActive(true);
            }
        }

        if (OneandOneManager.oneAndOne.oppenentAnswered && Timer.timer.time > 0f)
        {
            using (RTData data = RTData.Get())
            { // construct a packet from the hit details
                float answer = GetPlayerInput();
                OneandOneManager.oneAndOne.GetPlayerByID(OneandOneManager.oneAndOne.myID).answer = answer;
                data.SetFloat(1, answer);
                OneandOneManager.oneAndOne.GetRTSession().SendData(1, GameSparks.RT.GameSparksRT.DeliveryIntent.UNRELIABLE, data);
                Timer.timer.allowed = false;
                transform.parent.gameObject.SetActive(false);
                OneandOneManager.oneAndOne.FartTimeEvent.Invoke();
            }
        }


    }

}
