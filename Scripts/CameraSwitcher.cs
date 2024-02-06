using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera StartCamera;
    public Camera MainCamera;
    public void ShowStartMenu()
    {
        MainCamera.enabled = false;
        StartCamera.enabled = true;
    }

    public void ShowMainCamera()
    {
        MainCamera.enabled = true;
        StartCamera.enabled = false;
    }
}
