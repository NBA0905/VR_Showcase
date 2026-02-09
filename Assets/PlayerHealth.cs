//using UnityEngine;
//using UnityEngine.UI;

//public class PlayerHealth : MonoBehaviour
//{
//    public float maxHealth = 100f;
//    public float currentHealth;

//    public Image healthFill;

//    void Start()
//    {
//        currentHealth = maxHealth;
//        UpdateHealthUI();
//    }

//    public void TakeDamage(float damage)
//    {
//        Debug.Log("Player take damage " + damage);
//        currentHealth -= damage;
//        Debug.Log(currentHealth);
//        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
//        UpdateHealthUI();
//    }

//    void UpdateHealthUI()
//    {
//        healthFill.fillAmount = Mathf.Lerp(
//        healthFill.fillAmount,
//        targetFill,
//        Time.deltaTime * 8f
//        );

//    }
//}


using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    public Image healthFill;

    float targetFill; 

    void Start()
    {
        currentHealth = maxHealth;
        targetFill = 1f;
        healthFill.fillAmount = 1f;
    }

    void Update()
    {
        // Smooth animation every frame
        healthFill.fillAmount = Mathf.Lerp(
            healthFill.fillAmount,
            targetFill,
            Time.deltaTime * 8f
        );
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Player take damage " + damage);

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Update target fill (0–1)
        targetFill = currentHealth / maxHealth;
    }
}
