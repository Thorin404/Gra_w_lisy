using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadSceneOnClick : MonoBehaviour {

    public int sceneIndex;
    public string sceneName;

    public void LoadSceneByIndex()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadSceneByName()
    {
        SceneManager.LoadScene(sceneName);
    }
}
