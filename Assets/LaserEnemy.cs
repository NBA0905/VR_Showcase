//using UnityEngine;
//using System.Collections;

//public class LaserEnemy : MonoBehaviour
//{
//    [Header("References")]
//    public LineRenderer laserRenderer;
//    public Transform firePoint;        // forward direction of the drone

//    [Header("Laser Settings")]
//    public float laserLength = 20f;
//    public float laserDuration = 2f;
//    public float cooldownDuration = 2f;

//    [Header("Rotation Settings")]
//    public float minAngle = -60f;
//    public float maxAngle = 60f;
//    public float rotateSpeed = 2f;

//    private void Start()
//    {
//        laserRenderer.enabled = false;
//        StartCoroutine(LaserRoutine());
//    }

//    IEnumerator LaserRoutine()
//    {
//        yield return new WaitForSeconds(1f);

//        while (true)
//        {
//            // Pick a random angle to block movement
//            float angle = Random.Range(minAngle, maxAngle);

//            // Smooth rotation
//            yield return StartCoroutine(RotateToAngle(angle));

//            // Activate laser
//            laserRenderer.enabled = true;

//            float timer = 0f;
//            while (timer < laserDuration)
//            {
//                UpdateLaser();
//                timer += Time.deltaTime;
//                yield return null;
//            }

//            // Deactivate laser
//            laserRenderer.enabled = false;

//            yield return new WaitForSeconds(cooldownDuration);
//        }
//    }

//    IEnumerator RotateToAngle(float angle)
//    {
//        Quaternion target = Quaternion.Euler(0, angle + 180f, 0);

//        while (Quaternion.Angle(transform.rotation, target) > 0.5f)
//        {
//            transform.rotation =
//                Quaternion.Slerp(transform.rotation, target, rotateSpeed * Time.deltaTime);
//            yield return null;
//        }
//    }

//    void UpdateLaser()
//    {
//        Vector3 startPos = firePoint.position;
//        Vector3 dir = firePoint.forward;

//        // Raycast to block the beam when hitting walls
//        if (Physics.Raycast(startPos, dir, out RaycastHit hit, laserLength))
//        {
//            // beam stops at collision
//            laserRenderer.SetPosition(0, startPos);
//            laserRenderer.SetPosition(1, hit.point);
//        }
//        else
//        {
//            // beam goes full length
//            laserRenderer.SetPosition(0, startPos);
//            laserRenderer.SetPosition(1, startPos + dir * laserLength);
//        }
//    }
//}


using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class LaserEnemy : MonoBehaviour
{
    [Header("References")]
    public LineRenderer laserRenderer;
    public Transform firePoint;        // forward direction of the drone
    private Transform player;           // Reference to XR Origin or player transform

    [Header("Laser Settings")]
    public float laserLength = 20f;
    public float laserDuration = 2f;
    public float cooldownDuration = 2f;

    [Header("Rotation Settings")]
    public float minAngle = -60f;
    public float maxAngle = 60f;
    public float rotateSpeed = 2f;

    [Header("Vertical Tilt Settings")]
    public float maxDownwardTilt = 30f;  // Max degrees to tilt down
    public float tiltThreshold = 2f;     // Height difference before tilting starts
    public float tiltMultiplier = 15f;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        laserRenderer.enabled = false;

        // Auto-find player if not assigned
        if (player == null)
        {
            GameObject xrOrigin = GameObject.Find("XR Origin");
            if (xrOrigin != null)
                player = xrOrigin.transform;
        }

        StartCoroutine(LaserRoutine());
    }

    IEnumerator LaserRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            // Pick a random angle to block movement
            float angle = Random.Range(minAngle, maxAngle);

            // Smooth rotation
            yield return StartCoroutine(RotateToAngle(angle));

            // Activate laser
            laserRenderer.enabled = true;
            float timer = 0f;
            while (timer < laserDuration)
            {
                UpdateLaser();
                timer += Time.deltaTime;
                yield return null;
            }

            // Deactivate laser
            laserRenderer.enabled = false;
            yield return new WaitForSeconds(cooldownDuration);
        }
    }

    IEnumerator RotateToAngle(float angle)
    {
        Quaternion target = CalculateTargetRotation(angle);

        while (Quaternion.Angle(transform.rotation, target) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotateSpeed * Time.deltaTime);
            yield return null;
        }
    }

    Quaternion CalculateTargetRotation(float yawAngle)
    {
        // Calculate vertical tilt based on height difference
        float pitchAngle = 0f;

        if (player != null)
        {
            float heightDiff = transform.position.y - player.position.y;
            Debug.Log($"Height Diff: {heightDiff:F2} | Drone Y: {transform.position.y:F2} | Player Y: {player.position.y:F2} | Pitch: {pitchAngle:F2}");


            // Only tilt down if drone is above player by threshold
            if (heightDiff > tiltThreshold)
            {
                // Calculate tilt based on height difference
                // More height difference = more tilt (up to maxDownwardTilt)
                pitchAngle = Mathf.Min((heightDiff - tiltThreshold) * tiltMultiplier, maxDownwardTilt);
                Debug.Log($"Tilting down by {pitchAngle:F2} degrees");
            }
        }

        // Combine yaw (horizontal) and pitch (vertical) rotations
        return Quaternion.Euler(pitchAngle, yawAngle + 180f, 0);
    }

    void UpdateLaser()
    {
        Vector3 startPos = firePoint.position;
        Vector3 dir = firePoint.forward;

        // Raycast to block the beam when hitting walls
        if (Physics.Raycast(startPos, dir, out RaycastHit hit, laserLength))
        {
            // beam stops at collision
            laserRenderer.SetPosition(0, startPos);
            laserRenderer.SetPosition(1, hit.point);
        }
        else
        {
            // beam goes full length
            laserRenderer.SetPosition(0, startPos);
            laserRenderer.SetPosition(1, startPos + dir * laserLength);
        }
    }
}