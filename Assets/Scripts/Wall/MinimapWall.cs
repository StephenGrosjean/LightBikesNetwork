using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapWall : MonoBehaviour
{
    [SerializeField] private float scale;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = GetComponentInParent<MeshRenderer>().material.color;
        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 255);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.parent.localScale.x > transform.parent.localScale.y) {
            transform.localScale = new Vector3(1, scale, 1);
        }
        else{
            transform.localScale = new Vector3(scale, 1, 1);

        }

    }
}
