using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomList : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private List<RoomInfo> rooms = new List<RoomInfo>();
    [SerializeField] private List<GameObject> roomsObjects = new List<GameObject>();
    [SerializeField] private GameObject roomContainer;

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {

        foreach(RoomInfo r in roomList) {
            if (rooms.Contains(r)) {
                if (r.IsOpen && r.MaxPlayers != 0 && r.PlayerCount != 0) {
                    RemoveRoomInList(r);
                    AddRoomInList(r);
                }
                else {
                    RemoveRoomInList(r);
                }
            }
            else {
                AddRoomInList(r);
            }
        }
        UpdateRoomObjects();
    }

    void AddRoomInList(RoomInfo room) {
            rooms.Add(room);
    }

    void RemoveRoomInList(RoomInfo room) {
        rooms.Remove(room);
    }

    void UpdateRoomObjects() {
        foreach(GameObject room in roomsObjects) {
            Destroy(room);
        }
        foreach(RoomInfo r in rooms) {
            if (r.IsOpen && r.IsVisible) {
                GameObject room = Instantiate(roomPrefab);
                room.transform.SetParent(roomContainer.transform, false);
                room.name = r.Name;

                string color = "";
                if (r.PlayerCount == r.MaxPlayers) {
                    color = "<color=#ff1717>";
                }
                else {
                    color = "<color=#30ff44>";

                }
                room.GetComponent<RoomObject>().SetRoomNameText(r.Name);
                room.GetComponent<RoomObject>().SetRoomCapacity(color + r.PlayerCount + "/" + r.MaxPlayers + "</color>");
                room.GetComponent<RoomObject>().SetRoomName(r.Name);
                roomsObjects.Add(room);
            }
        }
    }

}
