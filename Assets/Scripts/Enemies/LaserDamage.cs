using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    public LineRenderer lr;
    public float damage = 5f;
    public float rayDistance = 20f;

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(lr.GetPosition(0),
                            lr.GetPosition(1) - lr.GetPosition(0),
                            out hit, rayDistance))
        {
            PlayerHealth player = hit.collider.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage * Time.deltaTime);
            }
        }
    }
}
