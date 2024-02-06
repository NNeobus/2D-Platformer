using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscMenu : MonoBehaviour
{
    public GameObject EscapeMenu;
    private bool OpenOrClose;
    void Start()
    {
        OpenOrClose = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (OpenOrClose && Input.GetKeyDown("escape"))
        {
            OpenMenu();
        }
        else if (!OpenOrClose && Input.GetKeyDown("escape"))
        {
            CloseMenu();
        }
    }
    private void OpenMenu()
    {
        EscapeMenu.SetActive(true);
        OpenOrClose = false;
        Time.timeScale = 0f;
    }
    private void CloseMenu()
    {
        EscapeMenu.SetActive(false);
        OpenOrClose = true;
        Time.timeScale = 1;
    }
    public void MakeTrue()
    {
        OpenOrClose = true;
    }
}
