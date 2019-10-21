using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class WaitRoomController : MonoBehaviourPunCallbacks {

    [SerializeField] private int playersInRoom;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string gameSceneName;
    [SerializeField] private TextMeshProUGUI playerNumber, roomName;

    private void Start() {
        playerNumber.text = "Max Players : " + PhotonNetwork.CurrentRoom.MaxPlayers;
        roomName.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name;

    }

    private void Update() {
        playersInRoom = PhotonNetwork.PlayerList.Length;
        text.text = "Players : " + playersInRoom.ToString();

        if(playersInRoom == PhotonNetwork.CurrentRoom.MaxPlayers) {
            ConnectToGame();
        }

    }

    public void ConnectToGame() {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        SceneManager.LoadScene(gameSceneName);
    }
    

}
