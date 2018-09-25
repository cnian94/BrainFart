using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

    public Sprite sprite_one;
    public Sprite sprite_two;
    public Sprite sprite_three;

    private float startTime;


    // Use this for initialization
    void Start () {
        startTime = Time.timeSinceLevelLoad;
    }

    void Update()
    {
        if(Time.timeSinceLevelLoad - startTime < 3)
        {
            InvokeRepeating("StartCountDown", 0f, 1f);
        }
    }

    // Update is called once per frame
    void StartCountDown () {

        if (Time.timeSinceLevelLoad - startTime >= 1 && Time.timeSinceLevelLoad - startTime < 2)
        {
            gameObject.GetComponent<Image>().sprite = sprite_two;
        }

        if (Time.timeSinceLevelLoad - startTime >= 2 && Time.timeSinceLevelLoad - startTime < 3)
        {
            gameObject.GetComponent<Image>().sprite = sprite_one;
        }

        if (Time.timeSinceLevelLoad >= 3)
        {
            Destroy(gameObject);
        }
    }
}
