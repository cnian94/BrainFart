using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Question {

    public string title;
    public float answer;
    public int answered;
}

[Serializable]
public class QuestionList
{   
    public Question[] questions;

    public static QuestionList CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<QuestionList>(jsonString);
    }

}
