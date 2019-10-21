using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerDeath : MonoBehaviour
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
                //view.RPC("Death", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void Death() {
        if (!dead) {
            dead = true;
            GetComponent<PlayerController>().canControl = false;
            PhotonNetwork.Instantiate("DeathObj", transform.position, transform.rotation);
        }
    }
}
