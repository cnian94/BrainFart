using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class Timer : MonoBehaviour {

    public static Timer timer = null;

    public Text timeText;

    Image fillImg;
    float timeAmt = 10;
    public float time;

    public UnityEvent timeIsUp;

    public bool allowed = true;


    private void Awake()
    {

        if (timer == null)
        {
            timer = this;
        }

        else if (timer != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    // Use this for initialization
    void Start()
    {
        fillImg = this.GetComponent<Image>();
        time = timeAmt;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0 && allowed)
        {
            time -= Time.deltaTime;
            fillImg.fillAmount = time / timeAmt;
            timeText.text = "Time : " + time.ToString("F");
        }
        if(time <= 0)
        {
            timeIsUp.Invoke();
        }
    }
}
