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

    private Transform playerSpawn;

    private Player myPlayer;

    public static GameRoomController instance;

    public enum GameState {
        WAITING,
        STARTED,
        FINISHED
    }

    private GameState gameState;

    private void Awake() {
        instance = this;
    }

    public GameState GetGameState() {
        return gameState;
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

    public void SetGameState(GameState state) {
        photonView.RPC("SetGameStateRPC", RpcTarget.All, state);
    }
    [PunRPC]
    void SetGameStateRPC(GameState state) {
        gameState = state;
        Debug.Log("NEW GAME STATE : " + gameState.ToString());
    }

    private void Start() {
        if (PhotonNetwork.IsMasterClient) {
            SetGameState(GameState.WAITING);
        }

        myPlayer = PhotonNetwork.LocalPlayer;
        Debug.Log(myPlayer.ActorNumber);
        playerSpawn = playerSpawns[myPlayer.ActorNumber-1];

        GameObject player = PhotonNetwork.Instantiate("PhotonPlayer", playerSpawn.position, playerSpawn.rotation);

        AddPlayer();
    }

    private void Update() {
        if (playerCount == PhotonNetwork.CurrentRoom.MaxPlayers && gameState == GameState.WAITING && PhotonNetwork.IsMasterClient && gameState != GameState.FINISHED) {
            SetGameState(GameState.STARTED);
        }
        if (playerCount == 1 && gameState == GameState.STARTED) {
            SetGameState(GameState.FINISHED);
        }

        if (gameState == GameState.FINISHED) {
            endGamePanel.SetActive(true);
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
}
