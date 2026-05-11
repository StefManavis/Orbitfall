using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    public float deadZonePixels = 20f;

    [Header("Combat")]
    public GameObject bulletPrefab;

    private Rigidbody2D rb;
    private PlayerStats stats;
    private Camera cam;

    private Vector2 input;
    private Vector2 touchStart;
    private float fireTimer;
    private float radius;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
        cam = Camera.main;
        radius = GetComponent<CircleCollider2D>().radius;
        rb.centerOfMass = Vector2.zero;
    }

    void Update()
    {
        HandleInput();
        Rotate();
        HandleShooting();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = input * stats.moveSpeed;
    }

    void LateUpdate()
    {
        ClampToScreen();
    }

    // ---------------- INPUT ----------------
    void HandleInput()
    {
        input = Vector2.zero;

        // TOUCH (Android)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                touchStart = touch.position;

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Vector2 delta = touch.position - touchStart;
                if (delta.magnitude > deadZonePixels)
                    input = delta.normalized;
            }
        }

#if UNITY_EDITOR
        // MOUSE (Editor testing)
        if (Input.GetMouseButtonDown(0))
            touchStart = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Vector2 delta = (Vector2)Input.mousePosition - touchStart;
            if (delta.magnitude > deadZonePixels)
                input = delta.normalized;
        }
#endif
    }

    // ---------------- ROTATION ----------------
    void Rotate()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }

    // ---------------- SHOOTING ----------------
    void HandleShooting()
    {
        fireTimer -= Time.deltaTime;

        if (rb.linearVelocity.sqrMagnitude < 0.05f && fireTimer <= 0f)
        {
            Transform target = GetNearestEnemy();
            if (target != null)
            {
                StartCoroutine(ShootRoutine(target));
                fireTimer = stats.fireRate;
            }
        }
    }

    IEnumerator ShootRoutine(Transform target)
    {
        Vector2 dir = (target.position - transform.position).normalized;

         float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        rb.MoveRotation(angle);

        for (int v = 0; v < stats.volleysPerShot; v++)
        {
            FireVolley(dir);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void FireVolley(Vector2 direction)
    {
        int count = stats.bulletsPerVolley;
        float spacing = 0.3f; // distance between parallel bullets

        // Base rotation (same for all bullets)
        float rotZ =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg
            - 90f;

        // Perpendicular direction (right of firing direction)
        Vector2 right = new Vector2(direction.y, -direction.x);

        for (int i = 0; i < count; i++)
        {
            float offsetIndex = i - (count - 1) * 0.5f;
            Vector2 spawnOffset = right * offsetIndex * spacing;

            Instantiate(
                bulletPrefab,
                (Vector2)transform.position + spawnOffset,
                Quaternion.Euler(0, 0, rotZ)
            );
        }
    }


    // ---------------- TARGETING ----------------
    Transform GetNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = e.transform;
            }
        }

        return nearest;
    }

    // ---------------- SCREEN CLAMP ----------------
    void ClampToScreen()
    {
        Vector3 pos = transform.position;

        float height = cam.orthographicSize;
        float width = height * cam.aspect;

        pos.x = Mathf.Clamp(pos.x, -width + radius, width - radius);
        pos.y = Mathf.Clamp(pos.y, -height + radius, height - radius);

        transform.position = pos;
    }
}
