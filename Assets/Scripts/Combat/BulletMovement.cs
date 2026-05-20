using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletMovement : MonoBehaviour
{
    public float speed = 12f;
    public float lifetime = 3f;
    public int damage = 1;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    private void Start()
    {
        rb.linearVelocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        EnemyShooter enemyShooter = other.GetComponentInParent<EnemyShooter>();

        if (enemyShooter != null)
        {
            enemyShooter.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Enemy") || other.transform.root.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            return;
        }
    }
}