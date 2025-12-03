using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}


//using Unity.XR.CoreUtils;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.XR.Interaction.Toolkit;

//public class SceneLoaderFade : MonoBehaviour
//{
//    public string sceneToLoad;
//    public XROrigin xrOrigin;
//    public float fadeDuration = 1f;

//    private ScreenFader fader;

//    private void Start()
//    {
//        fader = FindObjectOfType<ScreenFader>();
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))
//            StartCoroutine(LoadSceneWithFade());
//    }

//    private IEnumerator LoadSceneWithFade()
//    {
//        yield return fader.FadeOut(fadeDuration);
//        SceneManager.LoadScene(sceneToLoad);
//    }
//}


