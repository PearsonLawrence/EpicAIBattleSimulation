using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingScript : MonoBehaviour {
    public string BlockArea;
    public AIMovement AI;
    public string targetName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword") && other.GetComponent<SwordCollider>().AI.TargetName == targetName)
        {
            if (other.GetComponent<SwordCollider>().AI.isAttacking)
            {
                if (AI.CanDodge && AI.isAttacking == false)
                {
                    AI.Dodge = true;
                    if (BlockArea == "left" && AI.DodgeRight == false)
                    {
                        AI.DodgeLeft = true;

                        AI.DodgeTimer = .7f;
                    }
                    if (BlockArea == "right" && AI.DodgeLeft == false)
                    {
                        AI.DodgeRight = true;
                        AI.DodgeTimer = .7f;
                    }
                }
            }
        }
    }


}
