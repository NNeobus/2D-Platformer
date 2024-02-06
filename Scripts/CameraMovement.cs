using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;
    public float YOffset;
    void Update()
    {
        transform.position = player.transform.position + new Vector3(0, 1* YOffset, -5);
    }
}