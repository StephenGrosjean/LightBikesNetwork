using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDeath : MonoBehaviourPunCallbacks
{
    private PhotonView view;
    private WallCreator wallCreator;
    private bool dead = false;

    private void Awake() {
        view = GetComponent<PhotonView>();
        wallCreator = GetComponent<WallCreator>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (view.IsMine) {
            if (collision.collider.tag == "Wall" && collision.gameObject != wallCreator.GetCurrentWall()) {
                Death();
            }
        }
    }

    public void Death() {
        if (!dead) {
            if (photonView.IsMine) {
                dead = true;
                GetComponent<PlayerController>().canControl = false;
                PhotonNetwork.Instantiate("DeathObj", transform.position, transform.rotation);
                GameRoomController.instance.RemovePlayer();
                GameRoomController.instance.SetEndGameText();
            }
        }
    }
}
