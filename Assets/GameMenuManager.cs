//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.SceneManagement;

//public class GameMenuManager : MonoBehaviour
//{
//    public Transform head;
//    public float distance = 2f;
//    public GameObject menu;
//    public InputActionProperty showButton;

//    [Header("Panels")]
//    public GameObject mainPanel;
//    public GameObject settingsPanel;

//    private bool isMenuOpen = false;

//    private Vector3 relativePosition;

//    private void Start()
//    {
//        head = Camera.main.transform;
//        settingsPanel.SetActive(false);
//        mainPanel.SetActive(true);
//        menu.SetActive(false);
//        Time.timeScale = 1f;
//    }

//    private void OnEnable()
//    {
//        if (showButton.action != null)
//        {
//            showButton.action.Enable();
//            Debug.Log("Menu button action enabled");
//        }
//        else
//        {
//            Debug.LogError("Show button action is NULL! Check Inspector settings.");
//        }
//    }

//    private void OnDisable()
//    {
//        if (showButton.action != null)
//        {
//            showButton.action.Disable();
//        }
//    }

//    //void Update()
//    //{
//    //    // Debug the action value
//    //    if (showButton.action != null)
//    //    {
//    //        float actionValue = showButton.action.ReadValue<float>();
//    //        if (actionValue > 0)
//    //        {
//    //            Debug.Log($"Button value: {actionValue}");
//    //        }
//    //    }

//    //    if (showButton.action != null && showButton.action.WasPressedThisFrame())
//    //    {
//    //        Debug.Log("Menu button PRESSED!");
//    //        ToggleMenu();
//    //    }

//    //    if (isMenuOpen)
//    //    {
//    //        menu.transform.position =
//    //            head.position +
//    //            new Vector3(head.forward.x, 0, head.forward.z).normalized * distance;
//    //        menu.transform.LookAt(new Vector3(
//    //            head.position.x,
//    //            menu.transform.position.y,
//    //            head.position.z
//    //        ));
//    //        menu.transform.forward *= -1;
//    //    }
//    //}

//    void Update()
//    {
//        if (showButton.action.WasPerformedThisFrame())
//        {
//            menu.SetActive(!menu.activeSelf);

//            if (menu.activeSelf)
//            {
//                menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * distance;
//                relativePosition = new Vector3(menu.transform.position.x - head.position.x, 0, menu.transform.position.z - head.position.z);
//            }
//        }

//        menu.transform.position = head.position + relativePosition;

//        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
//        menu.transform.forward *= -1;
//    }

//    void ToggleMenu()
//    {
//        isMenuOpen = !isMenuOpen;
//        Debug.Log($"Toggling menu. IsOpen: {isMenuOpen}");

//        menu.SetActive(isMenuOpen);

//        if (isMenuOpen)
//        {
//            mainPanel.SetActive(true);
//            settingsPanel.SetActive(false);
//        }

//        Time.timeScale = isMenuOpen ? 0f : 1f;
//    }

//    public void ResumeGame()
//    {
//        isMenuOpen = false;
//        menu.SetActive(false);
//        Time.timeScale = 1f;
//    }

//    public void RestartGame()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//    }

//    public void GoHome()
//    {
//        Time.timeScale = 1f;
//        SceneManager.LoadScene("Start");
//    }

//    public void QuitGame()
//    {
//        Time.timeScale = 1f;
//        Application.Quit();
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#endif
//    }

//    public void OpenSettings()
//    {
//        mainPanel.SetActive(false);
//        settingsPanel.SetActive(true);
//    }

//    public void CloseSettings()
//    {
//        settingsPanel.SetActive(false);
//        mainPanel.SetActive(true);
//    }
//}




using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GameMenuManager : MonoBehaviour
{
    public Transform head;
    public float distance = 2f;
    public GameObject menu;
    public InputActionProperty showButton;

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;

    [Header("UI Interaction Rays")]
    public XRRayInteractor leftRay;
    public XRRayInteractor rightRay;

    private Vector3 relativePosition;

    private void Start()
    {
        head = Camera.main.transform;
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
        menu.SetActive(false);

        // Disable rays at start
        if (leftRay != null) leftRay.enabled = false;
        if (rightRay != null) rightRay.enabled = false;
    }

    void Update()
    {
        if (showButton.action.WasPerformedThisFrame())
        {
            menu.SetActive(!menu.activeSelf);

            if (menu.activeSelf)
            {
                // Position menu in front of player
                menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * distance;
                relativePosition = new Vector3(menu.transform.position.x - head.position.x, 0, menu.transform.position.z - head.position.z);

                // Enable rays when menu opens
                if (leftRay != null) leftRay.enabled = true;
                if (rightRay != null) rightRay.enabled = true;

                Time.timeScale = 0f; // Pause game
            }
            else
            {
                // Disable rays when menu closes
                if (leftRay != null) leftRay.enabled = false;
                if (rightRay != null) rightRay.enabled = false;

                Time.timeScale = 1f; // Resume game
            }
        }

        // Update menu position and rotation when active
        if (menu.activeSelf)
        {
            menu.transform.position = head.position + relativePosition;
            menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
            menu.transform.forward *= -1;
        }
    }

    // Button Functions
    public void ResumeGame()
    {
        menu.SetActive(false);
        if (leftRay != null) leftRay.enabled = false;
        if (rightRay != null) rightRay.enabled = false;
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OpenSettings()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}