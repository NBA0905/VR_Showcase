//using UnityEngine;
//using System.Collections;
//using TMPro;

//public class GameStartTrigger : MonoBehaviour
//{
//    [Header("Player Setup")]
//    public string playerTag = "Player";

//    [Header("Countdown Settings")]
//    public float countdownTime = 3f;
//    public GameObject floatingCountdownPrefab; // NEW
//    private GameObject countdownInstance;
//    private TextMeshPro countdownText;


//    private bool hasTriggered = false;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (hasTriggered) return;

//        if (other.CompareTag(playerTag))
//        {
//            hasTriggered = true;
//            StartCoroutine(StartGameSequence(other.transform));
//        }
//    }

//    private IEnumerator StartGameSequence(Transform player)
//    {
//        // Spawn floating hologram text
//        countdownInstance = Instantiate(floatingCountdownPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
//        countdownInstance.GetComponent<Billboard>().target = player;

//        countdownText = countdownInstance.GetComponent<TextMeshPro>();

//        float t = countdownTime;

//        while (t > 0)
//        {
//            countdownText.text = Mathf.Ceil(t).ToString();

//            // Optional pulse effect
//            countdownInstance.transform.localScale = Vector3.one * (1 + 0.1f * Mathf.Sin(Time.time * 6f));

//            yield return new WaitForSeconds(1f);
//            t -= 1f;
//        }

//        Destroy(countdownInstance);

//        gameObject.SetActive(false);
//    }


//}



using UnityEngine;
using System.Collections;
using TMPro;

public class GameStartTrigger : MonoBehaviour
{
    [Header("Player Setup")]
    public string playerTag = "Player";

    [Header("Countdown Settings")]
    public float countdownTime = 3f;
    public GameObject floatingCountdownPrefab;

    [Header("Positioning")]
    public Vector3 countdownOffset = new Vector3(0, 2f, 3f); // Up 2, forward 3

    [Header("Wave Manager")]
    public WaveManager waveManager; // Link to WaveManager

    private GameObject countdownInstance;
    private TextMeshPro countdownText;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag(playerTag))
        {
            hasTriggered = true;
            StartCoroutine(StartGameSequence(other.transform));
        }
    }

    private IEnumerator StartGameSequence(Transform player)
    {
        // Calculate spawn position relative to player's forward direction
        Vector3 spawnPos = player.position + player.forward * countdownOffset.z + Vector3.up * countdownOffset.y;

        // Spawn floating hologram text
        countdownInstance = Instantiate(floatingCountdownPrefab, spawnPos, Quaternion.identity);

        // Set up billboard if it exists
        Billboard billboard = countdownInstance.GetComponent<Billboard>();
        if (billboard != null)
        {
            billboard.target = player;
        }

        countdownText = countdownInstance.GetComponent<TextMeshPro>();

        // Fixed countdown loop
        for (int i = (int)countdownTime; i > 0; i--)
        {
            countdownText.text = i.ToString();

            //Pulse effect
            float elapsed = 0f;
            while (elapsed < 1f)
            {
                countdownInstance.transform.localScale = Vector3.one * (1 + 0.1f * Mathf.Sin(Time.time * 6f));
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        //"GO!" message
        countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);

        Destroy(countdownInstance);


        //Start the wave manager
        if (waveManager != null)
        {
            waveManager.StartWaves();
        }
        else
        {
            Debug.LogWarning("WaveManager reference not set in GameStartTrigger!");
        }

        gameObject.SetActive(false);
    }
}