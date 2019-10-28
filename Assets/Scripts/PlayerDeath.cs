using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDeath : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject model;
    private WallCreator wallCreator;
    private bool dead = false;

    private void Awake() {
        wallCreator = GetComponent<WallCreator>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (photonView.IsMine) {
            if (collision.collider.tag == "Wall" && collision.gameObject != wallCreator.GetCurrentWall()) {
                Death();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (photonView.IsMine) {
            if (other.tag == "Wall" && other.gameObject != wallCreator.GetCurrentWall()) {
                Death();
            }
        }
    }

    public void Death() {
        if (!dead) {
            if (photonView.IsMine) {
                dead = true;
                GetComponent<PlayerController>().canControl = false;
                object[] initData = new object[1];
                initData[0] = GetComponent<PlayerController>().GetPlayerID();
                PhotonNetwork.Instantiate("DeathObj", transform.position, transform.rotation, 0, initData);
                GameRoomController.instance.RemovePlayer(photonView.OwnerActorNr);
                GameRoomController.instance.SetEndGameText();
                photonView.RPC("DisableModel", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void DisableModel() {
        model.SetActive(false);
    }
}
