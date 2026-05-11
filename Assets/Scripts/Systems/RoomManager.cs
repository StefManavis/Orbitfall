using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    [Header("Room Pool")]
    public List<GameObject> roomPrefabs;

    GameObject currentRoom;
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        LoadNextRoom();
    }

    public void LoadNextRoom()
    {
        // Destroy old room
        if (currentRoom != null)
            Destroy(currentRoom);

        // Pick random room
        GameObject roomPrefab =
            roomPrefabs[Random.Range(0, roomPrefabs.Count)];

        // Spawn room
        currentRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);

        // Enter it
        RoomController rc = currentRoom.GetComponent<RoomController>();
        rc.EnterRoom(player);
    }
}
