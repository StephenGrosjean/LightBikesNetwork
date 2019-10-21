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
        Debug.Log("No room");
    }



    public void CreateRoom() {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)(playerNumber.value+2);
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;

        string roomOptionName = roomName.text;
        PhotonNetwork.JoinOrCreateRoom(roomOptionName, roomOptions, TypedLobby.Default);
    }

    //MENU CHOICES

    public void Menu_RandomJoin() {
        if (isOnline) {
            PhotonNetwork.JoinRandomRoom();
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
