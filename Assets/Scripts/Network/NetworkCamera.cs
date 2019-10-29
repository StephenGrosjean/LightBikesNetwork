using UnityEngine;
using System.Collections;

public class NetworkCamera : MonoBehaviour {

    [SerializeField] private Transform target;
    [SerializeField] private Transform center;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float xOffset, yOffset, zOffset;
    [SerializeField] private float xOffsetRot, yOffsetRot, zOffsetRot;

    private Vector3 velocity = Vector3.zero;
    private float timeCount;

    public void AssignPlayer(Transform player) {
        target = player;
    }

    void Update() {
        if (target != null) {
            Vector3 goalPos = target.localPosition + new Vector3(xOffset, yOffset, zOffset);

            center.position = Vector3.SmoothDamp(center.position, goalPos, ref velocity, smoothTime);


            Vector3 goalRot = target.rotation.eulerAngles + new Vector3(xOffsetRot, yOffsetRot, zOffsetRot);
            center.rotation = Quaternion.Lerp(center.rotation, target.rotation, 0.03f);

        }
    }

    public Transform GetTarget() {
        return target;
    }

}