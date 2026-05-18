using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class XPBlob : MonoBehaviour
{
    [Header("XP")]
    public int xpAmount = 1;

    [Header("Collection")]
    public float moveSpeed = 8f;
    public float acceleration = 18f;
    public float collectDistance = 0.15f;

    [Header("Idle Animation")]
    public float floatAmplitude = 0.08f;
    public float floatFrequency = 4f;

    private Transform targetPlayer;
    private PlayerLevel playerLevel;
    private bool isCollecting = false;
    private float currentSpeed;
    private Vector3 startPosition;
    private float randomOffset;

    void Awake()
    {
        startPosition = transform.position;
        randomOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (isCollecting)
        {
            MoveTowardsPlayer();
        }
        else
        {
            IdleFloat();
        }
    }

    public void StartCollecting(Transform player)
    {
        if (player == null)
        {
            return;
        }

        targetPlayer = player;
        playerLevel = player.GetComponent<PlayerLevel>();
        isCollecting = true;
        currentSpeed = moveSpeed;
    }

    private void IdleFloat()
    {
        float yOffset = Mathf.Sin((Time.time + randomOffset) * floatFrequency) * floatAmplitude;
        transform.position = startPosition + new Vector3(0f, yOffset, 0f);
    }

    private void MoveTowardsPlayer()
    {
        if (targetPlayer == null)
        {
            Destroy(gameObject);
            return;
        }

        currentSpeed += acceleration * Time.deltaTime;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPlayer.position,
            currentSpeed * Time.deltaTime
        );

        float distance = Vector3.Distance(transform.position, targetPlayer.position);

        if (distance <= collectDistance)
        {
            Collect();
        }
    }

    private void Collect()
    {
        if (playerLevel != null)
        {
            playerLevel.AddXP(xpAmount);
        }

        Destroy(gameObject);
    }
}