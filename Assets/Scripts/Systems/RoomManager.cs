using UnityEngine;
using System.Collections.Generic;

public class RoomManager : MonoBehaviour
{
    [Header("Room Pool")]
    public List<GameObject> roomPrefabs;

    [Header("Room Progress")]
    public int currentRoomNumber = 0;

    private GameObject currentRoom;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        LoadNextRoom();
    }

    public void LoadNextRoom()
    {
        currentRoomNumber++;

        if (currentRoom != null)
        {
            Destroy(currentRoom);
        }

        if (roomPrefabs == null || roomPrefabs.Count == 0)
        {
            Debug.LogWarning("RoomManager: No room prefabs assigned.");
            return;
        }

        GameObject roomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Count)];

        currentRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);

        RoomController roomController = currentRoom.GetComponent<RoomController>();

        if (roomController != null)
        {
            roomController.EnterRoom(player);
        }
        else
        {
            Debug.LogWarning("RoomManager: Spawned room has no RoomController.");
        }

        Debug.Log("Entered Room: " + currentRoomNumber);
    }
}