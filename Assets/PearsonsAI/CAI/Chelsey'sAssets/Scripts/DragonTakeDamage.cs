using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class DragonTakeDamage : MonoBehaviour, IDamageableDragon
{

    public GameObject player;
    private Animator anim;
    public float healthValue = 0;
    public float startHealth;
    public Dragon.MoveDragon AI;
    public NavMeshAgent agent;
    private bool IsAlive = true;
  
    public void TakeDamage(float damageDealt)
    {
      
            anim = GetComponent<Animator>();
            healthValue -= damageDealt;
            

            if (healthValue <= 0)
            {

                IsAlive = false;
                anim.SetTrigger("Die");
                GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);


                AI.isAttacking = false;


                anim.SetBool("IsAlive", false);
                anim.SetBool("IsAttacking", false);


                agent.isStopped = true;
                agent.enabled = false;
                agent.radius = 0;
                AI.Engage = false;
                AI.Wander = false;
                AI.IsAlive = false;

            }
        
    }

    void Start()
    {

        anim = GetComponent<Animator>();
        IsAlive = true;
    }
}
