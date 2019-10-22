using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomObject : MonoBehaviour
{
    public string roomName;
    [SerializeField] private TextMeshProUGUI roomNameText, capacityText;

    public void JoinRoom() {
        GameObject.Find("LobbyController").GetComponent<LobbyController>().Menu_JoinRoom(roomName);
    }

    public void SetRoomName(string name) {
        roomNameText.text = name;
    }

    public void SetRoomCapacity(string text) {
        capacityText.text = text;
    }
        
}
