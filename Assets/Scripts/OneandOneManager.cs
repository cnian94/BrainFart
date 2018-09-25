using GameSparks.Api.Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.RT;
using GameSparks.Core;
using UnityEngine.SceneManagement;

public class OneandOneManager : MonoBehaviour
{

    public static OneandOneManager oneAndOne = null;

    private GameSparksRTUnity gameSparksRTUnity;

    private string challengeID;

    public UnityEvent ChallengeStarted;

    [System.Serializable]
    public class OpponentAnsweredEvent : UnityEngine.Events.UnityEvent<string> { }


    public OpponentAnsweredEvent answerEvent;

    public QuestionList questions;

    private RTSessionInfo sessionInfo;

    private void Awake()
    {
        //ChallengeStartedMessage.Listener += OnChallengeStarted;

        if (oneAndOne == null)
        {
            oneAndOne = this;
        }

        else if (oneAndOne != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

    }

    private void GetQuestionsSuccess(LogEventResponse response)
    {
        //Debug.Log("Questions: " + response.ScriptData.JSON);
        questions = QuestionList.CreateFromJSON(response.ScriptData.JSON);
        Debug.Log("Questions: " + questions.questions.Length);

    }

    private void GetQuestionsError(LogEventResponse response)
    {
        Debug.Log("Questions error: " + response.Errors);
    }


    void GetQuestions()
    {
        LogEventRequest question_request = new LogEventRequest();
        question_request.SetEventKey("QUESTIONS");
        question_request.Send(GetQuestionsSuccess, GetQuestionsError);

    }

    public string GetChallengeID()
    {
        return challengeID;
    }

    public bool IsChallengeActive { get; private set; }

    public string ChallengerName { get; private set; }

    public string ChallengerId { get; private set; }

    public string OpponentName { get; private set; }

    public string OpponentId { get; private set; }

    public int Round { get; private set; }

    //public string CurrentPlayerName { get; private set; }

    private void OnChallengeStarted(ChallengeStartedMessage message)
    {
        Debug.Log("MESSAGE:" + message.Challenge.Challenged);
        IsChallengeActive = true;
        challengeID = message.Challenge.ChallengeId;
        ChallengerName = message.Challenge.Challenger.Name;
        ChallengerId = message.Challenge.Challenger.Id;
        GetQuestions();

        using (IEnumerator<ChallengeStartedMessage._Challenge._PlayerDetail> enumer = message.Challenge.Challenged.GetEnumerator())
        {
            if (enumer.MoveNext())
            {
                Debug.Log("ENUMER: " + enumer.Current.Name);
                OpponentName = enumer.Current.Name;
                OpponentId = enumer.Current.Id;
            }
            ChallengeStarted.Invoke();
        }
    }

    public GameSparksRTUnity GetRTSession()
    {
        return gameSparksRTUnity;
    }

    public RTSessionInfo GetSessionInfo()
    {
        return sessionInfo;
    }

    #region Matchmaking Request
    /// <summary>
    /// This will request a match between as many players you have set in the match.
    /// When the max number of players is found each player will receive the MatchFound message
    /// </summary>
    public void FindPlayers()
    {
        Debug.Log("GSM| Attempting Matchmaking...");
        new GameSparks.Api.Requests.MatchmakingRequest()
            .SetMatchShortCode("1and1") // set the shortCode to be the same as the one we created in the first tutorial
            .SetSkill(0) // in this case we assume all players have skill level zero and we want anyone to be able to join so the skill level for the request is set to zero
            .Send((response) => {
                if (response.HasErrors)
                { // check for errors
                    Debug.LogError("GSM| MatchMaking Error \n" + response.Errors.JSON);
                }
            });
    }
    #endregion

    public void StartNewRTSession(RTSessionInfo _info)
    {
        Debug.Log("GSM| Creating New RT Session Instance...");
        sessionInfo = _info;
        gameSparksRTUnity = this.gameObject.AddComponent<GameSparksRTUnity>(); // Adds the RT script to the game
        // In order to create a new RT game we need a 'FindMatchResponse' //
        // This would usually come from the server directly after a successful MatchmakingRequest //
        // However, in our case, we want the game to be created only when the first player decides using a button //
        // therefore, the details from the response is passed in from the gameInfo and a mock-up of a FindMatchResponse //
        // is passed in. //
        GSRequestData mockedResponse = new GSRequestData()
                                            .AddNumber("port", (double)_info.GetPortID())
                                            .AddString("host", _info.GetHostURL())
                                            .AddString("accessToken", _info.GetAccessToken()); // construct a dataset from the game-details

        FindMatchResponse response = new FindMatchResponse(mockedResponse); // create a match-response from that data and pass it into the game-config
        // So in the game-config method we pass in the response which gives the instance its connection settings //
        // In this example, I use a lambda expression to pass in actions for 
        // OnPlayerConnect, OnPlayerDisconnect, OnReady and OnPacket actions //
        // These methods are self-explanatory, but the important one is the OnPacket Method //
        // this gets called when a packet is received //

        gameSparksRTUnity.Configure(response,
            (peerId) => { OnPlayerConnectedToGame(peerId); },
            (peerId) => { OnPlayerDisconnected(peerId); },
            (ready) => { OnRTReady(ready); },
            (packet) => { OnPacketReceived(packet); });
        gameSparksRTUnity.Connect(); // when the config is set, connect the game

    }

    private void OnPlayerConnectedToGame(int _peerId)
    {
        Debug.Log("GSM| Player Connected, " + _peerId);
    }

    private void OnPlayerDisconnected(int _peerId)
    {
        Debug.Log("GSM| Player Disconnected, " + _peerId);
    }

    private void OnRTReady(bool _isReady)
    {
        if (_isReady)
        {
            GetQuestions();
            Debug.Log("GSM| RT Session Connected...");
            SceneManager.LoadScene(2);
        }

    }


    private void OnPacketReceived(RTPacket _packet)
    {

        switch (_packet.OpCode)
        {
            // op-code 1 refers to any chat-messages being received by a player //
            // from here, we will send them to the chat-manager //
            case 1:
                Debug.Log("OPPONENT ANSWERED THE QUESTION !!");
                Debug.Log("PACKET: " + _packet.Data);
                answerEvent.Invoke(_packet.Data.GetString(1));
                break;
                /*
            case 2:

                GameController.Instance().UpdateOpponentTanks(_packet);
                break;

            case 3:
                GameController.Instance().InstantiateOpponentShells(_packet);
                break;

            case 4:
                GameController.Instance().UpdateOpponentShells(_packet);
                break;

            case 5:
                GameController.Instance().RegisterOpponentCollision(_packet);
                break;*/

        }
    }

}
