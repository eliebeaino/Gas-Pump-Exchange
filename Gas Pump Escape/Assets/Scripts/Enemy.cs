using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement Propreties")]
    [SerializeField] float moveSpeed = 1f;

    [SerializeField] float delayBetweenDamage = 0.5f;
    [SerializeField] float health = 5f;
    [SerializeField] float destoryObjectTime = 5f;
    bool isAlive = true;
    // Cached component references
    Rigidbody2D rigidbody;


    // Start is called before the first frame update
    void Awake()
    {
        // cache all componenets
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;
        DirectionCheck();
        Die();
    }

    private void Die()
    {
        if (health <=0)
        {
            isAlive = false;
            GetComponent<Animator>().SetTrigger("Die");
            rigidbody.velocity = Vector2.zero;
            Destroy(gameObject.GetComponent<CapsuleCollider2D>());
            Destroy(gameObject.GetComponent<BoxCollider2D>());
            Destroy(gameObject, destoryObjectTime);
        }
    }

    // depending on direction, move the enemy
    private void DirectionCheck()
    {
        if (IsFacingRight())
        {
            rigidbody.velocity = new Vector2(moveSpeed, 0f);
        }
        else
        {
            rigidbody.velocity = new Vector2(-moveSpeed, 0f);
        }
    }

    // check direction
    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    // change direction if reached edge of platform
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            transform.localScale = new Vector2(-Mathf.Sign(rigidbody.velocity.x), 1f);
        }
    }

    // deal damage to player if collide
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<PlayerHealth>().TakeDamage();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

}
