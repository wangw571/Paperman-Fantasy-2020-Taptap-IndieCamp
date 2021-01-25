using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabHandler : MonoBehaviour
{
    public TouchInputBroadcaster broadcaster;
    private bool IAmGrabbing = false;
    public 玩家控制器 mePC;

    public float ignoranceHeight = 3;
    public float ArmMaxLength = 15;
    public float ClimbForce = 100;

    public bool ConstantForce = true;
    private ConfigurableJoint theJoint;
    private Vector3 rayHitPos;

    public float InkConsumption = 5;

    public float holdTime = 0.5f;

    //public SimpleMotionAnimator motionAnimator;
    private LineRenderer lr;
    private float timing = 0;
    void Start()
    {
        timing = holdTime;
        lr = this.gameObject.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Additive"));

        lr.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {

        if (theJoint != null)
        {
            theJoint.autoConfigureConnectedAnchor = false;
            Vector3 difference = rayHitPos - this.transform.position;
            //motionAnimator.ToPosition = this.transform.parent.InverseTransformPoint(((difference) / 2) + this.transform.position);
            //motionAnimator.ToRotation = new Vector3(0, 0, Mathf.Rad2Deg * (Mathf.Atan2(difference.y, difference.x)));
            //motionAnimator.ToScale = new Vector3(Vector3.Magnitude((rayHitPos - this.transform.position)), 1, 1);
            //motionAnimator.go();

            lr.SetPosition(0, this.transform.position);
            lr.SetPosition(1, rayHitPos);

            if (ConstantForce)
            {
                SoftJointLimit sjl = new SoftJointLimit();
                sjl.limit = difference.magnitude * 0.5f;
                theJoint.linearLimit = sjl;
            }
            
            timing -= Time.deltaTime;
            if (timing <= 0)
            {
                releaseJoint();
            }
        }
        else
        {
            lr.SetPosition(0, this.transform.position);
            lr.SetPosition(1, this.transform.position);
            if (IAmGrabbing) 
            { return; }
            foreach (TouchInputBroadcaster.TouchDetail TIB in broadcaster.hits())
            {
                if (TIB.rayHit == null) { continue; }
                RaycastHit hit = (RaycastHit)TIB.rayHit;
                if (hit.collider.GetComponentInParent<Grabable>())
                {
                    rayHitPos = new Vector3(hit.point.x, hit.point.y, 0);
                    IAmGrabbing = true;
                    Ray downRay = new Ray(new Vector3(hit.point.x, hit.point.y + 150, 0), rayHitPos);
                    foreach (RaycastHit hist in Physics.RaycastAll(downRay))
                    {
                        if (hist.collider == hit.collider && hist.point.y - rayHitPos.y < ignoranceHeight)
                        {
                            rayHitPos = hist.point;
                            break;
                        }
                    }

                    if(Vector3.SqrMagnitude(this.transform.position - rayHitPos) > ArmMaxLength * ArmMaxLength)
                    {
                        IAmGrabbing = false;
                        return;
                    }

                    DoJointJob(hit.transform.gameObject);
                    if (mePC.myRigidbody.velocity.y < 20)
                    {
                        mePC.myRigidbody.velocity += Vector3.up * mePC.BaseUpAcceleration;
                    }
                    mePC.timeController.AddTime(-0.1f * InkConsumption);
                    return;
                }
            }
        }
    }

    public void releaseJoint()
    {
        GameObject.DestroyImmediate(theJoint);
        theJoint = null;
        IAmGrabbing = false;
        timing = holdTime;
        //h.l("Releasing!");
    }

    public void DoJointJob(GameObject other)
    {
        //h.l(other.name);
        theJoint = other.AddComponent<ConfigurableJoint>();
        theJoint.connectedBody = mePC.myRigidbody;
        theJoint.autoConfigureConnectedAnchor = false;
        theJoint.anchor = other.transform.InverseTransformPoint(rayHitPos);
        theJoint.enableCollision = true;
        //theJoint.connectedAnchor = meRigidbody.transform.InverseTransformPoint(this.transform.TransformPoint(Vector3.right * 0.5f));
        theJoint.connectedAnchor = Vector3.zero;
        theJoint.breakForce = Mathf.Infinity;
        theJoint.breakTorque = Mathf.Infinity;
        theJoint.xMotion = ConfigurableJointMotion.Limited;
        theJoint.yMotion = ConfigurableJointMotion.Limited;
        theJoint.zMotion = ConfigurableJointMotion.Limited;
        SoftJointLimit sjl = new SoftJointLimit();
        sjl.limit = 1;
        theJoint.linearLimit = sjl;
        SoftJointLimitSpring sjls = new SoftJointLimitSpring();
        sjls.spring = ClimbForce;
        //sjls.damper = 3;
        theJoint.linearLimitSpring = sjls;
        IAmGrabbing = true;
    }


}
