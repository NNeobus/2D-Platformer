using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackColliderScript : MonoBehaviour
{
    public SkeleHealth SHScript;
    public Image XPImage;
    private bool Attack = false;
    public float MaxXp;
    public float CurrentXp;
    private void Start()
    {
        SHScript.GetComponent<SkeleHealth>();
    }
    private void Update()
    {
        if(Attack && Input.GetMouseButtonDown(0))
        {
            SHScript.DamageSelf();
            if (SHScript.Health == 0)
            {
                CurrentXp++;
                XPImage.fillAmount = CurrentXp / MaxXp;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
       if (other.CompareTag("Enemie"))
       {
            Attack = true;
       }
    }
}