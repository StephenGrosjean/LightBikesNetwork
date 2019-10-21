using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MaterialSync : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> meshRenderers;
    private Color color;
    private Material material;

    public void UpdateColor(Color newColor) {
        color = newColor;
        GetComponent<PhotonView>().RPC("UpdateMaterialColor", RpcTarget.All);
    }

    public void UpdateMaterial(Material newMaterial) {
        material = newMaterial;
        GetComponent<PhotonView>().RPC("UpdateMaterial", RpcTarget.All);
    }

    [PunRPC]
    private void UpdateMaterialColor() {
        foreach (MeshRenderer m in meshRenderers) {
            m.material.color = color;
            if(m.gameObject.tag == "Wall") {
                m.material.color = new Color(color.r, color.g, color.b, 140);
            }
        }
    }

    [PunRPC]
    private void UpdateMaterial() {
        foreach (MeshRenderer m in meshRenderers) {
            m.material = material;
        }
    }
}
