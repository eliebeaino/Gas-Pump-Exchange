using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class Lightning_Emulation : MonoBehaviour
{
    
    [Header ("Timer Properties")]
    [SerializeField] float minTimerBetweenLightningStrikes = 8f;
    [SerializeField] float maxTimerBetweenLightningStrikes = 20f;
    [SerializeField] float currentTimer;
    Animator animator;

    /* OLD lightning strike paramaters 
    [SerializeField] float lightningStrikeEmissionTime = 0.2f;
    [SerializeField] float lightIntesity = 50f;
    [SerializeField] Light2D light;
    */

    [Header("Sound")]
    [SerializeField] AudioClip[] lightningSFX;

    // Start is called before the first frame update
    void Start()
    {
        currentTimer = Random.Range(minTimerBetweenLightningStrikes, maxTimerBetweenLightningStrikes);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer -= Time.deltaTime;
        if (currentTimer < 0)
        {
            CounterRestart();
            animator.SetTrigger("Lightning");
            LightningStrikeAudio();
        }
    }

    // restarts counter for next lightning strike
    private void CounterRestart()
    {
        currentTimer = Random.Range(minTimerBetweenLightningStrikes, maxTimerBetweenLightningStrikes);
    }

    /*  OLD lightning strike functionality
    private IEnumerator LightningStrike()
    {
        light.intensity = lightIntesity;
        yield return new WaitForSeconds(lightningStrikeEmissionTime);
        light.intensity = 0;
        yield return new WaitForSeconds(lightningStrikeEmissionTime/2);
        light.intensity = lightIntesity/2;
        yield return new WaitForSeconds(lightningStrikeEmissionTime/2);
        light.intensity = 0;
    } */

    // Audio randomizer for lightningstrike
    private void LightningStrikeAudio()
    {
        int lenghtAudio = lightningSFX.Length;
        int audioClipPicker = Random.Range(0, lenghtAudio -1 );
        AudioSource.PlayClipAtPoint(lightningSFX[audioClipPicker], Camera.main.transform.position, 1f);
    }
}
