using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITakeDamage : MonoBehaviour, IDamageable
{
    public GameObject player;
    private Animator anim;
    public float healthValue = 0;
    public float startHealth;
    public AIMovement AI;
    public NavMeshAgent agent;
    private bool IsAlive = true;
    public AIAnimAction  Act;
    public void TakeDamage(float damageDealt)
    {
        if (AI.Dodging == false)
        {
            Act.Audio2.PlayOneShot(Act.Grunts[Random.Range(0, Act.Grunts.Length)], Act.Audio2.volume );

            anim = GetComponent<Animator>();
            healthValue -= damageDealt;
            anim.SetBool("StrafeLeft", false);
            anim.SetBool("StrafeRight", false);
            anim.SetFloat("Speed", 0);
            anim.SetTrigger("TakeDamage");
            AI.isAttacking = false;
            AI.CanAttack = false;
            anim.SetBool("IsAttacking", false);

            GetComponent<NavMeshAgent>().isStopped = true;
            AI.damageTIme = .7f;


            if (healthValue <= 0)
            {

                IsAlive = false;
                anim.SetTrigger("Die");
                GetComponent<BoxCollider>().size = new Vector3(0, 0, 0);


                AI.isAttacking = false;


                anim.SetBool("IsAlive", false);
                anim.SetBool("IsAttacking", false);


                agent.isStopped = true;
                agent.enabled = true;
                agent.radius = 0;
                AI.Engage = false;
                AI.Wander = false;
                AI.IsAlive = false;

            }
        }
    }
    public void TakeDamageExplosion(float damageDealt, Vector3 center)
    {
        if (AI.CastBlock == false)
        {
            AI.explosionHit = true;
            healthValue -= damageDealt;


            anim = GetComponent<Animator>();

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
    }
    void Start()
    {

        anim = GetComponent<Animator>();
        IsAlive = true;
    }

    void Update()
    { 
    }
}