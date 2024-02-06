using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class ToChurch : MonoBehaviour
{
    public Volume Lighting;
    public HeroKnight PlayerScript;
    private bool PlayerIn = false;
    private void Start()
    {
        Lighting.GetComponent<Volume>();
    }
    void Update()
    {
        if (Input.GetKeyDown("w") && PlayerIn)
        {
            PlayerScript.GetComponent<HeroKnight>().StopMovement();
            SceneManager.LoadScene("Church");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerIn = true;
        }
    }
}
