using UnityEngine;
using System.Collections;
using UnityEngine.Animations;
using UnityEngine.UI;
public class HeroKnight : MonoBehaviour
{
    [SerializeField] float m_speed = 3.0f;
    [SerializeField] float m_jumpForce = 5f;
    [SerializeField] float m_rollForce = 5.0f;
    [SerializeField] int m_MaxJumps;
    [SerializeField] GameObject m_slideDust;
    public Image image;
    private Animator m_animator;
    public Rigidbody2D RB;
    public CapsuleCollider2D HitBox;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_CeilingSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool m_isWallSliding = false;
    private bool canDodge = true;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private bool Blocking = false;
    private bool Attacking = false;
    private bool KillSelfBool;
    private bool m_ceilling;
    private int m_currentJumps;
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    public float PlayerHelth;
    public float MaxPlayerHelth;
    private float m_timeSinceAttack = 0.0f;
    public float lastKeyPressTime = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 7.5f / 14.0f;
    private float m_rollCurrentTime;
    private float delay = 0;
    private float HorizontalDouble;
    private float HorizontalDoubleThresh = .25f;
    float Velocity;
    void Start()
    {
        HitBox = gameObject.GetComponent<CapsuleCollider2D>();
        m_animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_CeilingSensor = transform.Find("CeilingSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        KillSelfBool = false;
        PlayerHelth = MaxPlayerHelth;
    }
    void Update()
    {
        m_animator.SetFloat("AirSpeedY", RB.velocity.y);
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());

        if (m_isWallSliding) 
        {
            RB.velocity = new Vector2(RB.velocity.x, RB.velocity.y / 2);
            m_animator.SetBool("WallSlide", m_isWallSliding);
        }
        if (m_grounded)
        {
            m_isWallSliding = false;
            m_animator.SetBool("WallSlide", m_isWallSliding);
        }
        m_timeSinceAttack += Time.deltaTime;
        if (m_rolling)
        {
            m_rollCurrentTime += Time.deltaTime;
        }
        if (m_rollCurrentTime > m_rollDuration)
        {
            m_rolling = false;
        }
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }
        if (m_CeilingSensor.State())
        {
            m_ceilling = true;
        }
        else if (!m_CeilingSensor.State())
        {
            m_ceilling = false;
        }
        if (m_ceilling)
        {
            Dodge();
        }
        float inputX = Input.GetAxis("Horizontal");
        if (inputX > 0)
        {
            if (!m_isWallSliding)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            m_facingDirection = 1;
            HorizontalDouble += Time.deltaTime;
            if (Input.GetKeyDown("d") && HorizontalDouble < HorizontalDoubleThresh)
            {
                HorizontalDouble = 0;
                Dodge();
            }
            else if (canDodge)
            {
                RB.velocity = new Vector2(inputX * m_speed, RB.velocity.y);
                HorizontalDouble = 0;
            }
        }
        else if (inputX < 0)
        {
            if (!m_isWallSliding)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            m_facingDirection = -1;
            HorizontalDouble += Time.deltaTime;
            if (Input.GetKeyDown("a") && HorizontalDouble < HorizontalDoubleThresh)
            {
                HorizontalDouble = 0;
                Dodge();
            }
            else if (canDodge)
            {
                RB.velocity = new Vector2(inputX * m_speed, RB.velocity.y);
                HorizontalDouble = 0;
            }
        }
        if (KillSelfBool && PlayerHelth == 0)
        {
            image.fillAmount = 0;
            m_animator.SetTrigger("Death");
            Time.timeScale = 0;
            KillSelfBool = false;
        }
        else if (KillSelfBool && PlayerHelth > 0)
        {
            float target = PlayerHelth / MaxPlayerHelth;
            image.fillAmount = Mathf.SmoothDamp(image.fillAmount, target, ref Velocity, 0.3f);
            m_animator.SetTrigger("Hurt");
            KillSelfBool = false;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        if (Attacking)
        {
            if (delay < 0.5f)
            {
                m_speed = 0;
                delay += Time.deltaTime;
            }
            else if (delay >= 0.5f)
            {
                delay = 0;
                Attacking = false;
            }
        }
        else if (!Attacking)
        {
            m_speed = 3;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Block();
        }
        if (Blocking)
        {
            if (delay < 0.5f)
            {
                m_speed = 0;
                delay += Time.deltaTime;
            }
            else if (delay >= 0.5f)
            {
                delay = 0;
                Blocking = false;
            }
        }
        else if (!Blocking)
        {
            m_speed = 3;
        }
        if (Input.GetKeyDown("w"))
        {
            Jump();
        }
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
            {
                m_animator.SetInteger("AnimState", 0);
            }
        }
    }
    public void KillSelf()
    {
        KillSelfBool = true;
        PlayerHelth -= 1;
    }
    public void DestroySelf()
    {
        PlayerHelth = 0;
        KillSelfBool = true;
    }
    public void StopMovement()
    {
        m_speed = 0;
        m_jumpForce = 0;
    }
    void AE_SlideDust()
    {
        Vector3 spawnPosition;
        if (m_facingDirection == 1)
        {
            spawnPosition = m_wallSensorR2.transform.position;
        }
        else
        {
            spawnPosition = m_wallSensorL2.transform.position;
        }
        if (m_slideDust != null)
        {
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trap"))
        {
            DestroySelf();
        }
    }
    private void Attack()
    {
        Attacking = true;
        if (m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_currentAttack++;
            if (m_currentAttack > 3)
            {
                m_currentAttack = 1;
            }
            if (m_timeSinceAttack > 1.0f)
            {
                m_currentAttack = 1;
            }
            m_animator.SetTrigger("Attack" + m_currentAttack);
        }
        else if (m_timeSinceAttack > 3)
        {
            m_timeSinceAttack = 0.0f;
        }
    }
    private void Block()
    {
        Blocking = true;
        if (!m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            m_animator.SetBool("IdleBlock", false);
        }
    }
    private void Dodge()
    {
        if (canDodge && !m_rolling)
        {
            var v = new Vector2(m_facingDirection * m_rollForce, RB.velocity.y);
            m_rolling = true;
            m_animator.SetTrigger("Roll");
            RB.velocity = v;
            StartCoroutine(DodgeDelay());
        }
    }
    private IEnumerator DodgeDelay()
    {
        canDodge = false;
        HitBox.size = new Vector2(0.4260885f, 1.221816f / 3);
        HitBox.offset = new Vector2(0, .73f / 3 + .1f);
        yield return new WaitForSeconds(.5f);
        HitBox.size = new Vector2(0.4260885f, 1.221816f);
        HitBox.offset = new Vector2 (0, .73f);


        canDodge = true;
    }
    private void Jump()
    {
        if (m_grounded && !m_rolling)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            RB.velocity = new Vector2(RB.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
            m_currentJumps++;
        }
        else if (!m_grounded && m_currentJumps < m_MaxJumps)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            RB.velocity = new Vector2(RB.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
            m_currentJumps++;
            
        }
        if (m_currentJumps == m_MaxJumps || m_grounded)
        {
            m_currentJumps = 0;
        }
    }
}