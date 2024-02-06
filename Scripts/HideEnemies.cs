using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideEnemies : MonoBehaviour
{
    public GameObject TutorialSkele;
    void Start()
    {
        TutorialSkele.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            TutorialSkele.SetActive(true);
        }
    }
}
