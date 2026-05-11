using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12f;
    public float lifeTime = 2f;
    private int damage;

    void Start()
    {
        PlayerStats stats = Object.FindFirstObjectByType<PlayerStats>();
        damage = stats != null ? stats.bulletDamage : 1;
        Destroy(gameObject, lifeTime);
    }


    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
