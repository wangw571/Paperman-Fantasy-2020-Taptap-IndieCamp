using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureAnimator : MonoBehaviour
{

    public float scrollSpeed;
    public float countX;
    public float countY;
 
    private float offsetX = 0.0f;
    private float offsetY = 0.0f;
    private float singleTexSize;

    public Renderer thisRenderer;
    private Material thisMaterial;

    void Start()
    {
        Vector2 singleTexSize = new Vector2(1.0f / countX, 1.0f / countY);
        thisMaterial = thisRenderer.material;
        thisMaterial.mainTextureScale = singleTexSize;
    }
    void Update()
    {
        var frame = Mathf.Floor(Time.time * scrollSpeed);
        offsetX = frame / countX;
        offsetY = -(frame - frame % countX) / countY / countX;
        thisMaterial.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }
}
