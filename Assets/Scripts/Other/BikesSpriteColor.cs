using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BikesSpriteColor : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bikeText;

    struct Bike {
        public int ID;
        public string color;
        public bool destroyed;
        public bool assigned;
    }
    private const int maxPlayers = 10;
    private Bike[] bikes = new Bike[maxPlayers];

    void Start()
    {
    }

    void Update()
    {
        bikeText.text = "";
        foreach(Bike b in bikes) {
            if (b.assigned) {
                if (!b.destroyed) {
                    bikeText.text += " <sprite index=3 color=" + b.color + ">";
                }
                else {
                    bikeText.text += " <sprite index=3 color=#252926>";
                }
            }
        }
    }

    public void AddBike(int ID) {
        Bike bike = new Bike();
        bike.assigned = true;
        bike.ID = ID-1;
        bike.color = "#" + ColorUtility.ToHtmlStringRGBA(GlobalPlayerColors.instance.GetPlayerColor(ID));
        bikes[ID-1] = bike; 
    }

    public void BikeDestroyed(int ID) {
        bikes[ID-1].destroyed = true;
    }

}
