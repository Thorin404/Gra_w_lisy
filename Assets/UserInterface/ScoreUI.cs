using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour {

    public static ScoreUI Instance;

    public Text scoreDetailsText;
    public InputField inputField; 

	// Use this for initialization
	void Awake () {
        Instance = this;
        scoreDetailsText = GetComponentInChildren<Text>();
        inputField = GetComponentInChildren<InputField>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
