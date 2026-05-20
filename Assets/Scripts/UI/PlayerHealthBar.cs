using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [Header("References")]
    public PlayerHealth playerHealth;
    public GameObject healthBarRoot;
    public Image greenFillImage;

    [Header("Settings")]
    public bool hideWhenFullHealth = true;

    void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }

        UpdateHealthBar();
    }

    void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (playerHealth == null || healthBarRoot == null || greenFillImage == null)
        {
            return;
        }

        int currentHealth = playerHealth.GetCurrentHealth();
        int maxHealth = playerHealth.GetMaxHealth();

        if (maxHealth <= 0)
        {
            return;
        }

        float healthPercent = (float)currentHealth / maxHealth;
        healthPercent = Mathf.Clamp01(healthPercent);

        greenFillImage.fillAmount = healthPercent;

        if (hideWhenFullHealth)
        {
            bool shouldShowBar = currentHealth < maxHealth;
            healthBarRoot.SetActive(shouldShowBar);
        }
        else
        {
            healthBarRoot.SetActive(true);
        }
    }
}