using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class TextEvent : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private int maxLines;
    private List<string> lines = new List<string>();
    private string txt;

    public static TextEvent instance;

    private void Awake() {
        instance = this;
    }

    void UpdateLines() {
        txt = "";
        foreach(string s in lines) {
            txt += "\n" + s;
        }
        textBox.text = txt;
    }

    [PunRPC]
    void AddDeathMessageRPC(string text) {
        lines.Add("<color=#ff1717>" + text + "</color>");
        if (lines.Count > maxLines) {
            lines.Remove(lines[0]);
        }
        UpdateLines();
    }

    public void AddDeathMessage(string text) {
        photonView.RPC("AddDeathMessageRPC", RpcTarget.All, text);
    }
    

}
