using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
    [Header("Fog Time Propreties")]
    [SerializeField] float FogLvlPerMin = 0.5f;
    [SerializeField] float fogTimer;  //debug only
    [SerializeField] bool DecreaseFogOn = false;
    [SerializeField] bool IncreaseFogOn = true;


    [Header("Fog Levels")]
    [SerializeField] [Range(0, 1)] float startFogLvl = 0;
    [SerializeField] float currentFogLvl;  // debug only
    [SerializeField] [Range(0, 1)] float endFogLvl = 1;
    [SerializeField] Color color1 = new Color32(161 , 89 , 69, 0);
    [SerializeField] Color color2 = new Color32(212 , 180 , 143, 0);
    ParticleSystem.ColorOverLifetimeModule colorModule;
    Gradient ourGradient;

    [Header("Fog Damage Propreties")]
    [SerializeField] [Range(0, 1)] [Tooltip("Below this value, Player takes no damage, above this value, player starts to takes small amounts of damage, referenced in low Dmg")]
    float minDamageThreshold = 0.3f;
    [SerializeField] [Range(0, 1)] [Tooltip("Above this value, player starts to takes high amounts of damage, referenced in High Dmg")]
    float highDamageThreshold = 0.8f;
    [SerializeField] [Range(0, 1)] [Tooltip("Above this value, player starts to takes critical amounts of damage, referenced in Critical Dmg")]
    float maxDamageThreshold = 0.8f;

    [SerializeField] float criticalDmg = 1f;
    [SerializeField] float highDmg = 0.5f;
    [SerializeField] float lowDmg = 0.25f;

    [SerializeField] [Tooltip("Interval in Seconds for each time the player takes damage from fog")]
    float dmgIntervals = 5f;

    // cached component references
    ParticleSystem myParticleSystem;
    PlayerHealth playerHealth;


    IEnumerator Start()
    {
        // Get the system and the color module.
        myParticleSystem = GetComponentInChildren<ParticleSystem>();
        colorModule = myParticleSystem.colorOverLifetime;
        playerHealth = FindObjectOfType<PlayerHealth>().GetComponent<PlayerHealth>();

        // set fog Starting point
        currentFogLvl = startFogLvl;
        fogTimer = startFogLvl * 60 / FogLvlPerMin;
        DecreaseFogOn = false;
        IncreaseFogOn = true;

        InitializeGradient();

        // activate fog damage controller
        do
        {
            yield return StartCoroutine(TakeFogDamage());
        }
        while (playerHealth.IsplayerAlive());
    }

    private void InitializeGradient()
    {
        // Intialize the color and alpha value of the fog in the gradient
        ourGradient = new Gradient();
        ourGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(currentFogLvl, 0.0f), new GradientAlphaKey(currentFogLvl, 1.0f) }
            );

        // Apply the gradient.
        colorModule.color = ourGradient;
    }

    private void FixedUpdate()
    {
        UpdateFogLevels();
        ModifyGradientAlpha();
    }

    // Fog Density Controller
    private void UpdateFogLevels()
    {
        if      (IncreaseFogOn)  IncreaseFogLvls();
        else if (DecreaseFogOn)  DecreaseFogLvls();
    }

    // increase fog level through time
    private void IncreaseFogLvls()
    {
        fogTimer += Time.deltaTime;
        float fogFactor = FogLvlPerMin * fogTimer / 60;
        currentFogLvl = Mathf.Clamp(fogFactor, startFogLvl, endFogLvl);
        // when reached maximum, stops
        if (currentFogLvl >= endFogLvl) IncreaseFogOn = false;
    }

    // decrease fog level through time
    private void DecreaseFogLvls()
    {
        fogTimer -= Time.deltaTime;
        float fogFactor = FogLvlPerMin * fogTimer / 60;
        currentFogLvl = Mathf.Clamp(fogFactor, startFogLvl, endFogLvl);
        // when reach minimum, stops
        if (currentFogLvl <= startFogLvl) DecreaseFogOn = false;
    }

    // updates the gradient alpha channel through time of the particle effects
    private void ModifyGradientAlpha()
    {
        //ReduceAlpha();
        ourGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color1, 0.0f), new GradientColorKey(color2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(currentFogLvl/2, 0.0f), new GradientAlphaKey(currentFogLvl/2, 1.0f) }  // divide the alpha value in half to reduce screen clutter
        );

        // Apply the changed gradient.
        colorModule.color = ourGradient;
    }

    // fog controller, activates on start as long as player is alive, and loops fog damage dealer on intervals
    private IEnumerator TakeFogDamage()
    {
        DamageThreshold();
        yield return new WaitForSeconds(dmgIntervals);
    }

    // fog damage dealer called from controller
    private void DamageThreshold()
    {
        if (currentFogLvl>= maxDamageThreshold)
        {
            playerHealth.FogDamage(criticalDmg);
        }
        else if (currentFogLvl >= highDamageThreshold)
        {
            playerHealth.FogDamage(highDmg);
        }
        else if (currentFogLvl >= minDamageThreshold)
        {
            playerHealth.FogDamage(lowDmg);
        }
    }

    public float GetFogValue()
    {
        return currentFogLvl;
    } 
}



/*private void ModifyFogLvls()
    {
        //lerpedFogLvlMax = Mathf.Lerp(lerpedFogLvlMax, endFogLvl, FogLvlRateChange);
        lerpedTime = (Mathf.Lerp(lerpedTime, maxFogLvlRateChange, FogLvlRateChange));

        // calculates new fog level from a start level to a end level -- speed decreases through time on a asymptotic curve --
        currentFogLvl = Mathf.Clamp(
                        Mathf.Lerp(currentFogLvl, endFogLvl, lerpedTime),
                        0, 1); // clamp to 0 and 1 being the max alpha values that a gradient can take
    }*/   // LERP METHOD TO CONTROL FOG    -- MEMORY KEEPING OF CODE --
