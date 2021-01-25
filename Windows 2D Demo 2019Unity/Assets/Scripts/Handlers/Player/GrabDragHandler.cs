using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabDragHandler : MonoBehaviour
{
    public TouchInputBroadcaster broadcaster;
    private bool IAmGrabbing = false;
    public 玩家控制器 mePC;
    public 圆动画器 效果圆动画器;

    public float ignoranceHeight = 3;
    public float ArmMaxLength = 15;
    public float ClimbForce = 20;

    public bool ConstantForce = true;

    public float InkConsumption = 1;

    public float MaximumMultiplier = 100;

    [Range(0.001f, 9999f)]
    public float CoolDown = 0.5f;

    public float LiftForceMultiplier = 2f;

    public Shader ArmShader;
    public Texture ArmTexture;

    private int TouchID = -2;

    //public SimpleMotionAnimator motionAnimator;
    private LineRenderer lr;
    private float CDTiming = 0;

    private Vector3 rayHitPos;
    private Vector2 TouchPosition;
    private Vector2 LaterTouchPosition;

    void Start()
    {
        CDTiming = CoolDown;
        lr = this.gameObject.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.material = new Material(ArmShader);
        lr.material.mainTexture = ArmTexture;
    }

    // Update is called once per frame
    void Update()
    {
        if (IAmGrabbing)
        {
            CDTiming = CoolDown;
            mePC.timeController.AddTime(-0.001f * InkConsumption);
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0.03f, 0.006f);
            lr.SetPosition(0, this.transform.position);
            lr.SetPosition(1, Vector3.Lerp(lr.GetPosition(1), rayHitPos, 0.2f));
            Vector3 newPos = Camera.main.ScreenToWorldPoint(LaterTouchPosition);
            newPos = new Vector3(newPos.x, newPos.y, -1);
            //lr.SetPosition(2, newPos);

            if (broadcaster.hits().Capacity > TouchID && broadcaster.hits()[TouchID] != null)
            {
                LaterTouchPosition = ((TouchInputBroadcaster.TouchDetail)broadcaster.hits()[TouchID]).pos;
                float radius1 = (Camera.main.ScreenToWorldPoint(LaterTouchPosition) - Camera.main.ScreenToWorldPoint(TouchPosition)).magnitude;
                float reducedMM = MaximumMultiplier * 0.1f;
                float radius = reducedMM - (reducedMM / radius1);
                radius = Mathf.Clamp(radius, 0, radius1);
                效果圆动画器.UpdatePosition(new Vector3(rayHitPos.x, rayHitPos.y, -5), radius);
            }
            else
            {
                IAmGrabbing = false;
                Vector2 difference = TouchPosition - LaterTouchPosition;
                float Magnitude = Mathf.Clamp(difference.magnitude, 0, MaximumMultiplier);
                Vector3 direction = (rayHitPos - this.transform.position).normalized;
                if (!ConstantForce)
                {
                    direction *= (rayHitPos - this.transform.position).magnitude * 0.1f;
                }
                direction = new Vector3(direction.x, direction.y * LiftForceMultiplier, 0);
                mePC.myRigidbody.AddForce(direction * Magnitude * ClimbForce );
                //h.l(direction * Magnitude * ClimbForce * 0.01f);
                mePC.timeController.AddTime(-1f * InkConsumption * Magnitude * 0.01f);
                //h.l(Magnitude);
            }
        }
        else
        {
            CDTiming -= Time.deltaTime;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, 0.1f);
            lr.SetPosition(0, this.transform.position);
            lr.SetPosition(1, this.transform.position);
            //lr.SetPosition(2, this.transform.position);
            if (CDTiming < 0)
            {
                效果圆动画器.UpdatePosition(this.transform.position, 0);
            }
            else
            {
                效果圆动画器.UpdatePosition(效果圆动画器.v, Mathf.Lerp(0, 效果圆动画器.R, (CDTiming / CoolDown)));
                //效果圆动画器.UpdatePosition(效果圆动画器.v, 5);
                return;
            }
            if (mePC.InPlaneMode)
            {
                return;
            }
            foreach (TouchInputBroadcaster.TouchDetail TIB in broadcaster.hits())
            {

                if (TIB == null || TIB.rayHit == null) { continue; }
                RaycastHit hit = (RaycastHit)TIB.rayHit;
                if (hit.collider.GetComponentInParent<Grabable>())
                //if (true)
                {
                    rayHitPos = new Vector3(hit.point.x, hit.point.y, 0);
                    IAmGrabbing = true;
                    this.TouchPosition = TIB.pos;
                    this.LaterTouchPosition = this.TouchPosition;
                    this.TouchID = TIB.touchID;

                    Ray downRay = new Ray(new Vector3(hit.point.x, hit.point.y + 150, 0), rayHitPos);
                    foreach (RaycastHit hist in Physics.RaycastAll(downRay))
                    {
                        if (hist.collider == hit.collider && hist.point.y - rayHitPos.y < ignoranceHeight)
                        {
                            rayHitPos = hist.point;
                            break;
                        }
                    }

                    if (Vector3.SqrMagnitude(this.transform.position - rayHitPos) > ArmMaxLength * ArmMaxLength)
                    {
                        IAmGrabbing = false;
                        return;
                    }
                    mePC.timeController.AddTime(-0.1f * InkConsumption);
                    return;
                }
            }

        }
    }

    public void releaseJoint()
    {
        IAmGrabbing = false;
        CDTiming = CoolDown;
        //h.l("Releasing!");
    }



}
