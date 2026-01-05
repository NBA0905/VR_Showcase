using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    public Transform head;
    public float distance = 2f;
    public GameObject menu;
    public InputActionProperty showButton;

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject settingsPanel;

    private bool isMenuOpen = false;

    private void Start()
    {
        //player = GameObject.FindWithTag("Player").transform;
        head = Camera.main.transform;

        settingsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            ToggleMenu();
        }

        if (isMenuOpen)
        {
            menu.transform.position =
                head.position +
                new Vector3(head.forward.x, 0, head.forward.z).normalized * distance;

            menu.transform.LookAt(new Vector3(
                head.position.x,
                menu.transform.position.y,
                head.position.z
            ));
            menu.transform.forward *= -1;
        }
    }

    void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menu.SetActive(isMenuOpen);


        if (isMenuOpen)
        {
            mainPanel.SetActive(true);
            settingsPanel.SetActive(false);
        }

        Time.timeScale = isMenuOpen ? 0f : 1f;
    }

    // Buttons functions

    public void ResumeGame()
    {
        isMenuOpen = false;
        menu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Important before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start"); // exact scene name
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
