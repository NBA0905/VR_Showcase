using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject shooterEnemy;
    public GameObject hitterEnemy;
    public GameObject laserEnemy;

    [Header("Spawn Settings")]
    public List<Transform> spawnPoints; // Add any spawn points around the map
    public float waveDuration = 30f;

    [Header("Wave Settings")]
    public int maxWaves = 5;

    [Header("Item Prefabs")]
    public GameObject itemA;
    public GameObject itemB;
    public GameObject itemC;

    [Header("Item Spawn")]
    public Transform itemSpawnPoint; // where items appear

    private int currentWave = 0;
    private bool waveActive = false;
    private bool gameStarted = false; // NEW: prevents auto-start
    private GameObject activeItem;

    public void StartWaves()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            StartCoroutine(StartNextWave());
        }
    }

    private IEnumerator StartNextWave()
    {
        // Stop if we've reached the max waves
        if (currentWave >= maxWaves)
        {
            Debug.Log("All waves completed!");
            yield break; // stops the coroutine permanently
        }


        yield return new WaitForSeconds(2f); // small delay if needed

        currentWave++;
        waveActive = true;

        Debug.Log("Starting Wave " + currentWave);
        SpawnWave(currentWave);
        SpawnRandomItem();

        yield return new WaitForSeconds(waveDuration);

        waveActive = false;

        ClearWaveItem();
        StartCoroutine(StartNextWave());
    }

    private void SpawnWave(int waveIndex)
    {
        // Wave 1: always 2 shooters
        if (waveIndex == 1)
        {
            SpawnEnemy(shooterEnemy);
            SpawnEnemy(shooterEnemy);
            return;
        }

        // Wave 2: previous + 1 more shooter + 1 hitter
        if (waveIndex == 2)
        {
            //SpawnEnemy(shooterEnemy);
            //SpawnEnemy(shooterEnemy);
            SpawnEnemy(shooterEnemy); // +1 shooter
            SpawnEnemy(hitterEnemy);
            return;
        }

        // Wave 3: previous + 1 laser
        if (waveIndex == 3)
        {
            //SpawnEnemy(shooterEnemy);
            //SpawnEnemy(shooterEnemy);
            //SpawnEnemy(shooterEnemy);
            SpawnEnemy(hitterEnemy);
            SpawnEnemy(laserEnemy);
            return;
        }

        // Wave 4+: add 1 random enemy per wave
        SpawnWave(3); // spawn base wave 3 enemies

        // Add random extras based on wave number
        int extraEnemies = waveIndex - 3;

        for (int i = 0; i < extraEnemies; i++)
        {
            SpawnRandomEnemy();
        }
    }

    private void SpawnEnemy(GameObject prefab)
    {
        const int maxAttempts = 10;
        float checkRadius = 1.5f; // adjust based on enemy size

        for (int i = 0; i < maxAttempts; i++)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Count)];

            // Check if area is free
            Collider[] hits = Physics.OverlapSphere(point.position, checkRadius);

            bool occupied = false;
            foreach (Collider c in hits)
            {
                if (c.CompareTag("Enemy")) 
                {
                    occupied = true;
                    break;
                }
            }

            if (!occupied)
            {
                Instantiate(prefab, point.position, point.rotation);
                return;
            }
        }

        // If all attempts fail, force spawn anywhere
        Debug.Log("All spawn points occupied, forcing spawn.");
        Transform fallback = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(prefab, fallback.position, fallback.rotation);
    }

    //private void ClearPreviousEnemies()
    //{
    //    foreach (GameObject enemy in activeEnemies)
    //    {
    //        if (enemy != null)
    //            Destroy(enemy);
    //    }
    //    activeEnemies.Clear();
    //}

    private void SpawnRandomEnemy()
    {
        int r = Random.Range(0, 3);

        if (r == 0) SpawnEnemy(shooterEnemy);
        else if (r == 1) SpawnEnemy(hitterEnemy);
        else SpawnEnemy(laserEnemy);
    }

    private void SpawnRandomItem()
    {
        // Safety: destroy previous item if something went wrong
        if (activeItem != null)
            Destroy(activeItem);

        int r = Random.Range(0, 3);

        GameObject prefab =
            r == 0 ? itemA :
            r == 1 ? itemB :
                     itemC;

        activeItem = Instantiate(prefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
    }

    private void ClearWaveItem()
    {
        if (activeItem != null)
        {
            Destroy(activeItem);
            activeItem = null;
        }
    }

}
