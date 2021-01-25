using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float SightMaxDistance = 10;
    public float AttackMaxDistance = 1;
    public bool goingRight = true;
    public float ChangeCD = 1f;
    private float tempChangeCD = 1f;
    public float AttackCD = 1f;
    private float tempAttackCD = 1f;

    public float BaseMoveSpeed;

    public float AttackDamage = 30;

    public Rigidbody thisRigidbody;

    public bool IsWaiting;

    private 玩家控制器 playerController;

    // Update is called once per frame
    void FixedUpdate()
    {
        tempChangeCD -= Time.fixedDeltaTime;
        tempAttackCD -= Time.fixedDeltaTime;
        if (tempChangeCD <= 0 && thisRigidbody.velocity.magnitude < 0.5f && !IsWaiting)
        {
            goingRight = !goingRight;
            tempChangeCD = ChangeCD;
        }
        if (!IsWaiting)
        {
            this.thisRigidbody.velocity = new Vector3(BaseMoveSpeed * (goingRight ? 1 : -1), this.thisRigidbody.velocity.y, 0);
        }

        if(playerController != null &&
            new Vector2(
                playerController.transform.position.x - this.transform.position.x, 
                playerController.transform.position.y - this.transform.position.y)
            .sqrMagnitude < AttackMaxDistance * AttackMaxDistance
            && this.tempAttackCD <= 0)
        {
            tempAttackCD = AttackCD;
            playerController.timeController.AddTime(-1 * AttackDamage);
        }

        if(playerController != null)
        {
            Vector3 difference = playerController.transform.position - this.transform.position;
            difference = difference.normalized;
            if (playerController.GetComponent<WeaponTag>())
            {
                this.thisRigidbody.velocity = new Vector3(BaseMoveSpeed * -(difference.x), this.thisRigidbody.velocity.y, 0);
            }
            else
            {
            this.thisRigidbody.velocity = new Vector3(BaseMoveSpeed * (difference.x), this.thisRigidbody.velocity.y, 0);

            }
        }
            detect();
    }

    public void detect()
    {
        玩家控制器 pc = GameObject.FindObjectOfType<玩家控制器>();
        if (pc)
        {
            RaycastHit hit;
            if(Physics.Raycast(this.transform.position, pc.transform.position - this.transform.position, out hit))
            {
                玩家控制器 handler = hit.collider.gameObject.GetComponentInParent<玩家控制器>();
                if (handler != null)
                {
                    playerController = handler;
                    return;
                }
                handler = hit.collider.gameObject.GetComponent<玩家控制器>();
                if (handler != null)
                {
                    playerController = handler;
                    return;
                }
                handler = hit.collider.gameObject.GetComponentInChildren<玩家控制器>();
                if (handler != null)
                {
                    playerController = handler;
                    return;
                }
            }
            playerController = null;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        WeaponTag pc = collision.gameObject.GetComponent<WeaponTag>();
        if (pc)
        {
            Destroy(this.gameObject);
        }
    }
}
