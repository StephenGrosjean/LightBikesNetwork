using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private Transform target;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3f;
    public float xOffset, yOffset, zOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null) {
            target = Camera.main.GetComponent<NetworkCamera>().GetTarget();
        }
        else {
            Vector3 goalPos = target.localPosition + new Vector3(xOffset, yOffset, zOffset);

            transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
        }
    }
}
