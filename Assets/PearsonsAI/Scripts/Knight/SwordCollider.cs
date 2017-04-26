using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollider : MonoBehaviour {


    public AIMovement AI;
    public ParticleSystem Hitter;
    public AudioSource sound;
    private void OnTriggerEnter(Collider other)
    {

       // Debug.Log("SWORD TRIGGER :: " + other.name);
        if (AI.AllowAttackDamage  && AI.anim.GetBool("IsAttacking") == true)
        {
            if(other.CompareTag(AI.TargetName) && other.GetComponent<AIMovement>().Dodging == false)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    ParticleSystem SwordHit = Instantiate(Hitter, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                   
                    sound.Play();
                    Hitter.simulationSpace = ParticleSystemSimulationSpace.World;


                    SwordHit.simulationSpace = ParticleSystemSimulationSpace.World;
                }
                other.GetComponent<AITakeDamage>().TakeDamage(10);
                
            }
        }
    }

    void SwordAttack()
    {
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
