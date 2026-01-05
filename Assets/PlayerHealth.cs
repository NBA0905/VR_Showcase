using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("PLAYER TOOK DAMAGE: " + amount + " | Remaining Health: " + health);

        if (health <= 0)
        {
            Debug.Log("PLAYER DIED!");
        }
    }
}

