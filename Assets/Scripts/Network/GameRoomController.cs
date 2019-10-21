using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class GameRoomController : MonoBehaviourPunCallbacks {
    [SerializeField] private Material masterMaterial, clientMaterial;

    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private int playerCount;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private TextMeshProUGUI endGameText;

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
        if (playerCount == PhotonNetwork.CurrentRoom.MaxPlayers && gameState == GameState.WAITING && PhotonNetwork.IsMasterClient) {
            SetGameState(GameState.STARTED);
        }
        if (playerCount == 1 && gameState == GameState.STARTED) {
            endGamePanel.SetActive(true);
        }
    }

    public void SetEndGameText() {
        endGameText.text = "You Lose";
        endGamePanel.SetActive(true);
    }
}
