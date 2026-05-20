using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private GameObject roomManagerObject;

    [Header("Enemy Types")]
    public List<GameObject> enemyPrefabs;

    [Header("Spawn Settings")]
    public int enemiesToSpawn = 5;

    [Header("Spawn Safety")]
    public float spawnCheckRadius = 0.45f;
    public float minDistanceFromPlayer = 3f;
    public int maxSpawnAttemptsPerEnemy = 30;

    [Header("Collision Checks")]
    public LayerMask obstacleLayer;
    public LayerMask enemyLayer;

    [Header("Room Clear XP Collection")]
    public float delayBeforeXPCollect = 0.25f;
    public float delayBeforeNextRoom = 0.35f;
    public float maxXPCollectionWaitTime = 3f;

    private readonly List<GameObject> aliveEnemies = new List<GameObject>();
    private bool roomClearing = false;

    private void Awake()
    {
        roomManagerObject = GameObject.Find("RoomManager");
    }

    public void SpawnRoom(BoxCollider2D spawnArea, Transform roomParent)
    {
        roomClearing = false;
        ClearRemainingEnemies();

        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("EnemySpawner: No enemy prefabs assigned.");
            return;
        }

        if (spawnArea == null)
        {
            Debug.LogWarning("EnemySpawner: Spawn area is missing.");
            return;
        }

        if (roomParent == null)
        {
            Debug.LogWarning("EnemySpawner: Room parent is missing.");
            return;
        }

        int spawnedCount = 0;

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            bool spawned = TrySpawnEnemy(spawnArea, roomParent);

            if (spawned)
            {
                spawnedCount++;
            }
            else
            {
                Debug.LogWarning("EnemySpawner: Failed to find a valid spawn position for enemy " + i + ".");
            }
        }

        Debug.Log("EnemySpawner: Spawned " + spawnedCount + " enemies.");
    }

    private void ClearRemainingEnemies()
    {
        for (int i = aliveEnemies.Count - 1; i >= 0; i--)
        {
            if (aliveEnemies[i] != null)
            {
                Destroy(aliveEnemies[i]);
            }
        }

        aliveEnemies.Clear();
    }

    private bool TrySpawnEnemy(BoxCollider2D spawnArea, Transform roomParent)
    {
        for (int attempt = 0; attempt < maxSpawnAttemptsPerEnemy; attempt++)
        {
            Vector2 spawnPosition = GetRandomPointInsideBox(spawnArea);

            if (!IsValidSpawnPosition(spawnPosition))
            {
                continue;
            }

            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            GameObject enemy = Instantiate(
                prefab,
                spawnPosition,
                prefab.transform.rotation,
                roomParent
            );

            aliveEnemies.Add(enemy);
            return true;
        }

        return false;
    }

    private Vector2 GetRandomPointInsideBox(BoxCollider2D box)
    {
        Bounds bounds = box.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        return new Vector2(x, y);
    }

    private bool IsValidSpawnPosition(Vector2 position)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            float distanceFromPlayer = Vector2.Distance(position, player.transform.position);

            if (distanceFromPlayer < minDistanceFromPlayer)
            {
                return false;
            }
        }

        if (obstacleLayer.value != 0)
        {
            Collider2D obstacleHit = Physics2D.OverlapCircle(position, spawnCheckRadius, obstacleLayer);

            if (obstacleHit != null)
            {
                return false;
            }
        }

        if (enemyLayer.value != 0)
        {
            Collider2D enemyHit = Physics2D.OverlapCircle(position, spawnCheckRadius, enemyLayer);

            if (enemyHit != null)
            {
                return false;
            }
        }

        return true;
    }

    public void OnEnemyKilled(GameObject enemy)
    {
        if (aliveEnemies.Contains(enemy))
        {
            aliveEnemies.Remove(enemy);
        }

        if (aliveEnemies.Count == 0 && !roomClearing)
        {
            OnRoomCleared();
        }
    }

    private void OnRoomCleared()
    {
        roomClearing = true;
        StartCoroutine(CollectXPThenLoadNextRoom());
    }

    private IEnumerator CollectXPThenLoadNextRoom()
    {
        yield return new WaitForSeconds(delayBeforeXPCollect);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("EnemySpawner: Player was not found for XP collection.");
            yield return new WaitForSeconds(delayBeforeNextRoom);
            LoadNext();
            yield break;
        }

        XPBlob[] xpBlobs = Object.FindObjectsByType<XPBlob>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        );

        foreach (XPBlob blob in xpBlobs)
        {
            if (blob != null)
            {
                blob.StartCollecting(player.transform);
            }
        }

        float timer = 0f;

        while (Object.FindObjectsByType<XPBlob>(
            FindObjectsInactive.Exclude,
            FindObjectsSortMode.None
        ).Length > 0 && timer < maxXPCollectionWaitTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(delayBeforeNextRoom);

        LoadNext();
    }

    private void LoadNext()
    {
        if (roomManagerObject != null)
        {
            roomManagerObject.SendMessage("LoadNextRoom");
        }
        else
        {
            Debug.LogWarning("EnemySpawner: RoomManager object was not found.");
        }
    }
}