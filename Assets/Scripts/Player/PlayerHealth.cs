using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 5;
    public float invulnerabilityTime = 1f;

    private int currentHP;
    private bool invulnerable;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        if (invulnerable)
        {
            return;
        }

        currentHP -= dmg;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log("Player HP: " + currentHP);

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Invulnerability());
        }
    }

    System.Collections.IEnumerator Invulnerability()
    {
        invulnerable = true;

        yield return new WaitForSeconds(invulnerabilityTime);

        invulnerable = false;
    }

    void Die()
    {
        Debug.Log("PLAYER DIED");

        // TEMP: restart scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RefreshMaxHP(int newMaxHP)
    {
        maxHP = newMaxHP;

        // Heal by 1 when max HP increases.
        currentHP = Mathf.Min(currentHP + 1, maxHP);

        Debug.Log("Max HP increased to: " + maxHP);
    }

    public int GetCurrentHealth()
    {
        return currentHP;
    }

    public int GetMaxHealth()
    {
        return maxHP;
    }
}