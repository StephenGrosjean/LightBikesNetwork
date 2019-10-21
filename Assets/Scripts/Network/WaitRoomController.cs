using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class WaitRoomController : MonoBehaviourPunCallbacks {

    public int playersInRoom;
    public TextMeshProUGUI text;
    public string gameSceneName;

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
