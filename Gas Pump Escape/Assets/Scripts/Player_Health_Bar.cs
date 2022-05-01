using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health_Bar : MonoBehaviour
{
    float startingHealthValue;

    [SerializeField] Image healthBar; // assign in the editor the heath block
    [SerializeField] Image damagedBar; // assign in the editor the damage block
    [SerializeField] Image healBar;  // assign in the editor the heal block

    [Header("Timer Properties")]
    [SerializeField] [Tooltip("Time to pause before bar starts decreasing/increasing.")]
    const float BAR_VARIANCE_TIMER = 1f;
    [SerializeField] [Tooltip("Speed at which the damage/heal bar decreases/increases until it reaches the current health levels.")]
    float barVarianceSpeed = 0.1f;

    [SerializeField] float damagedBarTimer;
    [SerializeField] float healBarTimer;

    // Set starting values for starting health and current level of health bars
    void Start()
    {
        startingHealthValue = FindObjectOfType<PlayerHealth>().GetComponent<PlayerHealth>().GetStartHealth(); 
        healthBar.fillAmount = damagedBar.fillAmount = healthBar.fillAmount =
            FindObjectOfType<PlayerHealth>().GetComponent<PlayerHealth>().GetHealth() / startingHealthValue;
    }

    // Constantly updating health
    void Update()
    {
        UpdateDamageBar();
        UpdateHealBar();
    }
    
    // Update health bar when damage taken
    public void TriggerDamageBar()
    {
        // Get current health value from player
        float health = FindObjectOfType<PlayerHealth>().GetComponent<PlayerHealth>().GetHealth();
        if (healBar.fillAmount > health / startingHealthValue)
        {
            healBar.fillAmount = health / startingHealthValue;
        }
        healthBar.fillAmount =  health / startingHealthValue;
        damagedBarTimer = BAR_VARIANCE_TIMER;

    }

    public void TriggerHealBar()
    {
        float health = FindObjectOfType<PlayerHealth>().GetComponent<PlayerHealth>().GetHealth();
        healBarTimer = BAR_VARIANCE_TIMER;
        healthBar.fillAmount = health / startingHealthValue;
    }

    // smooth the damage bar transition to current health
    public void UpdateDamageBar()
    {
        damagedBarTimer -= Time.deltaTime;
        if ( damagedBarTimer < 0)
        {
            if (healthBar.fillAmount < damagedBar.fillAmount)
            {
                damagedBar.fillAmount -= barVarianceSpeed * Time.deltaTime;
            }
            else
            {
                damagedBar.fillAmount = healthBar.fillAmount;
            }
        }
    }

    public void UpdateHealBar()
    {
        healBarTimer -= Time.deltaTime;
        if (healBarTimer < 0)
        {
            if (healthBar.fillAmount > healBar.fillAmount)
            {
                healBar.fillAmount += barVarianceSpeed * Time.deltaTime;
            }
            else
            {
                healBar.fillAmount = healthBar.fillAmount;
            }
        }
    }
}