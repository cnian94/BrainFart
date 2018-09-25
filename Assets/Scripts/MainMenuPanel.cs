using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;
using System.Linq;

public class MainMenuPanel : MonoBehaviour
{

    private RTSessionInfo tempRTSessionInfo;

    [SerializeField]
    private Button playButton;

    void Awake()
    {

        playButton.onClick.AddListener(() => {
            OneandOneManager.oneAndOne.FindPlayers();
        });

        MatchNotFoundMessage.Listener += OnMatchNotFound;
        MatchFoundMessage.Listener += OnMatchFound;
        //ChallengeStartedMessage.Listener += OnChallengeStarted;
    }

    void OnDestroy()
    {
        //ChallengeStartedMessage.Listener -= OnChallengeStarted;
    }

    private void OnMatchmakingSuccess(MatchmakingResponse response)
    {
        Debug.Log("Matchmaking Success !! " + response);
    }

    private void OnMatchmakingError(MatchmakingResponse response)
    {
        Debug.Log("Matchmaking Error !! " + response);
    }

    private void OnMatchFound(GameSparks.Api.Messages.MatchFoundMessage _message)
    {
        Debug.Log("Match Found!...");
        tempRTSessionInfo = new RTSessionInfo(_message);
        playButton.gameObject.SetActive(false);
        OneandOneManager.oneAndOne.StartNewRTSession(tempRTSessionInfo);
        /*StringBuilder sBuilder = new StringBuilder();
        sBuilder.AppendLine("Match Found...");
        sBuilder.AppendLine("Host URL:" + _message.Host);
        sBuilder.AppendLine("Port:" + _message.Port);
        sBuilder.AppendLine("Access Token:" + _message.AccessToken);
        sBuilder.AppendLine("MatchId:" + _message.MatchId);
        //sBuilder.AppendLine("Opponents:" + _message.Participants);
        sBuilder.AppendLine("_________________");
        sBuilder.AppendLine(); // we'll leave a space between the player-list and the match data
        foreach (GameSparks.Api.Messages.MatchFoundMessage._Participant player in _message.Participants)
        {
            sBuilder.AppendLine("Player:" + player.PeerId + " User Name:" + player.DisplayName); // add the player number and the display name to the list
        }
        playerList.text = sBuilder.ToString(); // set the string to be the player-list field */
    }

    private void OnMatchNotFound(MatchNotFoundMessage message)
    {
        Debug.Log("Match Not Found !! ");
    }

    private void OnChallengeStarted(ChallengeStartedMessage message)
    {
        Debug.Log("CHALLENGE STARTED !! " + message);
        SceneManager.LoadScene(2);
        //LoadingManager.Instance.LoadNextScene();

    }
}
