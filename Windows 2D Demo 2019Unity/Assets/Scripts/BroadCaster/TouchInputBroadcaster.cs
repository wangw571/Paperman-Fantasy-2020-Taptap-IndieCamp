using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchInputBroadcaster : MonoBehaviour
{

    private List<TouchDetail> TouchRaycastHits;

    public class TouchDetail
    {
        public RaycastHit? rayHit;
        public int touchID;
        public Vector2 pos;
        public TouchPhase phase;
        public TouchDetail(RaycastHit? RayHit, int TouchID, Vector2 HitPos, TouchPhase Phase)
        {
            rayHit = RayHit;
            touchID = TouchID;
            pos = HitPos;
            phase = Phase;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        TouchRaycastHits = new List<TouchDetail>();
    }

    // Update is called once per frame
    void Update()
    {
        TouchRaycastHits.Clear();
        TouchRaycastHits.Capacity = 12;
        for(int i = 0; i < TouchRaycastHits.Capacity; ++i)
        {
            TouchRaycastHits.Add(null);
        }
        foreach (Touch touch in Input.touches)
        {

            RaycastHit newHit;
            if (Input.touchCount > 1 && touch.fingerId == 0)
            {

                Vector2 newPos = Vector2.zero;

                foreach (Touch subtouch in Input.touches)
                {
                    newPos += subtouch.fingerId == 0 ? Vector2.zero : (subtouch.position / Input.touchCount);
                }
                newPos = (touch.position - newPos) * Input.touchCount;

                h.GenerateBall(Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 5)), Color.red, true, 1, 1f);
                if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(newPos.x, newPos.y, 0)), out newHit))
                {
                    TouchRaycastHits[touch.fingerId + 1] = (new TouchDetail(newHit, touch.fingerId + 1, newPos, TouchPhase.Moved));
                }
                continue;
            }
            else if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(touch.position.x, touch.position.y, 0)), out newHit))
            {
                h.GenerateBall(Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 5)), Color.green, true, 1, 1f);
                TouchRaycastHits[touch.fingerId + 1] = (new TouchDetail(newHit, touch.fingerId + 1, touch.position, touch.phase));
            }
            else
            {
                h.GenerateBall(Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 5)), Color.blue, true, 1, 1f);
                TouchRaycastHits[touch.fingerId + 1] = (new TouchDetail(null, touch.fingerId + 1, touch.position, touch.phase));
            }
        }
        if (Input.GetMouseButton(0))
        {
            RaycastHit newHit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)), out newHit))
            {
                TouchRaycastHits[0] = (new TouchDetail(newHit, 0, Input.mousePosition, TouchPhase.Began));
            }
            else
            {
                TouchRaycastHits[0] = (new TouchDetail(null, 0, Input.mousePosition, TouchPhase.Moved));
            }
        }
    }
    public List<TouchDetail> hits()
    {
        return TouchRaycastHits;
    }
}
