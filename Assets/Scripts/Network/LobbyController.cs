using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyController : MonoBehaviourPunCallbacks {
    [SerializeField] private string waitRoomName;
    [SerializeField] private TMP_InputField roomName;
    [SerializeField] private TMP_Dropdown playerNumber;
    [Header("MENU")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject joinRoomPanel;
    [SerializeField] private GameObject createRoomPanel;
    [SerializeField] private TMP_InputField nickName;

    private bool isOnline;
    private string playerName;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        if (PlayerPrefs.HasKey("playerName")) {
            playerName = PlayerPrefs.GetString("playerName");
            nickName.text = playerName;
        }
        else{
            playerName = "Player_" + Random.Range(0, 100).ToString();
            nickName.text = playerName;
            PlayerPrefs.SetString("playerName", playerName);
        }

        if (PlayerPrefs.HasKey("playerName")) {
            if(PlayerPrefs.GetString("playerName") == "") {
                playerName = "Player_" + Random.Range(0, 100).ToString();
                nickName.text = playerName;
                PlayerPrefs.SetString("playerName", playerName);
            }
        }
    }

    public override void OnConnectedToMaster() {
        Debug.Log("Connected To Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby() {
        Debug.Log("Lobby Joined");
        mainPanel.SetActive(true);
        isOnline = true;
    }

    public override void OnJoinedRoom() {
        Debug.Log("Joined Room");

        SceneManager.LoadScene(waitRoomName);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("No room");
    }



    public void CreateRoom() {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)(playerNumber.value+2);
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.EmptyRoomTtl = 0;
 
        string roomOptionName = roomName.text;
        PhotonNetwork.CreateRoom(roomOptionName, roomOptions, TypedLobby.Default);
    }

    public void SetNickName() {
        PhotonNetwork.NickName = nickName.text;
        PlayerPrefs.SetString("playerName", nickName.text);
    }

    //MENU CHOICES

    public void Menu_RandomJoin() {
        if (isOnline) {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public void Menu_JoinRoom(string name) {
        if (isOnline) {
            PhotonNetwork.JoinRoom(name);
        }
    }

    public void Menu_JoinRoomPanel() {
        joinRoomPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void Menu_CreateRoomPanel() {
        createRoomPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void Menu_ReturnMenuPanel() {
        joinRoomPanel.SetActive(false);
        createRoomPanel.SetActive(false);
        mainPanel.SetActive(true);
    }
}
