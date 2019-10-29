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
    [SerializeField] private TextMeshProUGUI playerList;
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI countdownText;

    private int countdown = 5;
    private bool counting;

    private void Start() {
        roomName.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    private void Update() {
        playersInRoom = PhotonNetwork.PlayerList.Length;
        text.text = "Players : " + playersInRoom.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        if(playersInRoom == PhotonNetwork.CurrentRoom.MaxPlayers) {
            if (!counting && PhotonNetwork.IsMasterClient) {
                counting = true;
                InvokeRepeating("Count", 0, 1);
            }
            if (countdown <= 0) {
                ConnectToGame();
            }
        }
        else {
            if (counting && PhotonNetwork.IsMasterClient) {
                CancelInvoke("Count");
                counting = false;
                photonView.RPC("ResetCountdown", RpcTarget.All);
            }
        }
    }

    public void ConnectToGame() {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        SceneManager.LoadScene(gameSceneName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        UpdatePlayerList();
    }

    void UpdatePlayerList() {
        playerList.text = "";
        Player[] players = PhotonNetwork.PlayerList;
        
        foreach(Player p in players) {
            if (p.IsMasterClient) {
                playerList.text += @" <sprite index=7>";
            }
            playerList.text += p.NickName;
            playerList.text += "\n";
        }
    }


    void Count() {
        photonView.RPC("CountRPC", RpcTarget.All);
    }
    
    public void QuitRoom() {
        SoundManager.instance.PlayEffect(SoundManager.Effect.BUTTON_CLICK);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("LobbyRoom");
    }

    [PunRPC]
    void ResetCountdown() {
        countdown = 5;
        countdownText.text = "";
    }

    [PunRPC]
    void CountRPC() {
        countdown--;
        countdownText.text = "Starting in : " + countdown.ToString();
        SoundManager.instance.PlayEffect(SoundManager.Effect.VALIDATION);
    }

}
