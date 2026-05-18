using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button[] buttons;

    [Header("Upgrade Icons / Card Images")]
    public Sprite fireRateIcon;
    public Sprite damageIcon;
    public Sprite moveSpeedIcon;
    public Sprite maxHPIcon;
    public Sprite doubleBulletIcon;
    public Sprite doubleShotIcon;

    private UpgradeType[] currentOptions;

    public void ShowUpgrades(UpgradeType[] options)
    {
        currentOptions = options;
        gameObject.SetActive(true);

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            UpgradeType upgrade = options[i];

            TextMeshProUGUI text = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = upgrade.ToString();
            }

            Image buttonImage = buttons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                Sprite cardImage = GetIconForUpgrade(upgrade);
                buttonImage.sprite = cardImage;
                buttonImage.preserveAspect = false;
            }

            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => SelectUpgrade(index));
        }
    }

    private Sprite GetIconForUpgrade(UpgradeType upgrade)
    {
        switch (upgrade)
        {
            case UpgradeType.FireRateUp:
                return fireRateIcon;

            case UpgradeType.DamageUp:
                return damageIcon;

            case UpgradeType.MoveSpeedUp:
                return moveSpeedIcon;

            case UpgradeType.MaxHPUp:
                return maxHPIcon;

            case UpgradeType.DoubleBullet:
                return doubleBulletIcon;

            case UpgradeType.DoubleShot:
                return doubleShotIcon;

            default:
                return null;
        }
    }

    private void SelectUpgrade(int index)
    {
        UpgradeType chosen = currentOptions[index];

        PlayerStats stats = UnityEngine.Object.FindFirstObjectByType<PlayerStats>();
        if (stats != null)
        {
            stats.ApplyUpgrade(chosen);
        }

        PlayerHealth health = UnityEngine.Object.FindFirstObjectByType<PlayerHealth>();
        if (health != null && stats != null && chosen == UpgradeType.MaxHPUp)
        {
            health.RefreshMaxHP(stats.maxHP);
        }

        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}