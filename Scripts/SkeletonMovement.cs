using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonMovement : MonoBehaviour
{
    public Rigidbody2D RB;
    public Transform HKT;
    public HeroKnight HKCode;
    private float Direction;
    private float DirectionRandom;
    private bool CanMove;
    private bool ChasePlayer = false;
    private bool NotDead = true;
   
    void Start()
    {
        CanMove = true;
        DirectionRandom = Random.Range(1, 3);
        if (DirectionRandom == 1)
        {
            Direction = 1;
        }
        else if (DirectionRandom == 2)
        {
            Direction = -1;
        }
        RB.GetComponent<Rigidbody2D>();
        HKCode.GetComponent<HeroKnight>();
    }

    void Update()
    {
        if (CanMove && NotDead)
        {
            StartCoroutine(TurnDelay());
        }
        else if (!CanMove)
        {
            RB.velocity = new Vector2(Direction * 1, RB.velocity.y);
        }
        if (ChasePlayer)
        {
            if (HKT.transform.position.x < gameObject.transform.position.x)
            {
                Direction = -1;
            }
            else if (HKT.transform.position.x > gameObject.transform.position.x)
            {
                Direction = 1;
            }
            CanMove = false;           
        }
        if (Direction > 0 && !CanMove)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (Direction < 0 && !CanMove)
        {
            GetComponent <SpriteRenderer>().flipX = false;
        }
    }
    public void StopMoving()
    {
        RB.velocity = Vector2.zero;
        NotDead = false;
        CanMove = true;
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HKCode.KillSelf();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ChasePlayer = true;

        }
    }
    private IEnumerator TurnDelay()
    {
        CanMove = false;
        if (Direction > 0 && !CanMove)
        {
            Direction -= 2;
        }
        else if (Direction < 0 && !CanMove)
        {
            Direction += 2;
        }
        yield return new WaitForSeconds(5);
        CanMove = true;
    }
}