using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class GameRoomController : MonoBehaviourPunCallbacks {
    [SerializeField] private Material masterMaterial, clientMaterial;

    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private int playerCount;
    [SerializeField] private GameObject endGamePanel, spectatePanel;
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private GameObject mainCamera, spectateCamera;
    [SerializeField] private GameObject endGameDummy;

    private Transform playerSpawn;

    private Player myPlayer;

    public bool endGame;
    private bool started;

    public static GameRoomController instance;

    private void Awake() {
        instance = this;
    }

    public void RemovePlayer() {
        photonView.RPC("RemovePlayerRPC", RpcTarget.All);
    }
    [PunRPC]
    void RemovePlayerRPC() {
        playerCount--;
    }


    public void AddPlayer() {
        photonView.RPC("AddPlayerRPC", RpcTarget.All);
    }
    [PunRPC]
    void AddPlayerRPC() {
        playerCount++;
    }

    private void Start() {
        Invoke("LateStart", 1);
    }

    private void LateStart() {
        myPlayer = PhotonNetwork.LocalPlayer;
        playerSpawn = playerSpawns[myPlayer.ActorNumber - 1];

        GameObject player = PhotonNetwork.Instantiate("PhotonPlayer", playerSpawn.position, playerSpawn.rotation);

        AddPlayer();
    }

    private void Update() {
        if (!started && playerCount == PhotonNetwork.CurrentRoom.MaxPlayers) {
            StartGame();
        }
        if (!endGame && playerCount == 1 && started) {
            endGamePanel.SetActive(true);
            endGameDummy.SetActive(true);
            endGame = true;
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        RemovePlayer();
    }

    public void SetEndGameText() {
        TextEvent.instance.AddMessage(PhotonNetwork.NickName + " Died", TextEvent.Colors.RED);
        endGameText.text = "You Lose";
        endGamePanel.SetActive(true);
    }

    public void Disconnect() {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("LobbyRoom");
    }

    public void Spectate() {
        endGamePanel.SetActive(false);
        spectatePanel.SetActive(true);
        spectateCamera.SetActive(true);
        mainCamera.SetActive(false);
    }

    void StartGame() {
        started = true;
    }

    public GameObject GetDummy() {
        return endGameDummy;
    }

}
