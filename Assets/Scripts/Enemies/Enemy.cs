using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 2.5f;
    public int maxHP = 3;
    public int contactDamage = 1;

    private int currentHP;
    private Transform player;
    private Rigidbody2D rb;

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
            player = playerObj.transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 toPlayer = (Vector2)player.position - rb.position;
        float distance = toPlayer.magnitude;

        if (distance > stopDistance)
        {
            Vector2 dir = toPlayer.normalized;

            // MOVE
            rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);

            // ROTATE TOWARDS MOVEMENT
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rb.MoveRotation(angle);
        }
    }



    // ---------------- DAMAGE ----------------
    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;

        if (currentHP <= 0)
        {
            EnemySpawner spawner = Object.FindFirstObjectByType<EnemySpawner>();
            if (spawner != null)
                spawner.OnEnemyKilled(gameObject);

            Destroy(gameObject);
        }
    }

    // ---------------- PLAYER OVERLAP (NO PUSH) ----------------
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(contactDamage);
        }
    }
}
