using System.Collections;
using UnityEngine;

public class ShootingMechanism : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnPoint;
    public float fireSpeed = 10f;
    public float interval = 2f;
    private Transform player;

    public BulletTrajectoryPreview trajectoryPreview;
    public float previewTime = 1f;

    [Header("Rotation Settings")]
    public float rotationSpeed = 5f;

    private void Update()
    {
        //player = GameObject.FindWithTag("Player").transform;
        player = Camera.main.transform;
        SmoothLookAtPlayer();
    }

    private void SmoothLookAtPlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    private void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            trajectoryPreview.ShowPreview();
            yield return new WaitForSeconds(previewTime);

            trajectoryPreview.HidePreview();

            FireBullet();
            yield return new WaitForSeconds(interval);
        }
    }

    private void FireBullet()
    {
        GameObject spawnedBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
        spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * fireSpeed;
        Destroy(spawnedBullet, 5f);
    }
}



//using System.Collections;
//using UnityEngine;

//public class ShootingMechanism : MonoBehaviour
//{
//    public GameObject bullet;
//    public Transform spawnPoint;
//    public float fireSpeed = 10f;
//    public float interval = 2f;
//    public Transform player;

//    public BulletTrajectoryPreview trajectoryPreview;
//    public float previewTime = 1f;

//    private void Start()
//    {
//        StartCoroutine(ShootRoutine());
//    }

//    private IEnumerator ShootRoutine()
//    {
//        while (true)
//        {
//            // Show trajectory preview
//            trajectoryPreview.ShowPreview();
//            yield return new WaitForSeconds(previewTime);

//            trajectoryPreview.HidePreview();

//            FireBullet();

//            yield return new WaitForSeconds(interval);
//        }
//    }

//    private void FireBullet()
//    {
//        if (player != null)
//            transform.LookAt(player); // adjust if model is flipped

//        GameObject spawnedBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
//        spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * fireSpeed;
//        Destroy(spawnedBullet, 5f);
//    }
//}
