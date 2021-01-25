using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using System;


public class InkHandler : MonoBehaviour
{
    public float FullAmountOfTime;
    public float AmountOfTime;
    public Vector3 OriginalScale = Vector3.one;
    public Vector3 EmptyScale = Vector3.one * 0.1f;
    public float ParticleEffectScale = 3;
    private Vector3 ScaleIntertia;
    public List<Renderer> thisRenderer;

    public bool 持续激活;
    private bool activated = false;

    [Serializable]
    public class 吸干事件 : UnityEvent { }

    [FormerlySerializedAs("吸干时")]
    [SerializeField]
    private 吸干事件 吸干 = new 吸干事件();

    [FormerlySerializedAs("未吸干时")]
    [SerializeField]
    private 吸干事件 未吸干 = new 吸干事件();


    public 吸干事件 吸干时
    {
        get { return 吸干; }
        set { 吸干 = value; }
    }

    public 吸干事件 未吸干时
    {
        get { return 未吸干; }
        set { 未吸干 = value; }
    }

    void Start()
    {
        //foreach(Renderer renderererer in thisRenderer)
        //{
        //    string shaderName = renderererer.material.shader.name;
        //    Texture textureUsed = renderererer.material.mainTexture;
        //    renderererer.material = new Material(Shader.Find(shaderName));
        //    renderererer.material.mainTexture = textureUsed;
        //}
        this.transform.localScale = OriginalScale;
        ColorReduction();
    }

    // Update is called once per frame
    void Update()
    {
        ScaleIntertia = Vector3.Lerp(EmptyScale, OriginalScale, AmountOfTime / FullAmountOfTime);
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, ScaleIntertia, 0.02f);
        ColorReduction();

        if (持续激活 && activated)
        {
            if (AmountOfTime <= 0)
            {
                吸干.Invoke();
            }
            else
            {
                activated = false;
            }
        }

        if (AmountOfTime <= 0 && !activated)
        {
            吸干.Invoke();
            activated = true;
        }

        if (!activated)
        {
            未吸干.Invoke();
        }
    }

    //public void OnCollisionStay(Collision col)
    //{
    //    if (AmountOfTime <= 0)
    //    {
    //        return;
    //    }
    //    if (col.gameObject.GetComponent<PlayerController>())
    //    {
    //        timeController.AddTime(TimePerAdd);
    //        AmountOfTime -= TimePerAdd;
    //        AmountOfTime = Mathf.Clamp(AmountOfTime, 0, float.MaxValue);
    //        //ColorReduction();
    //    }
    //}

    private void ColorReduction()
    {
        foreach (Renderer renderererer in thisRenderer)
        {
            if (renderererer == null) continue;
            renderererer.material.color = (Color.Lerp(Color.white, Color.black, AmountOfTime / FullAmountOfTime));
        }
    }
}
