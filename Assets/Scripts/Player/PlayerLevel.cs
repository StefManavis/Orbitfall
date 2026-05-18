using System;
using UnityEngine;

public class PlayerLevel : MonoBehaviour
{
    [Header("Level")]
    public int currentLevel = 1;
    public int currentXP = 0;

    [Header("XP Scaling")]
    public int baseXPToLevel = 5;
    public float xpGrowthRate = 1.25f;

    public event Action<int, int, int> OnXPChanged;
    public event Action<int> OnLevelUp;

    public int XPToNextLevel
{
    get
    {
        float xpPerRoom = 5f;
        float roomsRequired;

        if (currentLevel == 1)
        {
            roomsRequired = 1f;
        }
        else if (currentLevel == 2)
        {
            roomsRequired = 2f;
        }
        else
        {
            roomsRequired = 2.2f + ((currentLevel - 3) * 0.3f);
        }

        return Mathf.RoundToInt(xpPerRoom * roomsRequired);
    }
}

    void Awake()
    {
        Debug.Log(
            "PlayerLevel Awake. Object: " + gameObject.name +
            " | Level: " + currentLevel +
            " | XP Needed: " + XPToNextLevel
        );
    }

    void Start()
    {
        NotifyXPChanged();
    }

    public void AddXP(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        Debug.Log(
            "AddXP called. Gained: " + amount +
            " | Before: " + currentXP + " / " + XPToNextLevel +
            " | Level: " + currentLevel
        );

        currentXP += amount;

        while (currentXP >= XPToNextLevel)
        {
            int xpNeededForThisLevel = XPToNextLevel;

            currentXP -= xpNeededForThisLevel;
            LevelUp();
        }

        NotifyXPChanged();

        Debug.Log(
            "After XP. Current: " + currentXP + " / " + XPToNextLevel +
            " | Level: " + currentLevel
        );
    }

    private void LevelUp()
    {
        currentLevel++;

        Debug.Log(
            "LEVEL UP! New Level: " + currentLevel +
            " | New XP Needed: " + XPToNextLevel
        );

        OnLevelUp?.Invoke(currentLevel);

        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.OfferUpgrade();
        }
        else
        {
            Debug.LogWarning("UpgradeManager instance was not found. Cannot offer upgrade.");
        }
    }

    private void NotifyXPChanged()
    {
        OnXPChanged?.Invoke(currentLevel, currentXP, XPToNextLevel);
    }
}