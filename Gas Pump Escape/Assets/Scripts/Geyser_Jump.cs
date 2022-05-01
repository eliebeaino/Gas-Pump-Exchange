using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geyser_Jump : MonoBehaviour
{
    [SerializeField] float jumpPower;
    Collider2D playerCollider;
    bool isPlayerInside;
    float gravityInitial;



    // intital boost for player, store the intial gravity and then stop all gravity
    public void OnTriggerEnter2D(Collider2D collision)
    {
        collision.attachedRigidbody.AddForce(Vector2.up * jumpPower);
        gravityInitial = collision.attachedRigidbody.gravityScale;
        collision.attachedRigidbody.gravityScale = 0;
        isPlayerInside = true;
        playerCollider = collision;
    }

    // restores gravity
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.attachedRigidbody.gravityScale = gravityInitial;
        isPlayerInside = false;
    }

    // as long as player is inside the geyser, the player will float
    void Update()
    {
        if (isPlayerInside) playerCollider.attachedRigidbody.AddForce(Vector2.up * jumpPower);
    }
}
