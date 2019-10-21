using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalPlayerColors : MonoBehaviour
{
    [SerializeField] private Color P1_Color, P2_Color, P3_Color, P4_Color;
    public static GlobalPlayerColors instance;

    private void Awake() {
        instance = this;
    }

    public Color GetPlayerColor(int id) {
        switch (id) {
            case 1:
                return P1_Color;
            case 2:
                return P2_Color;
            case 3:
                return P3_Color;
            case 4:
                return P4_Color;
            default:
                return Color.white;
        }  
    }
}
