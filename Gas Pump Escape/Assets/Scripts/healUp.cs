using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healUp : MonoBehaviour
{
    [SerializeField] float healPower = 2f;
    [SerializeField] AudioClip healthConsume;
    [SerializeField] float audioVolume =1f;

    PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
    }

    // heals player with given amount, and starts destory object sequence
    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (!playerHealth.IsPlayerFullHealth())
        {
            playerHealth.HealUp(healPower);
            GetComponent<Animator>().SetTrigger("Consume");
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // Triggered from animator -- to add instantiate particle effect later and sound within destory --
    private void Destory()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(healthConsume, Camera.main.transform.position, audioVolume);
    }
}
