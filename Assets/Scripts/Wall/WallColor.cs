using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WallColor : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback {
    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        object[] initData = info.photonView.InstantiationData;
        photonView.RPC("SetColor", RpcTarget.All, initData[0]);
    }

    [PunRPC]
    void SetColor(int id) {
        GetComponent<Renderer>().material.color = GlobalPlayerColors.instance.GetPlayerColor(id);
    }
}
