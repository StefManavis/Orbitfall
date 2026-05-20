using System.Collections;
using UnityEngine;

public class EnemyShooter : MonoBehaviour, IDamageable
{
    [Header("References")]
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Aiming")]
    public Transform aimTransform;
    public bool invertAimRotation = false;

    [Header("Movement")]
    public float moveSpeed = 2.5f;
    public float minMoveTime = 0.6f;
    public float maxMoveTime = 1.4f;

    [Header("Shooting")]
    public float stopBeforeShootingTime = 0.25f;
    public float shootDelay = 0.25f;
    public float bulletSpeed = 8f;
    public float timeBetweenShots = 0.8f;

    [Header("Bullet Visual Rotation")]
    public float bulletRotationOffset = -90f;

    [Header("Health")]
    public int maxHealth = 3;

    private int currentHealth;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isMoving = false;
    private bool isDead = false;

    private Quaternion startingAimLocalRotation;
    private EnemySpawner enemySpawner;

    private void Awake()
    {
        currentHealth = maxHealth;
        enemySpawner = Object.FindFirstObjectByType<EnemySpawner>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (aimTransform == null)
        {
            aimTransform = transform;
        }

        startingAimLocalRotation = aimTransform.localRotation;

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");

            if (foundPlayer != null)
            {
                player = foundPlayer.transform;
            }
        }

        StartCoroutine(EnemyRoutine());
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;

        if (!isMoving)
            return;

        if (rb == null)
            return;

        Vector2 newPosition = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    private IEnumerator EnemyRoutine()
    {
        while (!isDead)
        {
            PickLeftOrRightStrafeDirection();

            isMoving = true;

            float moveTime = Random.Range(minMoveTime, maxMoveTime);
            yield return new WaitForSeconds(moveTime);

            isMoving = false;

            yield return new WaitForSeconds(stopBeforeShootingTime);

            float aimTimer = 0f;

            while (aimTimer < shootDelay && !isDead)
            {
                AimAtPlayer();
                aimTimer += Time.deltaTime;
                yield return null;
            }

            if (!isDead)
            {
                AimAtPlayer();
                ShootAtCurrentPlayerPosition();
            }

            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    private void PickLeftOrRightStrafeDirection()
    {
        if (player == null || rb == null)
        {
            moveDirection = Random.insideUnitCircle.normalized;
            return;
        }

        Vector2 directionToPlayer = ((Vector2)player.position - rb.position).normalized;

        Vector2 strafeDirection = new Vector2(-directionToPlayer.y, directionToPlayer.x);

        if (Random.value < 0.5f)
        {
            strafeDirection *= -1f;
        }

        moveDirection = strafeDirection.normalized;
    }

    private void AimAtPlayer()
    {
        if (player == null || aimTransform == null)
            return;

        Vector2 directionToPlayer = ((Vector2)player.position - (Vector2)aimTransform.position).normalized;

        float angle = Vector2.SignedAngle(Vector2.down, directionToPlayer);

        if (invertAimRotation)
        {
            angle *= -1f;
        }

        aimTransform.localRotation =
            startingAimLocalRotation *
            Quaternion.AngleAxis(angle, Vector3.up);
    }

    private void ShootAtCurrentPlayerPosition()
    {
        if (player == null || bulletPrefab == null || firePoint == null)
            return;

        Vector2 direction = ((Vector2)player.position - (Vector2)firePoint.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.Euler(0f, 0f, angle + bulletRotationOffset)
        );

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity = direction * bulletSpeed;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        GiveXP giveXP = GetComponent<GiveXP>();

        if (giveXP != null)
        {
            giveXP.DropXP();
        }

        if (enemySpawner != null)
        {
            enemySpawner.OnEnemyKilled(gameObject);
        }

        Destroy(gameObject);
    }
}