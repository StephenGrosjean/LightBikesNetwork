using UnityEngine;
using System.Collections;

public class NetworkCamera : MonoBehaviour {

    public Transform target;
    public float smoothTime = 0.3f;
    public float xOffset, yOffset, zOffset;
    private Vector3 velocity = Vector3.zero;


    public void AssignPlayer(Transform player) {
        target = player;
    }

    void Update() {
        if (target != null) {
            Vector3 goalPos = target.position + new Vector3(xOffset, yOffset, zOffset);
            goalPos.y = transform.position.y;
            transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
        }
    }
}