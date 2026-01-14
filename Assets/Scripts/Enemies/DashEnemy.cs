//using UnityEngine;
//using System.Collections;

//public class DashEnemy : MonoBehaviour
//{
//    [Header("References")]
//    public Transform player;
//    public Transform dashStartPoint;                // where dash originates (e.g., the enemy root)
//    public BulletTrajectoryPreview trajectory;      // reuse the same trajectory system

//    [Header("Dash Settings")]
//    public float dashSpeed = 15f;
//    public float dashDistance = 8f;
//    public float previewTime = 1.5f;
//    public float cooldownTime = 2f;

//    private bool isDashing = false;

//    void Start()
//    {
//        StartCoroutine(DashLoop());
//    }

//    IEnumerator DashLoop()
//    {
//        yield return new WaitForSeconds(1f); // small initial delay

//        while (true)
//        {
//            // 1. Show trajectory preview
//            trajectory.ShowPreview();

//            yield return new WaitForSeconds(previewTime);

//            // 2. Hide preview
//            trajectory.HidePreview();

//            // 3. Dash
//            yield return StartCoroutine(PerformDash());

//            // 4. Cooldown
//            yield return new WaitForSeconds(cooldownTime);
//        }
//    }

//    IEnumerator PerformDash()
//    {
//        if (player == null) yield break;

//        if (player != null)
//            transform.LookAt(player);

//        isDashing = true;

//        Vector3 start = dashStartPoint.position;
//        Vector3 direction = (player.position - start).normalized;

//        float traveled = 0f;

//        while (traveled < dashDistance)
//        {
//            float step = dashSpeed * Time.deltaTime;

//            transform.position += direction * step;

//            traveled += step;

//            yield return null;
//        }

//        isDashing = false;
//    }
//}


using UnityEngine;
using System.Collections;

public class DashEnemy : MonoBehaviour
{
    [Header("References")]
    private Transform player;
    public Transform dashStartPoint;
    public BulletTrajectoryPreview trajectory;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;
    public float dashDistance = 8f;
    public float previewTime = 1.5f;
    public float cooldownTime = 2f;

    [Header("Collision Settings")]
    public LayerMask obstacleLayer; // Assign walls/pillars layer
    public float collisionCheckDistance = 0.5f;

    private Rigidbody rb;
    private bool isDashing = false;

    void Start()
    {
        //player = GameObject.FindWithTag("Player").transform;
        player = Camera.main.transform;
        rb = GetComponent<Rigidbody>();

        // CRITICAL: Set collision detection to Continuous
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.isKinematic = false; // Changed to false for proper collision
        rb.useGravity = false;
        rb.freezeRotation = true;

        StartCoroutine(DashLoop());
    }

    IEnumerator DashLoop()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            trajectory.ShowPreview();
            yield return new WaitForSeconds(previewTime);
            trajectory.HidePreview();
            yield return StartCoroutine(PerformDash());
            yield return new WaitForSeconds(cooldownTime);
        }
    }

    IEnumerator PerformDash()
    {
        if (player == null) yield break;

        transform.LookAt(player);
        isDashing = true;

        Vector3 start = dashStartPoint.position;
        Vector3 direction = (player.position - start).normalized;
        float traveled = 0f;

        while (traveled < dashDistance && isDashing)
        {
            float step = dashSpeed * Time.fixedDeltaTime;

            // Check for obstacles before moving
            if (Physics.Raycast(rb.position, direction, out RaycastHit hit, step + collisionCheckDistance, obstacleLayer))
            {
                Debug.Log($"Hit obstacle: {hit.collider.name}");
                // Stop dashing if we hit a wall
                isDashing = false;
                //Destroy(gameObject, 5);
                break;
            }

            // Use velocity for more reliable collision detection
            rb.velocity = direction * dashSpeed;

            traveled += step;
            yield return new WaitForFixedUpdate();
        }

        // Stop movement
        rb.velocity = Vector3.zero;
        isDashing = false;
    }

    // Stop dashing if we collide with something
    void OnCollisionEnter(Collision collision)
    {
        if (isDashing)
        {
            Debug.Log($"Collided with: {collision.gameObject.name}");
            isDashing = false;
            rb.velocity = Vector3.zero;
        }
    }
}
