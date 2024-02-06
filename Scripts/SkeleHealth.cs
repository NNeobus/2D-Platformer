using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeleHealth : MonoBehaviour
{
    public Animator SkeleAnimator;
    public GameObject SkelePrefab;
    public SkeletonMovement SkeleMover;
    public float Health = 1;
    private bool CanDie = false;
    private void Start()
    {
        SkeleAnimator.GetComponent<Animator>();
    }
    private void Update()
    {
        if (Health <= 0)
        {
            SkeleMover.GetComponent<SkeletonMovement>().StopMoving();
            SkeleAnimator.SetBool("EnemyDeath", true);
            StartCoroutine(DeathDealay());
        }
        if(CanDie)
        {
            SkelePrefab.SetActive(false);
        }
    }
    private IEnumerator DeathDealay()
    {
        yield return new WaitForSeconds(1);
        CanDie = true;
    }
    public void DamageSelf()
    {
        Health -= 1;
    }
}
