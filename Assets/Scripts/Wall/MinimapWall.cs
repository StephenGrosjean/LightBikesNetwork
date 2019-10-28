using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapWall : MonoBehaviour
{
    [SerializeField] private float scale;
    private SpriteRenderer spriteRenderer;
    private MeshRenderer meshRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        meshRenderer = GetComponentInParent<MeshRenderer>();
    }

    void Start()
    {
        spriteRenderer.color = meshRenderer.material.color;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 255);
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
