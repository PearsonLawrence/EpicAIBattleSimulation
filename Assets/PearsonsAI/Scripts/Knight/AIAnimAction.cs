using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIAnimAction : MonoBehaviour {

    public Transform Sword, Sword_Ueq, Sword_Eq;
    public bool sword_is_equipped;
    Animator anim;
    public ParticleSystem left, right;
    public AudioClip[] footSteps;
    public AudioClip swing;
    public AudioClip[] Grunts;
    public AudioSource Audio, Audio2;
    public AudioClip swordDraw, UnEquipSound;

    private void Awake()
    {
        
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    public float emmisionTime;
    private bool emmitter;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            anim.SetTrigger("Sword_i");
        }
       
        if (sword_is_equipped)
        {
            Sword.position = Sword_Eq.position;
            Sword.rotation = Sword_Eq.rotation;
        }
        else
        {
            Sword.position = Sword_Ueq.position;
            Sword.rotation = Sword_Ueq.rotation;
        }
        if(emmitter = true)
        {
            emmisionTime -= Time.deltaTime;
            if(emmisionTime < 0)
            {

                ParticleCast3.emissionRate = 0;
                emmitter = false;
            }
        }
    }

    public void Sword_Equip()
    {
        Audio2.PlayOneShot(swordDraw);
        if (sword_is_equipped == false)
        {
            sword_is_equipped = true;
        }
        else
        {
            sword_is_equipped = false;
        }
    }
    public void Sword_UnEquip()
    {
        Audio2.PlayOneShot(UnEquipSound, .5f);
        if (sword_is_equipped == true)
        {
            sword_is_equipped = false;
        }
    }

    public void FootStepLeft()
    {
        if (left != null)
        {
            left.Emit(100);

            Audio.PlayOneShot(footSteps[Random.Range(0, footSteps.Length)], Audio.volume);
        }
        left.simulationSpace = ParticleSystemSimulationSpace.World;
    }
    public void FootStepRight()
    {
        if (right != null)
        {
            right.Emit(100);

            Audio.PlayOneShot(footSteps[Random.Range(0, footSteps.Length)], Audio.volume);
        }
        right.simulationSpace = ParticleSystemSimulationSpace.World;
    }

    public void FootStepRightLeft()
    {
        if (right != null && left != null)
        {
            right.Emit(100);



            Audio.PlayOneShot(footSteps[Random.Range(0, footSteps.Length)], Audio.volume);
            left.Emit(100);

            Audio.PlayOneShot(footSteps[Random.Range(0, footSteps.Length)], Audio.volume);

        }

        right.simulationSpace = ParticleSystemSimulationSpace.World;
        left.simulationSpace = ParticleSystemSimulationSpace.World;
    }

    public void SwordSwing()
    {

        Audio.PlayOneShot(swing, Audio.volume * 15);
    }
    public ParticleSystem ParticleCast1, ParticleCast2, ParticleCast3;
    public GameObject lightFire;
   

    public void CastIt()
    {

        //ParticleCast1.loop = true;
        //ParticleCast1.Emit(10);
        //ParticleCast2.loop = true;
        //ParticleCast2.Emit(10);
        lightFire.SetActive(true);
       
        ParticleCast1.emissionRate = 15;

        ParticleCast2.emissionRate = 15;
    }
    public void StopCastIt()
    {
        lightFire.SetActive(false);

        ParticleCast1.emissionRate = 0;

        ParticleCast2.emissionRate = 0;

    }
    public GameObject Cast;
    public void FlameCast()
    {
        lightFire.SetActive(false);

        ParticleCast1.emissionRate = 0;

        ParticleCast2.emissionRate = 0;
        GameObject C = Instantiate(Cast, transform.position + new Vector3(0,1f,0), transform.rotation);
        C.GetComponent<FlameSlash>().Target = GetComponent<AIMovement>().DesiredTarget;
       
    }
    public void CastBlock()
    {

        emmitter = true;
        ParticleCast3.emissionRate = 100;
        emmisionTime = 1;

    }
   
}
