using UnityEngine;
using System;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public UpgradeUI upgradeUI;

    public void OfferUpgrade()
    {
        Time.timeScale = 0f;

        UpgradeType[] options = GetRandomUpgrades(3);
        upgradeUI.ShowUpgrades(options);
    }


    void AutoPick()
    {
        UpgradeType upgrade = GetRandomUpgrades(1)[0];

        PlayerStats stats = UnityEngine.Object.FindFirstObjectByType<PlayerStats>();
        if (stats != null)
            stats.ApplyUpgrade(upgrade);

        PlayerHealth health = UnityEngine.Object.FindFirstObjectByType<PlayerHealth>();
        if (health != null && upgrade == UpgradeType.MaxHPUp)
            health.RefreshMaxHP(stats.maxHP);

        Time.timeScale = 1f;
    }

    UpgradeType[] GetRandomUpgrades(int count)
    {
        UpgradeType[] all = (UpgradeType[])Enum.GetValues(typeof(UpgradeType));
        System.Random rng = new System.Random();

        for (int i = 0; i < all.Length; i++)
        {
            int j = rng.Next(i, all.Length);
            (all[i], all[j]) = (all[j], all[i]);
        }

        UpgradeType[] result = new UpgradeType[count];
        for (int i = 0; i < count; i++)
            result[i] = all[i];

        return result;
    }
}
