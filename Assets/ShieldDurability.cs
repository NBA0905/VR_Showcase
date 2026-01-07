using UnityEngine;

public class ShieldDurability : MonoBehaviour
{
    [Header("Shield Settings")]
    public int maxHits = 5;

    private int currentHits;

    void Awake()
    {
        currentHits = maxHits;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Only count enemy hits
        if (!collision.gameObject.CompareTag("Enemy"))
            return;

        TakeHit();
    }

    void TakeHit()
    {
        currentHits--;

        // Optional: feedback hook
        Debug.Log("Shield hit! Remaining: " + currentHits);

        if (currentHits <= 0)
        {
            BreakShield();
        }
    }

    void BreakShield()
    {
        // Optional: VFX / sound here

        Destroy(gameObject);
    }
}
