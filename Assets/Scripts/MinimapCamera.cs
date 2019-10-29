using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float xOffset, yOffset, zOffset;

    private Transform target;
    private Vector3 velocity = Vector3.zero;

    void Update(){
        if(target == null) {
            target = Camera.main.GetComponent<NetworkCamera>().GetTarget();
        }
        else {
            Vector3 goalPos = target.localPosition + new Vector3(xOffset, yOffset, zOffset);

            transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
        }
    }
}
