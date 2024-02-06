using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StartMenuPlayButton : MonoBehaviour
{
    public CameraSwitcher CSScript;
    public GameObject StartPanel;
    private void Start()
    {
        Time.timeScale = 0;
        CSScript.GetComponent<CameraSwitcher>().ShowStartMenu();
    }
    public void TaskOnClick()
    {
        CSScript.GetComponent<CameraSwitcher>().ShowMainCamera();
        StartPanel.SetActive(false);
        Time.timeScale = 1;
    }
}