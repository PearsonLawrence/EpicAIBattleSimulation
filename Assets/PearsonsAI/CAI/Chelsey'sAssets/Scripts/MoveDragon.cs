using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Dragon
{
    public class MoveDragon : MonoBehaviour
    {
       
        public GameObject Target;
        public GameObject Knight;
        public Transform Left, Right, Back;
        private GameObject BreathTrigger;
        private NavMeshPath LastagentPath;
        public NavMeshAgent agent;
        public Animator anim;



        public bool isDestructible;
        public bool IsFollow = false;
        public bool Wander = false;
        public bool engaged = false;

        public float dt;
        public float timer;
        public int newtarget;
        public float speed;
        public float searchRad;
        public float targetTime;

        public bool isAttacking;
        public bool CanAttack;
        public bool Attack;
        public float attackTime;
        public bool Engage;
        public bool Combat;
        public bool IsAlive = true;
        public bool CanDodge, Dodging, Dodge;
        public float DodgeCooldown;
        public float AttackCooldown = 3;
      
        public float stopTime;
        private float playerDis;
        private float FightDis;
        public bool CanCast, isCasting, Cast;
        public float CastCooldown;
        private bool cacheOnce = true;
		private bool AttackOnce = false;
        public GameObject[] targets;
        public string DesiredClass;
        public string DesiredTarget;
        public float stoptime;
        enum States
        {
            Attacking,
            Defending,
            Casting,
            Moving,
        }
        States currentState;
        void Start()
        {
            flyHeightCombat = 10;
            circleLeft = true;
            PreFightTime = 0;
            agent = GetComponent<NavMeshAgent>();
            anim = GetComponent<Animator>();
            targetTime = 0;
            currentState = States.Moving;
        }
        public void FlyUp()
        {
            flyup = true;
        }

        public void AttackEnd()
        {

            anim.SetBool("Attack", false);
            flyup = false;
            CanAttack = false;
            isAttacking = false;
            agent.isStopped = false;
            currentState = States.Moving;
            AttackCooldown = 20;
            IsLanded = false;
        }


        public void isWander()
        {
            if (timer <= targetTime)
            {
                timer = 3;
                agent.speed = 10;
                agent.acceleration = 12;
                Vector3 temp = Random.insideUnitSphere * searchRad;
                NavMeshHit hit;
                NavMesh.SamplePosition(temp, out hit, searchRad, 1);
                Vector3 randompos = hit.position;
                agent.destination = randompos;
                agent.isStopped = false;

            }
            if (Vector3.Distance(agent.destination, transform.position) <= 1 )
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0)
                {
                    targetTime = 0;
                    timer -= dt;
                    agent.isStopped = true;
                  
                    agent.acceleration = 0;
                }

            }
            playerDis = Vector3.Distance(Target.transform.position, transform.position);

            if (playerDis <= 30)
            {
                engaged = true;
                Wander = false;
            }
        }
      
        void engage()
        {   

                agent.destination = Target.transform.position;
             
                agent.speed = 15;
                agent.acceleration = 17;
                playerDis = Vector3.Distance(Target.transform.position, transform.position);
           
            if (playerDis <= 30)
                {
                Combat = true;
                }
           
        }
    
      
     
       public GameObject EnemyTracker()
        {
            targets = GameObject.FindGameObjectsWithTag(DesiredTarget);
            if (targets == null)
            {
                return null;
            }

            GameObject bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (GameObject potentialTarget in targets)
            {
                if (DesiredClass == "DragonClass")
                {
                    if (potentialTarget.GetComponent<MoveDragon>().IsAlive)
                    {
                        Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                        float dSqrToTarget = directionToTarget.sqrMagnitude;
                        if (dSqrToTarget < closestDistanceSqr)
                        {
                            closestDistanceSqr = dSqrToTarget;
                            bestTarget = potentialTarget;
                        }
                    }
                }
                if (DesiredClass == "KnightClass")
                {
                    if (potentialTarget.GetComponent<AIMovement>().IsAlive)
                    {
                        Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                        float dSqrToTarget = directionToTarget.sqrMagnitude;
                        if (dSqrToTarget < closestDistanceSqr)
                        {
                            closestDistanceSqr = dSqrToTarget;
                            bestTarget = potentialTarget;
                        }
                    }
                }
                

            }
            if (bestTarget == null)
            {

                anim.SetBool("IsAttacking", false);
                anim.SetBool("StrafeLeft", false);
                anim.SetBool("StrafeRight", false);
                anim.SetBool("DodgeLeft", false);
                anim.SetBool("DodgeRight", false);
                anim.SetBool("InCombat", false);
                anim.SetBool("CastBlock", false);
                anim.SetBool("cast", false);
                anim.SetBool("InCombat", false);
                agent.isStopped = true;
                stoptime -= dt;
                anim.SetTrigger("Sword_i");
                if (stoptime < -.5f)
                {

                    agent.isStopped = false;

                    //agent.speed = 1.5f;
                    //agent.acceleration = 2;
                    Engage = false;
                    Combat = false;

                    return null;

                }
            }

            return bestTarget;


        }
		public bool IsLanded = false;
		void doAttack()
		{

			if (CanAttack)
			{
				if (isAttacking == false)
				{

					agent.isStopped = true;
                    agent.speed = 0;
                    agent.acceleration = 0;
                    isAttacking = true;
				}
				stopTime -= dt;
				if (flyup == false)
				{
					flyHeightCombat -= dt * 2;



                    if (flyHeightCombat <= 0 && isAttacking == true && IsLanded == false)
					{

						anim.SetBool("Attack", true);
						IsLanded = true;
						AttackOnce = true;
						stopTime = 2.5f;
					}
					if (stopTime <= 0 && IsLanded == true && AttackOnce == true)
					{
                        
                       if (Vector3.Distance(Target.transform.position, transform.position) <= 15)
                        {

                            agent.isStopped = true;
                            agent.destination = Target.transform.position;
                            agent.speed = 0;
                            agent.acceleration = 0;
                        }
                        else// (Vector3.Distance(transform.position, Target.transform.position) > 10)
                        {

                            agent.isStopped = false;
                            agent.destination = Target.transform.position;
                            agent.speed = 8;
                            agent.acceleration = 12;
                        }

                       // AttackOnce = false;

					}
				}
			}
		}
	
       private bool circleLeft , CircleRight , MoveBack , moveForward, Land;
       private float PreFightTime = 10;
        private float flyHeightCombat;

        void doMove()
        {


            flyHeightCombat += dt * 3;

            PreFightTime -= dt;

            if (Target != null)
            {
                FightDis = Vector3.Distance(Target.transform.position, transform.position);
                /////////////////////////////////////////////////////
                if (PreFightTime <= 0)
                {
                    if (CircleRight == true)
                    {
                        circleLeft = true;
                        CircleRight = false;
                    }
                    else if (circleLeft == true)
                    {

                        circleLeft = false;
                        CircleRight = true;
                    }
                    PreFightTime = Random.Range(15.0f, 20.0f);
                }

                ///////////////////////////////////////////////////////////

                if (CircleRight == true)
                {

                    agent.destination = Left.position;
                    agent.speed = 3;
                    agent.acceleration = 5;
                }
                else if (circleLeft == true)
                {
                    agent.destination = Right.position;
                    agent.speed = 3;
                    agent.acceleration = 5;
                }

                //////////////////////////////////////////////////////////////

                if (FightDis <= 15)
                {
                    MoveBack = true;
                    if (MoveBack == true)
                    {
                        agent.destination = Back.position;
                        agent.speed = 6;
                        agent.acceleration = 8;
                    }
                    else if (FightDis >= 20)
                    {
                        moveForward = true;
                        if (moveForward == true)
                        {
                            agent.destination = Target.transform.position;
                            agent.speed = 4;
                            agent.acceleration = 5;


                        }
                    }

                }
            }

        }
        void doCast()
        {
           BreathTrigger = GameObject.Find("Breath Fire Trigger");
            if(Cast == true )
            {
                anim.SetTrigger("Fire1");
            }

        }
        public GameObject Head;
        private MoveDragon DragonAI;
        private AIMovement KnightAI;
       void watch ()
        {
            if (Target != null)
            {
                if (DesiredClass == "KnightClass")
                {
                    KnightAI = Target.GetComponent<AIMovement>();
                    AttackCooldown -= dt;
                    if (AttackCooldown <= 0 && currentState != States.Attacking && currentState != States.Casting && currentState != States.Defending)
                    {
                        CanAttack = true;
                        currentState = States.Attacking;
                    }
                    else if (currentState != States.Attacking && currentState != States.Casting && currentState != States.Defending)
                    {
                        currentState = States.Moving;
                    }


                }
                 if (DesiredClass == "DragonClass")
                {
                    
                    DragonAI = Target.GetComponent<MoveDragon>();
                    AttackCooldown -= dt;
                    if (AttackCooldown <= 0 && currentState != States.Attacking && currentState != States.Casting && currentState != States.Defending)
                    {
                        CanAttack = true;
                        currentState = States.Attacking;
                    }
                    else if(currentState != States.Attacking && currentState != States.Casting && currentState != States.Defending)
                    {
                        currentState = States.Moving;
                    }

                    
                    

                }
            }

            ////var fwd = transform.forward;
            ////var dir = (Target.transform.position = transform.position).normalized;
            ////var lkat = (Vector3.Slerp(dir, fwd, dt * 5));
            //transform.LookAt(Target.transform.position);
            ////transform.forward = (lkat);
            ////transform.LookAt(lkat + transform.position, Vector3.up);
        }
		void doDefend()
		{
		}
       void CombatMovment()
        {
            stopTime -= dt;
            if (cacheOnce == true)
            {
                anim.SetTrigger("Rawr");
                anim.SetBool("InCombat", true);
                agent.isStopped = true;
                
                stopTime = 3;
                cacheOnce = false;
            }
            if(stopTime <= 0)
            {
                agent.isStopped = false;

                switch (currentState)
                {
                    case States.Moving:
                        doMove();
                        break;
                    case States.Attacking:
                        doAttack();
                        break;
                    case States.Defending:
                        doDefend();
                        break;
                    case States.Casting:
                        doCast();
                        break;

                }

            }
            if (flyup)
            {
                flyHeightCombat += dt * 2;
            }


            watch();
        }
		/// 
		/// /////////////////////
		/// 
        void FixedUpdate()
        {
            if (IsAlive)
            {

            
                dt = Time.deltaTime;
                Target = EnemyTracker();
                anim.SetFloat("IsIdle", agent.velocity.magnitude / 15);

                if (Target != null)
                {
                    //var fwd = transform.forward;
                    //var dir = (Target.transform.position = transform.position).normalized;
                    //var lkat = (Vector3.Slerp(dir, fwd, dt * 5));
                    transform.LookAt(Target.transform.position);
                    //transform.forward = (lkat);
                    //transform.LookAt(lkat + transform.position, Vector3.up);

                }
                if (!engaged)
                {
                    isWander();
                }
                else
                {
                    if (Combat != true)
                    {
                        engage();
                    }
                    else
                    {
                        CombatMovment();
                    }
                }

                flyHeightCombat = Mathf.Clamp(flyHeightCombat, 0, 5.5f);

                agent.baseOffset = flyHeightCombat;

            }
        }
        private bool flyup;
     

    }
}