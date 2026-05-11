using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UpgradeUI : MonoBehaviour
{
    public Button[] buttons;
    private UpgradeType[] currentOptions;
    

    public void ShowUpgrades(UpgradeType[] options)
    {
        currentOptions = options;
        gameObject.SetActive(true);

        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i;
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = options[i].ToString();


            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => SelectUpgrade(index));
        }
    }

    void SelectUpgrade(int index)
    {
        UpgradeType chosen = currentOptions[index];

        PlayerStats stats = UnityEngine.Object.FindFirstObjectByType<PlayerStats>();
        if (stats != null)
            stats.ApplyUpgrade(chosen);

        PlayerHealth health = UnityEngine.Object.FindFirstObjectByType<PlayerHealth>();
        if (health != null && chosen == UpgradeType.MaxHPUp)
            health.RefreshMaxHP(stats.maxHP);

        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
