using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class RoomList : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private List<RoomInfo> rooms = new List<RoomInfo>();
    [SerializeField] private List<GameObject> roomsObjects = new List<GameObject>();
    [SerializeField] private GameObject roomContainer;

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        rooms = roomList;
        UpdateRoomList();
    }

    void UpdateRoomList() {
        foreach(GameObject r in roomsObjects) { 
            Destroy(r);
        }

        roomsObjects.Clear();

        foreach(RoomInfo r in rooms) {
            GameObject room = Instantiate(roomPrefab);
            room.transform.parent = roomContainer.transform;
            room.name = r.Name;

            string color = "";
            if(r.PlayerCount == r.MaxPlayers) {
                color = "<color=#ff1717>";
            }
            else {
                color = "<color=#30ff44>";

            }
            room.GetComponent<RoomObject>().SetRoomName(r.Name);
            room.GetComponent<RoomObject>().SetRoomCapacity(color + r.PlayerCount + "/" + r.MaxPlayers + "</color>");
            room.GetComponent<RoomObject>().roomName = r.Name;
            roomsObjects.Add(room);
        }
    }

}
