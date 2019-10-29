using UnityEngine;

public class Spectate : MonoBehaviour
{
    //Rotate Spectate Camera
    [SerializeField] private float speed;
    private void Update() {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
