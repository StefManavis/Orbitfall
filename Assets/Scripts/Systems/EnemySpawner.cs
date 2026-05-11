using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{

    RoomManager roomManager;

    void Awake()
    {
        roomManager = FindFirstObjectByType<RoomManager>();
    }

    [Header("Enemy Types")]
    public List<GameObject> enemyPrefabs;   // drag enemy prefabs here

    private List<GameObject> aliveEnemies = new List<GameObject>();

    // ---------------- SPAWN FROM ROOM ----------------
    public void SpawnRoom(List<Transform> spawnPoints)
    {
        aliveEnemies.Clear();

        if (enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("EnemySpawner: No enemy prefabs assigned!");
            return;
        }

        foreach (Transform sp in spawnPoints)
        {
            GameObject prefab =
                enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

            GameObject enemy = Instantiate(
                prefab,
                sp.position,
                Quaternion.identity
            );

            aliveEnemies.Add(enemy);
        }
    }

    // ---------------- ENEMY DEATH ----------------
    public void OnEnemyKilled(GameObject enemy)
    {
        if (aliveEnemies.Contains(enemy))
            aliveEnemies.Remove(enemy);

        if (aliveEnemies.Count == 0)
        {
            OnRoomCleared();
        }
    }

    // ---------------- ROOM CLEAR ----------------
    void OnRoomCleared()
    {
        UpgradeManager.Instance.OfferUpgrade();

        // TEMP: auto-advance after upgrade
        Invoke(nameof(LoadNext), 0.5f);
    }

    void LoadNext()
    {
        roomManager.LoadNextRoom();
    }

}
