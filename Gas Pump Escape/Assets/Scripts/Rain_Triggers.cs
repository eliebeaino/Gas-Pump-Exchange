using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain_Triggers : MonoBehaviour
{
    [SerializeField] AudioSource rainSFX;
    [SerializeField] int pictchLevelChange = 6;
    [SerializeField] float pitchChangeRate = 0.1f;
    [SerializeField] float pitchChangeSpeed = 0.2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // checks if player collided with corresponding layer and then proceeds to check for corresponding tag
        // depending on pitch level and tag of collider, reduce or increase pitch of rain levels
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactables"))
        {
            if (collision.tag == ("EnterBuilding"))
            {
                if (rainSFX.pitch >= 1) StartCoroutine(ReducePitch());
            }
            float pitchLow = 1 - pictchLevelChange * pitchChangeRate; // calculate pitch level difference
            if (collision.tag == ("ExitBuilding"))
            {
                if (rainSFX.pitch <= pitchLow) StartCoroutine(IncreasePitch());
            }
        }
    }

    // slowly lowers the pitch level
    IEnumerator ReducePitch()
    {
        for (int count = 0; count < pictchLevelChange; count++)
        {
            rainSFX.pitch -= pitchChangeRate;
            yield return new WaitForSeconds(pitchChangeSpeed);
        }
    }

    // slowly increases the pitch level
    IEnumerator IncreasePitch()
    {
        for (int count = 0; count < pictchLevelChange; count++)
        {
            rainSFX.pitch += pitchChangeRate;
            yield return new WaitForSeconds(pitchChangeSpeed);
        }
    }
}
