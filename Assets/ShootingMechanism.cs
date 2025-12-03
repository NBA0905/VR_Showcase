using System.Collections;
using UnityEngine;

public class ShootingMechanism : MonoBehaviour
{
    public GameObject bullet;
    public Transform spawnPoint;
    public float fireSpeed = 10f;
    public float interval = 2f;
    public Transform player;

    public BulletTrajectoryPreview trajectoryPreview;
    public float previewTime = 1f;

    private void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            // Show trajectory preview
            trajectoryPreview.ShowPreview();
            yield return new WaitForSeconds(previewTime);

            trajectoryPreview.HidePreview();

            FireBullet();

            yield return new WaitForSeconds(interval);
        }
    }

    private void FireBullet()
    {
        if (player != null)
            transform.LookAt(player); // adjust if model is flipped

        GameObject spawnedBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
        spawnedBullet.GetComponent<Rigidbody>().velocity = spawnPoint.forward * fireSpeed;
        Destroy(spawnedBullet, 5f);
    }
}
