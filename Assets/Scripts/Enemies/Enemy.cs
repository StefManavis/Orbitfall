using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public float moveSpeed = 1.8f;
    public int maxHP = 3;
    public int contactDamage = 1;

    private int currentHP;
    private Transform player;
    private Rigidbody2D rb;
    private bool isDead = false;

    public float stopDistance = 0.6f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        currentHP = maxHP;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }

        if (player == null)
        {
            return;
        }

        Vector2 toPlayer = (Vector2)player.position - rb.position;
        float distance = toPlayer.magnitude;

        if (distance > stopDistance)
        {
            Vector2 dir = toPlayer.normalized;

            rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rb.MoveRotation(angle);
        }
    }

    public void TakeDamage(int dmg)
    {
        if (isDead)
        {
            return;
        }

        currentHP -= dmg;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        GiveXP giveXP = GetComponent<GiveXP>();

        if (giveXP != null)
        {
            giveXP.DropXP();
        }

        EnemySpawner spawner = Object.FindFirstObjectByType<EnemySpawner>();

        if (spawner != null)
        {
            spawner.OnEnemyKilled(gameObject);
        }

        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead)
        {
            return;
        }

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(contactDamage);
        }
    }
}