using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class ExposureChanger : MonoBehaviour
{
    public Volume Volume;
    private bool AndThereWasLight;
    private void Start()
    {
        Volume = GetComponent<Volume>();
        AndThereWasLight = true;
    }
    private void Update()
    {
        if (!AndThereWasLight && Volume.weight > .35f)
        {
            Volume.weight -= Time.deltaTime*5;
        }
        else if (Volume.weight <= .35f)
        {
            AndThereWasLight = true;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AndThereWasLight = false;
        }
    }
}