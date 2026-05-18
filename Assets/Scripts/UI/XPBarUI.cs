using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    [Header("References")]
    public PlayerLevel playerLevel;
    public Image fillImage;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;

    [Header("Animation")]
    public float fillSmoothSpeed = 8f;

    private float targetFillAmount = 0f;

    void Start()
    {
        if (playerLevel == null)
        {
            playerLevel = Object.FindFirstObjectByType<PlayerLevel>();
        }

        if (playerLevel != null)
        {
            playerLevel.OnXPChanged += UpdateXPBar;
            playerLevel.OnLevelUp += UpdateLevelText;

            UpdateXPBar(playerLevel.currentLevel, playerLevel.currentXP, playerLevel.XPToNextLevel);
        }
        else
        {
            Debug.LogWarning("XPBarUI could not find PlayerLevel.");
        }
    }

    void Update()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = Mathf.Lerp(
                fillImage.fillAmount,
                targetFillAmount,
                Time.unscaledDeltaTime * fillSmoothSpeed
            );
        }
    }

    private void UpdateXPBar(int level, int currentXP, int xpToNextLevel)
    {
        if (xpToNextLevel <= 0)
        {
            targetFillAmount = 0f;
        }
        else
        {
            targetFillAmount = (float)currentXP / xpToNextLevel;
        }

        if (levelText != null)
        {
            levelText.text = "LVL " + level;
        }

        if (xpText != null)
        {
            xpText.text = currentXP + " / " + xpToNextLevel + " XP";
        }
    }

    private void UpdateLevelText(int level)
    {
        if (levelText != null)
        {
            levelText.text = "LVL " + level;
        }
    }

    void OnDestroy()
    {
        if (playerLevel != null)
        {
            playerLevel.OnXPChanged -= UpdateXPBar;
            playerLevel.OnLevelUp -= UpdateLevelText;
        }
    }
}