using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TouchButtonHandler : MonoBehaviour
{
    public TouchInputBroadcaster broadcaster;
    public Collider MeCollider;
    public float InvokeCoolDown;
    public float ReleaseInvokeCoolDown;
    private float localInvokeCoolDown;
    private float localReleaseInvokeCoolDown;
    public bool IsDown = false;

    public Texture NotPressedTexture;
    public Texture PressedTexture;

    [Serializable]
    public class TouchButtonEvent : UnityEvent { }

    [FormerlySerializedAs("OnTouchDown")]
    [SerializeField]
    private TouchButtonEvent onTouchDown = new TouchButtonEvent();

    [FormerlySerializedAs("OnTouchHold")]
    [SerializeField]
    private TouchButtonEvent onTouchHold = new TouchButtonEvent();

    [FormerlySerializedAs("OnTouchUp")]
    [SerializeField]
    private TouchButtonEvent onTouchUp = new TouchButtonEvent();

    private Renderer thisRenderer;
    void Start()
    {
        thisRenderer = this.gameObject.GetComponent<Renderer>();
        localInvokeCoolDown = 0;
        localReleaseInvokeCoolDown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        localInvokeCoolDown -= Time.deltaTime;
        localReleaseInvokeCoolDown -= Time.deltaTime;
        foreach (TouchInputBroadcaster.TouchDetail TIB in broadcaster.hits())
        {
            if (TIB == null || TIB.rayHit == null) { continue; }
            RaycastHit hit = (RaycastHit)TIB.rayHit;

            if (MeCollider == hit.collider)
            {
                thisRenderer.material.mainTexture = PressedTexture;
                if (localInvokeCoolDown <= 0 && !IsDown)
                {
                    onTouchDown.Invoke();
                    localInvokeCoolDown = InvokeCoolDown;
                    IsDown = true;
                    return;
                }
                IsDown = true;
                if (localInvokeCoolDown <= 0 && IsDown)
                {
                    onTouchHold.Invoke();
                    localInvokeCoolDown = InvokeCoolDown;
                }
                return;
            }
        }
        thisRenderer.material.mainTexture = NotPressedTexture;

        if (IsDown && localReleaseInvokeCoolDown <= 0)
        {
            onTouchUp.Invoke();
            localReleaseInvokeCoolDown = ReleaseInvokeCoolDown;
        }
        IsDown = false;
        
    }

    public TouchButtonEvent OnTouchDown
    {
        get { return onTouchDown; }
        set { onTouchDown = value; }
    }

    public TouchButtonEvent OnTouchHold
    {
        get { return onTouchHold; }
        set { onTouchHold = value; }
    }

    public TouchButtonEvent OnTouchUp
    {
        get { return onTouchUp; }
        set { onTouchUp = value; }
    }
}
