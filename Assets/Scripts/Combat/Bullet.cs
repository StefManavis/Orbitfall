using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Damage")]
    public int damage = 1;

    [Header("Lifetime")]
    public float lifetime = 3f;

    [Header("Hit Settings")]
    public LayerMask hitLayers;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!CanHitLayer(other.gameObject.layer))
            return;

        IDamageable damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    private bool CanHitLayer(int objectLayer)
    {
        return (hitLayers.value & (1 << objectLayer)) != 0;
    }
}