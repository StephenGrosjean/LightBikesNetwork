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

    public enum Colors {
        RED,
        BLUE,
        YELLOW
    }

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
    void AddMessageRPC(string text, Colors color) {
        lines.Add(ParseColor(color) + text + "</color>");
        if (lines.Count > maxLines) {
            lines.Remove(lines[0]);
        }
        UpdateLines();
    }

    public void AddMessage(string text, Colors color) {
        photonView.RPC("AddMessageRPC", RpcTarget.All, text, color);
    }

    private string ParseColor(Colors color) {
        switch (color) {
            case Colors.RED:
                return "<color=#ff1717>";
            case Colors.BLUE:
                return "<color=#0011FF>";
            case Colors.YELLOW:
                return "<color=#fdff00>";
            default:
                return "<color=#ff1717>";
        }
    }
    

}
