using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBite : MonoBehaviour {


    public Dragon.MoveDragon AI;
    public ParticleSystem Hitter;
   
    private void OnTriggerEnter(Collider other)
    {

        // Debug.Log("SWORD TRIGGER :: " + other.name);
   
            if (other.CompareTag(AI.DesiredTarget) )
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit))
                {
                    ParticleSystem SwordHit = Instantiate(Hitter, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                
                    Hitter.simulationSpace = ParticleSystemSimulationSpace.World;


                    SwordHit.simulationSpace = ParticleSystemSimulationSpace.World;
                }
            if (AI.DesiredClass == "KnightClass")
            {
                other.GetComponent<AITakeDamage>().TakeDamage(50);
            }
            if(AI.DesiredClass == "DragonClass")
            {

                other.GetComponent<DragonTakeDamage>().TakeDamage(50);
            }
            }
        
    }


}
