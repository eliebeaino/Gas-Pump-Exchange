using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    [Header("Player Propreties")]

    [SerializeField] float footStepsVolume = 1f;
    public AudioClip[] footStepSFX;

    [Header("Movement Propreties")]
    public float moveSpeed = 10f;
    public float jumpPower = 10f;
    public float climbSpeed = 10f;
    public float knockbackInterrupt = 0.1f;
    public float knowbackPower = 10f;
    Vector2 movement;

    // State
    bool isKnockedBack = false;

    [Header("Fire Popreties")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject firePos;
    [SerializeField] float fireSpeed = 0.2f;
    Coroutine firingCoroutine;

    // Cached component references
    Rigidbody2D rb;
    Animator anim;
    Collider2D bodyCollider;
    Collider2D feetCollider;
    PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Awake()
    {
        // cache in all componenets
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerHealth.IsplayerAlive() || isKnockedBack) return;
        Run();
        Jump();
        Climb();
        Fire();
    }
    
    // If input fire (mousebutton), start fire coroutine
    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireCoroutine());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    // Instantiate bullet and fires in corresponding direction with speed
    IEnumerator FireCoroutine()
    {
        while (true)
        {
            GameObject bulletTemp = Instantiate(bullet, firePos.transform.position, Quaternion.identity);
            float bulletSpeed = bulletTemp.GetComponent<Bullet>().GetBulletSpeed();
            bool firePositionPositive = firePos.transform.position.x - transform.position.x > 0;
            if (firePositionPositive)
            {
                bulletTemp.GetComponent<Rigidbody2D>().velocity = Vector2.right * bulletSpeed;
            }
            else if (!firePositionPositive)
            {
                bulletTemp.GetComponent<Rigidbody2D>().velocity = Vector2.left * bulletSpeed;
            }
            yield return new WaitForSeconds(fireSpeed);
        }
    }

    private void Climb()
    {
        // check if touching the ladder, return if not & reset all settings
        bool touchingLadder = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (!touchingLadder)
        {
            anim.SetBool("IsClimbing", false);
            rb.gravityScale = 1;
            return;
        }

        // give vertical speed for ladder up or down and stop gravity on character
        movement.y = CrossPlatformInputManager.GetAxisRaw("Vertical") * climbSpeed;
        rb.velocity = new Vector2(rb.velocity.x, movement.y);
        rb.gravityScale = 0;

        // climb animation controller
        bool playerHasVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        anim.SetBool("IsClimbing", playerHasVerticalSpeed);
    }

    private void Jump()
    {
        // check if grounded, set speed vertical, do take off trigger followed by jumping animation
        bool grounded = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (CrossPlatformInputManager.GetButtonDown("Jump") && grounded)
        {
            // set jump velocity and triggers animation for take off, followed with jumping animation
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            anim.SetTrigger("TakeOff");
            anim.SetBool("IsJumping", true);
        }

        // if in the air without triggered jumpg, start fall animation
        else if (!grounded && !anim.GetBool("IsJumping")) anim.SetBool("IsFalling", true);


        // if back to ground, stop jumping animation, stop fall animation, trigger land animation and generate camera shake
        else if (grounded && (anim.GetBool("IsJumping") || anim.GetBool("IsFalling")) && !anim.GetBool("IsClimbing"))
        {
            anim.SetBool("IsJumping", false);
            anim.SetBool("IsFalling", false);
            anim.SetTrigger("PlayerLand");
            GetComponent<Cinemachine.CinemachineImpulseSource>().GenerateImpulse();
        }
    }

    private void Run()
    {
        // move the player from input
        if (!rb) return;
        movement.x = CrossPlatformInputManager.GetAxisRaw("Horizontal") * moveSpeed;
        rb.velocity = new Vector2(movement.x, rb.velocity.y);

        //run animation
        if (movement.x != 0) anim.SetBool("IsRun", true);
        else if (movement.x == 0) anim.SetBool("IsRun", false);

        //flip character left or right
        if (movement.x > 0) transform.localScale = new Vector2(Mathf.Sign(movement.x), 1f);
        if (movement.x < 0) transform.localScale = new Vector2(Mathf.Sign(movement.x), 1f);
    }

    public void Die()
    {
        gameObject.layer = 13;    // change the layer of player to enemy layer to trick the game's colliders
        anim.SetTrigger("Die");   // start death animation

        FindObjectOfType<LevelExit>().FadeOutGameOver();     // Start Game Over Sequence and move to game over scene
    }



    // Knockbacks player when taking damageand interrupts all actions for a brief moment
    public IEnumerator KnockBackOnDamage()
    {
        //stops all update methods to interrupt all character movements
        isKnockedBack = true;
        // change playyer to red and knockback in air
        GetComponent<SpriteRenderer>().color = Color.red;
        rb.velocity = new Vector2(Mathf.RoundToInt(-Mathf.Sign(rb.velocity.x)) * knowbackPower,
                                  Mathf.RoundToInt(-Mathf.Sign(rb.velocity.y)) * knowbackPower);
        // returns all states to default after brief moment
        yield return new WaitForSeconds(knockbackInterrupt);
        GetComponent<SpriteRenderer>().color = Color.white;
        isKnockedBack = false;
    }

    // playfootstep sounds on animation run using events
    public void PlayFootSteps()
    {
        bool grounded = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (grounded)
        {
            var chooseFootStep = UnityEngine.Random.Range(0, footStepSFX.Length);
            AudioSource.PlayClipAtPoint(footStepSFX[chooseFootStep], Camera.main.transform.position, footStepsVolume);
        }
    }
}
