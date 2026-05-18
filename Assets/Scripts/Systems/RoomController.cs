using UnityEngine;
using System.Collections.Generic;

public class RoomController : MonoBehaviour
{
    [Header("Player Spawn")]
    public Transform playerSpawn;

    [Header("Enemy Spawn Area")]
    public BoxCollider2D enemySpawnArea;

    [Header("Old Manual Spawn Points")]
    public List<Transform> enemySpawnPoints;

    private EnemySpawner spawner;

    void Awake()
    {
        spawner = FindFirstObjectByType<EnemySpawner>();

        if (enemySpawnArea == null)
        {
            Transform spawnAreaTransform = transform.Find("EnemySpawnArea_Top");

            if (spawnAreaTransform != null)
            {
                enemySpawnArea = spawnAreaTransform.GetComponent<BoxCollider2D>();
            }
        }
    }

    public void EnterRoom(GameObject player)
    {
        if (player == null)
        {
            Debug.LogWarning("RoomController: Player is missing.");
            return;
        }

        if (playerSpawn != null)
        {
            player.transform.position = playerSpawn.position;
        }
        else
        {
            Debug.LogWarning("RoomController: Player spawn is missing.");
        }

        if (spawner == null)
        {
            Debug.LogWarning("RoomController: EnemySpawner was not found.");
            return;
        }

        if (enemySpawnArea == null)
        {
            Debug.LogWarning("RoomController: EnemySpawnArea_Top is missing or has no BoxCollider2D.");
            return;
        }

        spawner.SpawnRoom(enemySpawnArea, transform);
    }
}