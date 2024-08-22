using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoCollisionAgaintOtherPlayer1 : MonoBehaviour
{
    private Collider playerCollider;

    void Start()
    {
        int playerLayer = LayerMask.NameToLayer("PlayerLayer");
        Physics.IgnoreLayerCollision(playerLayer, playerLayer);
    }

   
}