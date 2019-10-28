using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class DeathObject : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback {


    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        object[] initData = info.photonView.InstantiationData;
        int playerID = (int)initData[0];
        GetComponentInChildren<ParticleSystem>().startColor = GlobalPlayerColors.instance.GetPlayerColor(playerID);
    }


}
