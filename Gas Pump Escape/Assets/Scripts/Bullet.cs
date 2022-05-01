using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float damageAmount = 0.1f;
    [SerializeField] float bulletSpeed = 4f;
    [SerializeField] float bulletLifetime = 1f;
    [SerializeField] AudioClip[] bulletSFX;
    [SerializeField] ParticleSystem shotVFX;

    void Start()
    {
        int audioPicker = Random.Range(0, bulletSFX.Length);
        AudioSource.PlayClipAtPoint(bulletSFX[audioPicker], Camera.main.transform.position);
        Destroy(gameObject, bulletLifetime);
    }

    public float dealDamge()
    {
        return damageAmount;
    }

    public float GetBulletSpeed()
    {
        return bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) return;
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damageAmount);
        }

        ParticleSystem shotParticleVFX = Instantiate(shotVFX, transform.position, Quaternion.identity);
        Destroy(shotParticleVFX, bulletLifetime);
        Destroy(gameObject);
    }

    
}
