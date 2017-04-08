using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetScene : MonoBehaviour {

    public Text buttonText;
    public string buttonName;
    public string sceneName;

	void LateUpdate () {
        if (Input.GetButtonDown(buttonName))
        {
            LoadNewScene();
        }
    }

    public void LoadNewScene()
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

}
