using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OneAndOneController : MonoBehaviour {

    public GameObject PlayerPrefab;
    private GameObject PlayerOne;
    private GameObject PlayerTwo;

    public GameObject platform;


    // Use this for initialization
    void Start () {

        /*Vector3 platformPos = platform.transform.position;
        platformPos.x = Screen.width / 2;
        platformPos.y = Screen.height / 5;
        platform = Instantiate(platform, platformPos, Quaternion.identity);

        PlayerOne = Instantiate(PlayerPrefab, platform.transform.position, Quaternion.identity);
        PlayerOne.name = "PlayerOne";
        Vector3 playerOnePos = platform.transform.position;
        playerOnePos.x = platform.transform.position.x - 50;
        playerOnePos.y += platform.GetComponent<Renderer>().bounds.size.y / 1.5f;
        PlayerOne.transform.position = playerOnePos;

        PlayerTwo = Instantiate(PlayerPrefab, platform.transform.position, Quaternion.identity);
        Vector3 rotate = new Vector3(0, 180, 0);
        PlayerTwo.transform.Rotate(rotate);
        PlayerTwo.name = "PlayerTwo";
        Vector3 playerTwoPos = platform.transform.position;
        playerTwoPos.x = platform.transform.position.x + 50;
        playerTwoPos.y += platform.GetComponent<Renderer>().bounds.size.y / 1.5f;
        PlayerTwo.transform.position = playerTwoPos;*/

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
