using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPlayerColors : MonoBehaviour
{
    [SerializeField] private List<Color> playerColors = new List<Color>();
    public static GlobalPlayerColors instance;

    private void Awake() {
        instance = this;
    }

    public Color GetPlayerColor(int id) {
        return playerColors[id-1];
    }
}
