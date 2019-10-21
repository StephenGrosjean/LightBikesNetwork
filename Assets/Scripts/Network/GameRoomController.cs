using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameRoomController : MonoBehaviourPunCallbacks {
    [SerializeField] private Material masterMaterial, clientMaterial;

    [SerializeField] private Transform[] playerSpawns;
    private Transform playerSpawn;

    private Player myPlayer;

    private void Start() {
        myPlayer = PhotonNetwork.LocalPlayer;

        if (PhotonNetwork.IsMasterClient) {
            playerSpawn = playerSpawns[0];
        }
        else {
            playerSpawn = playerSpawns[1];
        }
        GameObject player = PhotonNetwork.Instantiate("PhotonPlayer", playerSpawn.position, playerSpawn.rotation);
        


    }
}
