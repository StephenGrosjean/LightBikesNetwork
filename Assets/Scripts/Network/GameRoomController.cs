using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class GameRoomController : MonoBehaviourPunCallbacks {
    [SerializeField] private Material masterMaterial, clientMaterial;
    [SerializeField] private Transform[] playerSpawns;
    [SerializeField] private GameObject endGamePanel, spectatePanel;
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private GameObject mainCamera, spectateCamera;
    [SerializeField] private GameObject endGameDummy;
    [SerializeField] private BikesSpriteColor bikeSpriteColorScript;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private int playerCount;

    private Transform playerSpawn;
    private Player myPlayer;
    public bool endGame;
    private bool started;


    public static GameRoomController instance; //Singleton

    private void Awake() {
        instance = this;
    }

    public void RemovePlayer(int ID) {
        photonView.RPC("RemovePlayerRPC", RpcTarget.All);
        photonView.RPC("RemoveBikeToStrings", RpcTarget.All, ID);
    }

    public void AddPlayer() {
        photonView.RPC("AddPlayerRPC", RpcTarget.All);
    }

    private void Start() {
        Invoke("LateStart", 1);
    }

    private void LateStart() {
        myPlayer = PhotonNetwork.LocalPlayer;
        playerSpawn = playerSpawns[myPlayer.ActorNumber - 1];

        GameObject player = PhotonNetwork.Instantiate("PhotonPlayer", playerSpawn.position, playerSpawn.rotation);

        AddPlayer();
        photonView.RPC("AddBikeToStrings", RpcTarget.All, myPlayer.ActorNumber);
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
        if (started) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                ToggleMenuPanel();
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        if (PhotonNetwork.IsMasterClient) {
            RemovePlayer(otherPlayer.ActorNumber);
        }
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

    public void ToggleMenuPanel() {
        menuPanel.SetActive(!menuPanel.activeInHierarchy);
    }

    [PunRPC]
    void AddBikeToStrings(int ID) {
        bikeSpriteColorScript.AddBike(ID);
    }

    [PunRPC]
    void RemoveBikeToStrings(int ID) {
        bikeSpriteColorScript.BikeDestroyed(ID);
    }

    [PunRPC]
    void RemovePlayerRPC() {
        playerCount--;
    }

    [PunRPC]
    void AddPlayerRPC() {
        playerCount++;
    }
}
