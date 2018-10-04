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

        if(OneandOneManager.oneAndOne.questionIndex == 0)
        {
            Vector3 playerPos = platformPos;
            playerPos.x -= 25;
            playerPos.y += 75;
            OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[0].pos = playerPos;
            Debug.Log("FIRST QUESTION !! " + OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[0].pos);
            PlayerOne = Instantiate(PlayerPrefab, playerPos, Quaternion.identity);
            PlayerOne.gameObject.GetComponent<ParticleSystem>().Stop();
            PlayerOne.name = OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[0].id;


            playerPos.x += 50;
            OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[1].pos = playerPos;
            PlayerTwo = Instantiate(PlayerPrefab, playerPos, Quaternion.identity);
            PlayerTwo.gameObject.GetComponent<ParticleSystem>().Stop();
            Vector3 rotate = new Vector3(0, 180, 0);
            PlayerTwo.transform.Rotate(rotate);
            PlayerTwo.name = OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[1].id;
        }

        else
        {
            Debug.Log("NOT FIRST QUESTION !! " + OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[0].pos);
            PlayerOne = Instantiate(PlayerPrefab, OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[0].pos, Quaternion.identity);
            PlayerOne.name = OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[0].id;

            PlayerTwo = Instantiate(PlayerPrefab, OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[1].pos, Quaternion.identity);
            Vector3 rotate = new Vector3(0, 180, 0);
            PlayerTwo.transform.Rotate(rotate);
            PlayerTwo.name = OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[1].id;


        }

        float answer = OneandOneManager.oneAndOne.questions.questions[OneandOneManager.oneAndOne.questionIndex].answer;

        float platformWidth = platform.GetComponent<Renderer>().bounds.size.x;

        if(OneandOneManager.oneAndOne.answered && OneandOneManager.oneAndOne.oppenentAnswered) 
        {
            float playerOne_proximity = Mathf.Abs((float)(answer - OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[0].answer));
            float playerTwo_proximity = Mathf.Abs((float)(answer - OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[1].answer));

            if (playerOne_proximity < playerTwo_proximity)
            {
                Debug.Log("Player 1 will fart " + (playerTwo_proximity - playerOne_proximity) + " unit more !!");

                Vector3 playerOneTargetPos = PlayerOne.transform.position;
                var movement = (platformWidth * (playerTwo_proximity - playerOne_proximity)) / 100;
                playerOneTargetPos.x += movement;

                Vector3 playerTwoTargetPos = PlayerTwo.transform.position;
                playerTwoTargetPos.x += movement;


                StartCoroutine(MoveToPosition(PlayerOne.transform, playerOneTargetPos,4f));
                StartCoroutine(MoveToPosition(PlayerTwo.transform, playerTwoTargetPos, 4f));


                OneandOneManager.oneAndOne.questionIndex++;
                //StartCoroutine(WaitForFartAnimation());
            }

            else
            {
                Debug.Log("Player 2 will fart " + (playerOne_proximity - playerTwo_proximity) + " unit more !!");

                Vector3 playerOneTargetPos = PlayerOne.transform.position;
                var movement = (platformWidth * (playerOne_proximity - playerTwo_proximity)) / 100;
                playerOneTargetPos.x -= movement;

                Vector3 playerTwoTargetPos = PlayerTwo.transform.position;
                playerTwoTargetPos.x -= movement;


                StartCoroutine(MoveToPosition(PlayerOne.transform, playerOneTargetPos, 4f));
                StartCoroutine(MoveToPosition(PlayerTwo.transform, playerTwoTargetPos, 4f));

                OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[0].pos = playerOneTargetPos;
                OneandOneManager.oneAndOne.GetSessionInfo().GetPlayerList()[1].pos = playerTwoTargetPos;


                OneandOneManager.oneAndOne.questionIndex++;
                //StartCoroutine(WaitForFartAnimation());
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

    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        transform.gameObject.GetComponent<ParticleSystem>().Play();
        SoundManager.Instance.PlayMusic("FartSound");
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }

    IEnumerator FartAnim(float percentage, float timeToFart)
    {
        SoundManager.Instance.PlayMusic("FartSound");
        yield return new WaitForSeconds(timeToFart);
        SoundManager.Instance.MusicSource.Stop();

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
