using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviourPunCallbacks {
    [SerializeField] private string waitRoomName;
    bool isOnline;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster() {
        Debug.Log("Connected To Master");
        isOnline = true;
    }

    public override void OnJoinedRoom() {
        Debug.Log("Joined Room");

        SceneManager.LoadScene(waitRoomName);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        CreateRoom();
    }

    public void RandomJoin() {
        if (isOnline) {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public void CreateRoom() {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        string roomName = "Room_" + Random.Range(0, 100).ToString();

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }
}
