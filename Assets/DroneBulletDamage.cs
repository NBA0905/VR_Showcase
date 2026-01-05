using UnityEngine;

public class DroneBulletDamage : MonoBehaviour
{
    public float damage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Bullet collided with: " + collision.collider.name);

        PlayerHealth player = collision.collider.GetComponentInParent<PlayerHealth>();
        if (player != null)
        {
            Debug.Log("PLAYER HIT — DEALING DAMAGE");
            player.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
