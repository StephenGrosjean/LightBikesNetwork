using UnityEngine;
using TMPro;

public class RoomObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomNameText, capacityText;
    [SerializeField] private string roomName;

    public void JoinRoom() {
        GameObject.Find("LobbyController").GetComponent<LobbyController>().Menu_JoinRoom(roomName);
    }

    public void SetRoomName(string value) {
        roomName = value;
    }

    public void SetRoomNameText(string name) {
        roomNameText.text = name;
    }

    public void SetRoomCapacity(string text) {
        capacityText.text = text;
    }
        
}
