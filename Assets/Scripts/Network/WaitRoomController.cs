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
    [SerializeField] private TextMeshProUGUI playerNumber, roomName;
    [SerializeField] private TextMeshProUGUI countdownText;
    public List<string> names = new List<string>();

    private int countdown = 5;
    private bool counting;

    private void Start() {
        playerNumber.text = "Max Players : " + PhotonNetwork.CurrentRoom.MaxPlayers;
        roomName.text = "Room Name : " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    private void Update() {
        playersInRoom = PhotonNetwork.PlayerList.Length;
        text.text = "Players : " + playersInRoom.ToString();

        if(playersInRoom == PhotonNetwork.CurrentRoom.MaxPlayers) {
            if (!counting && PhotonNetwork.IsMasterClient) {
                counting = true;
                InvokeRepeating("Count", 0, 1);
            }
            if (countdown <= 0) {
                ConnectToGame();
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
            playerList.text += p.NickName;
            if (p.IsMasterClient) {
                playerList.text += @" <sprite=""crowns"" index=7>";
            }
            playerList.text += "\n";
        }
    }


    void Count() {
        photonView.RPC("CountRPC", RpcTarget.All);
    }
    [PunRPC]
    void CountRPC() {
        countdown--;
        countdownText.text = "Starting in : " + countdown.ToString();
    }
    


}
