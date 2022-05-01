using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float health = 15f;
    [SerializeField] float startingHealth;
    bool isAlive = true;
    PlayerController playerController;


    // Set starting health value
    void Awake()
    {
        startingHealth = health;
        playerController = GetComponent<PlayerController>();
    }

    // used for slider to get base health on initializtion
    public float GetStartHealth()
    {
        return startingHealth;
    }

    // used for slider to get health value
    public float GetHealth()
    {
        return health;
    }

    // used to check if player is alive in outside events
    public bool IsPlayerFullHealth()
    {
        return startingHealth == health;
    }

    // heals player
    public void HealUp(float healAmount)
    {
        health = Mathf.Clamp(health + healAmount, 0, startingHealth);
        FindObjectOfType<Player_Health_Bar>().GetComponent<Player_Health_Bar>().TriggerHealBar();
    }



    // called when taking damage from other scripts, decreases health and does knockback to player
    // or runs Die method if no health
    public void TakeDamage()
    {
        health -= 1f;
        FindObjectOfType<Player_Health_Bar>().GetComponent<Player_Health_Bar>().TriggerDamageBar();
        if (health <= 0)
        {
            playerController.Die();
            ChangeAliveStatus();
        }
        else playerController.KnockBackOnDamage();
    }

    public void FogDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            playerController.Die();
            ChangeAliveStatus();
        }
        FindObjectOfType<Player_Health_Bar>().GetComponent<Player_Health_Bar>().TriggerDamageBar();
    }


    // set to false to stop all update methods from running
    public void ChangeAliveStatus()
    {
        isAlive = false;
    }

    //checks if player is alive
    public bool IsplayerAlive()
    {
        return isAlive;
    }

}
