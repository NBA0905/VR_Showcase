using UnityEngine;

public class BulletTrajectoryPreview : MonoBehaviour
{
    [Header("References")]
    public LineRenderer lineRenderer;
    public Transform spawnPoint;
    private Transform player; // XR Origin or camera offset

    [Header("Settings")]
    public int steps = 30;
    public LayerMask collisionLayers; // What should block the trajectory

    //private float bulletSpeed = 10f;
    //private float simulationTime = 2f;
    private bool showPreview = false;

    private void Start()
    {
        //player = GameObject.FindWithTag("Player").transform;
        player = Camera.main.transform;
    }
    public void ShowPreview()
    {
        showPreview = true;
        lineRenderer.enabled = true;
        DrawTrajectory();
    }

    public void HidePreview()
    {
        showPreview = false;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (showPreview)
            DrawTrajectory();
    }

    private void DrawTrajectory()
    {
        if (!lineRenderer || !spawnPoint || !player) return;

        Vector3 startPos = spawnPoint.position;
        Vector3 targetPos = player.position;
        Vector3 direction = (targetPos - startPos).normalized;
        float totalDistance = Vector3.Distance(startPos, targetPos);

        // Perform raycast to check for obstacles
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(startPos, direction, out hit, totalDistance, collisionLayers);

        // Determine end position
        Vector3 endPos;
        float drawDistance;

        if (hitSomething)
        {
            // Line stops at the obstacle
            endPos = hit.point;
            drawDistance = hit.distance;
        }
        else
        {
            // Line goes all the way to player
            endPos = targetPos;
            drawDistance = totalDistance;
        }

        // Draw the line
        lineRenderer.positionCount = steps;
        for (int i = 0; i < steps; i++)
        {
            float t = i / (float)(steps - 1);
            lineRenderer.SetPosition(i, Vector3.Lerp(startPos, endPos, t));
        }
    }
}




//using UnityEngine;

//public class BulletTrajectoryPreview : MonoBehaviour
//{
//    [Header("References")]
//    public LineRenderer lineRenderer;       // Assign your LineRenderer
//    public Transform spawnPoint;            // Bullet spawn point
//    public Transform player;                // XR Camera Offset
//    public GameObject reticlePrefab;        // Small circle or sphere

//    [Header("Settings")]
//    public int steps = 30;                  // Resolution of the line
//    public float maxDistance = 20f;         // Max length of line

//    private GameObject reticleInstance;
//    private bool showPreview = false;

//    void Start()
//    {
//        // Instantiate reticle
//        if (reticlePrefab)
//        {
//            reticleInstance = Instantiate(reticlePrefab);
//            reticleInstance.SetActive(false);
//        }

//        if (lineRenderer)
//            lineRenderer.enabled = false;
//    }

//    void Update()
//    {
//        if (showPreview)
//            DrawTrajectory();
//    }

//    /// <summary>
//    /// Enable and show the trajectory
//    /// </summary>
//    public void ShowPreview()
//    {
//        showPreview = true;
//        if (lineRenderer) lineRenderer.enabled = true;
//        if (reticleInstance) reticleInstance.SetActive(true);
//    }

//    /// <summary>
//    /// Hide trajectory and reticle
//    /// </summary>
//    public void HidePreview()
//    {
//        showPreview = false;
//        if (lineRenderer) lineRenderer.enabled = false;
//        if (reticleInstance) reticleInstance.SetActive(false);
//    }

//    /// <summary>
//    /// Draw trajectory and place reticle
//    /// </summary>
//    private void DrawTrajectory()
//    {
//        if (!lineRenderer || !spawnPoint || !player) return;

//        Vector3 startPos = spawnPoint.position;
//        Vector3 direction = (player.position - startPos).normalized;

//        // Use raycast to detect obstacles
//        float distance = maxDistance;
//        if (Physics.Raycast(startPos, direction, out RaycastHit hit, maxDistance))
//        {
//            distance = hit.distance;
//        }

//        // Draw line
//        lineRenderer.positionCount = steps;
//        for (int i = 0; i < steps; i++)
//        {
//            float t = i / (float)(steps - 1);
//            lineRenderer.SetPosition(i, startPos + direction * distance * t);
//        }

//        // Place reticle at end of line
//        if (reticleInstance)
//        {
//            reticleInstance.transform.position = startPos + direction * distance;
//        }
//    }
//}
