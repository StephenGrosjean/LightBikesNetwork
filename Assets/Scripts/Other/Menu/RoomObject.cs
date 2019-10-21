using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject : MonoBehaviour
{
    public string roomName;

    public void JoinRoom() {
        GameObject.Find("LobbyController").GetComponent<LobbyController>().Menu_JoinRoom(roomName);
    }

}
