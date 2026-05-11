using UnityEngine;
using System.Collections.Generic;

public class RoomController : MonoBehaviour
{
    public Transform playerSpawn;
    public List<Transform> enemySpawnPoints;

    EnemySpawner spawner;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            EnterRoom(player);
    }


    void Awake()
    {
        spawner = FindFirstObjectByType<EnemySpawner>();
    }

    public void EnterRoom(GameObject player)
    {
        // Move player to spawn
        player.transform.position = playerSpawn.position;

        // Tell spawner to use THIS room's spawn points
        spawner.SpawnRoom(enemySpawnPoints);
    }
}
