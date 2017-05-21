using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{

    public enum InterfaceGroups { INTRO, GAME, SCORE, PAUSE };
    public GameObject[] UiElements;

    public static InGameUI Instance;

    void Awake()
    {
        //Disable all interface elements
        foreach(GameObject go in UiElements)
        {
            go.SetActive(false);
        }
        Instance = this;
    }

    public void SetInterfaceGroup(InterfaceGroups ig, bool active)
    {
        //Debug.Log(" active" + active);
        if (UiElements[(int)ig] != null){
       		UiElements[(int)ig].SetActive(active);
        	Debug.Log(UiElements[(int)ig].name + " active" + active);
		}
    }

}
