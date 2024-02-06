using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public GameObject EscapeMenu;
    public EscMenu EscMenuScript;
    public void TaskOnClick()
    {
        Time.timeScale = 1;
        EscapeMenu.SetActive(false);
        EscMenuScript.GetComponent<EscMenu>().MakeTrue();
    }
}
