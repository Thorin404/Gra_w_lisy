using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitch : MonoBehaviour {

    public GameObject[] cameras;
    public Text buttonText;
    public string button;

    private int mActiveObject;

	void OnEnable () {
        buttonText.text = cameras[mActiveObject].name;
	}

    void LateUpdate()
    {
        if (Input.GetButtonDown(button))
        {
            NextCamera();
        }
    }

    public void NextCamera()
    {
        int nextactiveobject = mActiveObject + 1 >= cameras.Length ? 0 : mActiveObject + 1;

        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(i == nextactiveobject);
        }

        mActiveObject = nextactiveobject;
        buttonText.text = cameras[mActiveObject].name;
    }
}
