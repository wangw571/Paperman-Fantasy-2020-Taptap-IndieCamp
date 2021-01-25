using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WavyAnimator : MonoBehaviour
{
    public Renderer ThisRenderer;
    private Material materal;
    public float wavery = 0.02f;
    public Vector2 Inertia = new Vector2(-0.0005f, 0f);
    public Vector2 MaxInertia = new Vector2(0.3f, 0.3f);
    public bool 也波动主纹理 = false;

    // Start is called before the first frame update
    void Start()
    {
        materal = ThisRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newOffset = this.materal.GetTextureOffset("_BumpMap");
        Inertia += new Vector2(Random.Range(-wavery, wavery), Random.Range(-wavery, wavery));
        Inertia = new Vector2(Mathf.Clamp(Inertia.x, -MaxInertia.x, MaxInertia.x), Mathf.Clamp(Inertia.y, -MaxInertia.y, MaxInertia.y));
        newOffset += Inertia * Time.deltaTime;
        this.materal.SetTextureOffset("_BumpMap", newOffset);
        this.materal.SetTextureOffset("_MainTex", newOffset * (也波动主纹理?1:0));
    }
}
