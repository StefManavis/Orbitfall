using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public float moveSpeed = 3f;
    public float fireRate = 0.4f;
    public int bulletDamage = 1;
    public int maxHP = 5;

    [Header("Shooting Modifiers")]
    public int bulletsPerVolley = 1;   // DoubleBullet
    public int volleysPerShot = 1;     // DoubleShot

    public void ApplyUpgrade(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.FireRateUp:
                fireRate *= 0.85f;
                break;

            case UpgradeType.DamageUp:
                bulletDamage += 1;
                break;

            case UpgradeType.MoveSpeedUp:
                moveSpeed += 1f;
                break;

            case UpgradeType.MaxHPUp:
                maxHP += 1;
                break;

            case UpgradeType.DoubleBullet:
                bulletsPerVolley += 1;
                break;

            case UpgradeType.DoubleShot:
                volleysPerShot += 1;
                break;
        }

        Debug.Log("Upgrade applied: " + type);
    }
}
