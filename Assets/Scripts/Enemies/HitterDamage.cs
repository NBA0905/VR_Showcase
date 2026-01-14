using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitterDamage : MonoBehaviour
{
    public float damage = 15f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}
