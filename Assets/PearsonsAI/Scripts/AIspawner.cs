using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIspawner : MonoBehaviour {
    public GameObject classKnight, classKnight1, classDragon;
    public string DesiredClass;
    public bool spawn;
    public bool CanUseMagic;
    public int health;
    public string DesiredTarget;
    enum States
    {
       IdleState,
       SpawnState,

    }
    private Vector3 startHeight;
     States currentState;

    void Start ()
    {
        currentState = States.IdleState;

       
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(spawn == true)
        {
            currentState = States.SpawnState;
            spawn = false;
        }
        else
        {
            currentState = States.IdleState;
        }

        switch (currentState)
        {
            case States.IdleState:
                Idle();
                break;
            case States.SpawnState:
                Spawn();
                break;

        }


    }

    public void Idle()
    {

    }
    public void Spawn()
    {   
        if (DesiredClass == "Knight")
        {

            GameObject AIclass = Instantiate(classKnight, transform.position, transform.rotation);
            AIclass.GetComponent<AITakeDamage>().healthValue = health;
            AIclass.GetComponent<AIMovement>().desiredClass = DesiredTarget;
            if(CanUseMagic)
            {    AIclass.GetComponent<AIMovement>().CanBlockCast = true;
                AIclass.GetComponent<AIMovement>().CanCast = true;
            }

        }

        
        if (DesiredClass == "Dragon")
        {
            GameObject AIclass = Instantiate(classDragon, transform.position, transform.rotation);
            AIclass.GetComponent<AITakeDamage>().healthValue = health;

           // AIclass.GetComponent<AIMovement>().desiredClass = DesiredTarget;
            if (CanUseMagic)
            {

                AIclass.GetComponent<AIMovement>().CanBlockCast = true;
                AIclass.GetComponent<AIMovement>().CanCast = true;
            }
        }
        currentState = States.IdleState;

    }

}
