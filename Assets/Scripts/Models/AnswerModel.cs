using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnswerModel{

    public string playerId;
    public float? answer;


    public AnswerModel(string playerId, float? answer)
    {
        this.playerId = playerId;
        this.answer = answer;
    }
}
