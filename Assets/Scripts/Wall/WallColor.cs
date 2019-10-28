using UnityEngine;
using Photon.Pun;

public class WallColor : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback {

    private Renderer renderer;

    private void Awake() {
        renderer = GetComponent<Renderer>();
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) {
        object[] initData = info.photonView.InstantiationData;
        photonView.RPC("SetColor", RpcTarget.All, initData[0]);
    }

    [PunRPC]
    void SetColor(int id) {
        renderer.material.color = GlobalPlayerColors.instance.GetPlayerColor(id);
    }
}
