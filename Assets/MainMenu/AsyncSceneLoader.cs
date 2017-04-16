using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncSceneLoader : MonoBehaviour {

    public float loadingScreenMinTime;
    public Text loadingText;

    private bool sceneIsBeingLoaded = false;

	void Start () {
		
	}
	
    public void LoadSceneAsync(int sceneIndex)
    {
        sceneIsBeingLoaded = true;
        StartCoroutine(LoadNewScene(sceneIndex));
    }

	void Update () {
        if (sceneIsBeingLoaded)
        {
            //Do stuff while scene is being loaded
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
        }
    }

    IEnumerator LoadNewScene(int sceneIndex)
    {
        //Wait for some time  
        yield return new WaitForSeconds(loadingScreenMinTime);

        AsyncOperation asyncJob = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncJob.isDone)
        {
            yield return null;
        }
    }

}
