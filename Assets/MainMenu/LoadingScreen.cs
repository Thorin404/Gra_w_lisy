using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

    public Sprite[] loadingImages;
    private Image mLoadingImage;

    void OnEnable()
    {
        Debug.Assert(loadingImages != null);
        mLoadingImage = GetComponent<Image>();

        mLoadingImage.sprite = loadingImages[Random.Range(0, loadingImages.Length)];
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
