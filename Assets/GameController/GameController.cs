using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Text gameStatus;
    public Text playerStatus;

    private int score = 0;


	// Use this for initialization
	void Start () {
        gameStatus.text = "Collected eggs:" + score;
	}
	
	// Update is called once per frame
	void Update () {
		if(score > 10)
        {
            gameStatus.text = "You won +" + score;
        }

	}

    public void AddScore()
    {
        Debug.Log("AddScore");
        score += 1;
        gameStatus.text = "Collected eggs:" + score;
    }

    public void SetPlayerStatus(string s)
    {
        playerStatus.text = "Player: " +s;
    }

}
