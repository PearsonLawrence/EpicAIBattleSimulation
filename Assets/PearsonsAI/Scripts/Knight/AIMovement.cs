using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIMovement : MonoBehaviour {
    public GameObject target;
    public GameObject[] targets;
    public Vector3 EngageArea;
    public NavMeshAgent agent;
    public Animator anim;
    public float Wanderer_idleTime;
    private float setWanderIdle;
    public bool Wander;
    public float DT;
    public float walkRadius;
    public bool AtSTop;
    public bool Engage;
    public bool Combat;
    private Rigidbody rb;
    public float speed = .01f;
    public Transform right, left, back;
    private bool circle = true, circleR;
    private float circleTime;
    private Transform circlePos;
    private bool CacheOnce = true;
    private float drawTime;
    private bool retargeting;
    public bool isAttacking;
    public bool CanAttack;
    public bool Attack;
    private int ComboNumb;
    public float attackTime;
    private float runTimeA;
    public string TargetName;
    public bool Dodge, DodgeRight, DodgeLeft, DodgeBack;
    private bool TookDamage;
    private bool isCasting, Cast;
    public bool CanCast;
    public float AttackCooldown = 3;
    public float damageTIme;
    public float DodgeTimer;
    public bool CanDodge, Dodging;
    public string blockstring;
    public bool IsAlive = true;
    public string DesiredTarget;
    public bool CanBlockCast;
    private float[] comboLastExecTime;
    // Use this for initialization
    void Start() {
        setWanderIdle = Wanderer_idleTime;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        anim.SetBool("IsAlive", true);
        rb = GetComponent<Rigidbody>();
        
        comboLastExecTime = new float[9];
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
            if (desiredClass == "KnightClass")
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
            else if (desiredClass == "DragonClass")
            {
                if (potentialTarget.GetComponent<Dragon.MoveDragon>().IsAlive)
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
            //anim.SetFloat("Speed", 0);
            anim.SetBool("StrafeLeft", false);
            anim.SetBool("StrafeRight", false);
            anim.SetBool("DodgeLeft", false);
            anim.SetBool("DodgeRight", false);
            anim.SetBool("InCombat", false);
            anim.SetBool("CastBlock", false);
            anim.SetBool("cast", false);
            anim.SetBool("InCombat", false);
            agent.isStopped = true;
            stoptime -= DT;
            anim.SetTrigger("Sword_i");
            if (stoptime < -.5f)
            {

                agent.isStopped = false;

                agent.speed = 1.5f;
                agent.acceleration = 2;
                Engage = false;
                Combat = false;

                return null;

            }
        }

        return bestTarget;


    }
    private Vector3 quickPos;
    public void Wanderer()
    {

        anim.SetBool("InCombat", false);
        Combat = false;
        CacheOnce = true;
        if (AtSTop == true)
        {
            setWanderIdle -= DT;


            if (setWanderIdle < +0)
            {
                setWanderIdle = Wanderer_idleTime;
                AtSTop = false;
                Wander = true;
                agent.isStopped = false;
            }
        }
        if (Wander == true)
        {


            Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            Vector3 finalPosition = hit.position;
            agent.destination = finalPosition;
            Wander = false;
            AtSTop = false;
            agent.speed = 1.5f;
            agent.acceleration = 2;
            agent.isStopped = false;
        }
        if (!AtSTop)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance || AtSTop == false)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        AtSTop = true;
                    }
                }
            }
        }
        if (target != null)
        {
            if (Vector3.Distance(target.transform.position, transform.position) < 30)
            {
                Engage = true;
            }
        }

    }
    public void Engager()
    {
        agent.destination = target.transform.position;

        ////anim.SetFloat("Speed", 2);
        agent.speed = 8.5f;
        agent.acceleration = 12;
        if (desiredClass == "DragonClass")
        {
            if (Vector3.Distance(transform.position, target.transform.position) < 30)
            {
                Combat = true;
            }
        }
        if (desiredClass == "KnightClass")
        {
            if (Vector3.Distance(transform.position, target.transform.position) < 15)
            {
                Combat = true;
            }
        }

    }
    private bool AttackOnce;

    public string desiredClass;
    public float desiredClassAddTime;
    public void AttackingMovement()
    {
        attackTime -= DT;
        runTimeA -= DT;
        Dodge = false;
        DodgeLeft = false;
        DodgeRight = false;
        float dist = 0;
        if (AttackOnce)
        {
            if(desiredClass == "KnightClass")
            {
                desiredClassAddTime = 0;
                dist = 0;

            }
            else if (desiredClass == "DragonClass")
            {
                desiredClassAddTime = 2;
                dist = 10;
            }

            quickPos = target.transform.position;
            anim.SetBool("StrafeRight", false);
            anim.SetBool("StrafeLeft", false);

            AttackOnce = false;

            isAttacking = true;
            if (Vector3.Distance(transform.position, target.transform.position) < 5 + dist && Vector3.Distance(transform.position, target.transform.position) > 1)
            {
                // evaluate each combo's staleness
                // gen a random number * staleness
                // the highest score wins.
                float maxScore = 0;
                int bestCombo = 0;
                for (int i = 0; i < 9; ++i)
                {
                    float staleness = Time.time - comboLastExecTime[i];
                    float randscore = Random.Range(0.0f, 1.0f);
                    float final = (staleness * randscore);// / Random.Range(1, staleness);
                    if (final > maxScore)
                    {
                        maxScore = final;
                        bestCombo = i;
                    }
                }

                ComboNumb = bestCombo + 1;
                //ComboNumb = Random.Range(1, 9);
                comboLastExecTime[ComboNumb - 1] = Time.time;

            }
            if (ComboNumb == 1)
            {
                agent.speed = 1;
                agent.acceleration = 2.5f;
                attackTime = 4.5f + desiredClassAddTime;
                runTimeA = 6 + desiredClassAddTime;
            }
            else if (ComboNumb == 2)
            {
                attackTime = 4f + desiredClassAddTime;
                runTimeA = 5 + desiredClassAddTime;
                agent.speed = 4;
                agent.acceleration = 5;

            }
            else if (ComboNumb == 3)
            {
                attackTime = 4 + desiredClassAddTime;
                runTimeA = 4.5f + desiredClassAddTime;
                agent.speed = 4;
                agent.acceleration = 5;
            }
            else if (ComboNumb == 4)
            {
                attackTime = 3f + desiredClassAddTime;
                runTimeA = 4 + desiredClassAddTime;
                agent.speed = 1;
                agent.acceleration = 2.5f;
            }
            else if (ComboNumb == 5)
            {
                attackTime = 3 + desiredClassAddTime;
                runTimeA = 4 + desiredClassAddTime;
                agent.speed = 4;
                agent.acceleration = 5;
            }
            else if (ComboNumb == 6)
            {
                attackTime = 3 + desiredClassAddTime;
                runTimeA = 4 + desiredClassAddTime;
                agent.speed = 4;
                agent.acceleration = 5;
            }
            else if (ComboNumb == 7)
            {
                attackTime = 4 + desiredClassAddTime;
                runTimeA = 4 + desiredClassAddTime;
                agent.speed = 1.5f;
                agent.acceleration = 2;
            }
            else if (ComboNumb == 8)
            {
                attackTime = 3 + desiredClassAddTime;
                runTimeA = 4f + desiredClassAddTime;
                agent.speed = 4;
                agent.acceleration = 5;
            }
            else if (ComboNumb == 9)
            {
                attackTime = 3 + desiredClassAddTime;
                runTimeA = 4f + desiredClassAddTime;
                agent.speed = 4;
                agent.acceleration = 5;
            }
        }
        if (isAttacking)
        {


            if (ComboNumb > 0)
            {
                agent.destination = target.transform.position;

                anim.SetInteger("ComboNumber", ComboNumb);
            }
            if (Vector3.Distance(transform.position, target.transform.position) < 2 && runTimeA > 0 || Vector3.Distance(transform.position, target.transform.position) < 3 && runTimeA > 0 && ComboNumb == 6)
            {
                agent.isStopped = true;
            }
            if (attackTime > 0)
            {
                AllowAttackDamage = true;
                anim.SetBool("IsAttacking", true);
            }
            else
            {
                AllowAttackDamage = false;
                anim.SetBool("IsAttacking", false);
                
            }


            if (runTimeA < 0)
            {
                agent.isStopped = false;
                agent.destination = back.position;
                anim.SetBool("StrafeRight", false);
                anim.SetBool("StrafeLeft", false);
                
                anim.SetBool("IsAttacking", false);
                agent.speed = 1.5f;
                agent.acceleration = 3;
                if (Vector3.Distance(transform.position, target.transform.position) > 4)
                {
                    CanAttack = false;
                    AttackCooldown = 5;
                    isAttacking = false;
                    ComboNumb = 0;

                }
            }

        }

    }
    public bool AllowAttackDamage;
    private AIMovement TargetAI;
    private Dragon.MoveDragon TargetAIDrago;
    private float BlockCoolDown;
    public void DodgeMovement()
    {

        DodgeTimer -= DT;

        if (Dodge)
        {


            if (DodgeLeft == true && DodgeLeft == false)
            {
                if (DodgeTimer > 0)
                {
                    agent.isStopped = false;
                    anim.SetBool("DodgeRight", true);
                    agent.destination = right.position;
                    agent.speed = 4;
                    agent.acceleration = 5;
                    DodgeRight = false;
                    Dodging = true;
                    anim.SetBool("Dodging", true);

                }
                else
                {
                    agent.isStopped = true;
                    anim.SetBool("DodgeRight", false);

                }
            }
            if (DodgeRight == true && DodgeLeft == false)
            {
                if (DodgeTimer > 0)
                {
                    DodgeLeft = false;
                    agent.isStopped = false;
                    anim.SetBool("DodgeLeft", true);
                    agent.destination = left.position;
                    agent.speed = 4;
                    agent.acceleration = 5;

                    Dodging = true;
                    anim.SetBool("Dodging", true);
                }
                else
                {

                    agent.isStopped = true;
                    anim.SetBool("DodgeLeft", false);

                }
            }


            if (DodgeTimer < 0)
            {
                agent.speed = 3;
                agent.acceleration = 3;
                Dodge = false;
                // agent.isStopped = false;
                anim.SetBool("DodgeRight", false);
                anim.SetBool("DodgeLeft", false);
                DodgeRight = false;
                DodgeLeft = false;
                Dodging = false;
                BlockCoolDown = 3;
                anim.SetBool("Dodging", false);
                CanDodge = false;
            }
        }




    }
    float stoptime = 3;
    public bool allTargetsDead;
    public int targetsAlive;
    public float ManaLevel;
    public float casttime;
    public void CastMovement()
    {
        if(CanCast)
        {
            isAttacking = false;
            CanAttack = false;
            if (isCasting == false)
            {
                casttime = 4;

                isCasting = true;
                anim.SetBool("cast",true);
            }
                casttime -= DT;
           
                agent.isStopped = true;
                if(casttime < 0)
                {
                    agent.isStopped = false;
                    isCasting = false;
                    Cast = false;

                anim.SetBool("cast", false);
                CastCoolDown = Random.Range(15.0f, 20.0f); ;
                }
            
        }
    }
    private bool isBlockingCast;
    public bool CastBlock;
    public void CastBlockMovement(AIMovement targettemp)
    {
        isAttacking = false;
        CanAttack = false;
        isCasting = false;
        Cast = false;
        if (isBlockingCast == false)
        {
            anim.SetBool("CastBlock",true);

        }
        isBlockingCast = true;
        if(targettemp.Cast == true)
        {
            
            agent.isStopped = true;
            
           
        }
        else
        {
            agent.isStopped = false;
            isBlockingCast = false;
            CastBlock = false;
            CastBlockCoolDown = Random.Range(15,30);
            
            
            anim.SetBool("CastBlock", false);
        }
    }
    public float CastCoolDown;
    public float CastBlockCoolDown;
    public bool explosionHit;
    public bool flyingback;
    private float falltime;
    public void ExplosionHit()
    {
        if(flyingback == false)
        {
            anim.SetTrigger("ExplodeDamage");
            flyingback = false;
            
            falltime = 3;
        }
        flyingback = true;
        falltime -= DT;
       
        if(falltime < 0)
        {
            explosionHit = false;
            flyingback = false;
        }
        
    }
    public void WatchTargetKnight()
    {
        if (target != null)
        {
            if (desiredClass == "KnightClass")
            {
                TargetAI = target.GetComponent<AIMovement>();


                AttackCooldown -= DT;
                BlockCoolDown -= DT;
                CastCoolDown -= DT;
                CastBlockCoolDown -= DT;
                if (TargetAI.IsAlive && target != null)
                {
                    if (TargetAI.isAttacking == false && CanAttack == false && isAttacking == false && !Dodge && AttackCooldown < 0 && TargetAI.Cast == false && Cast == false)
                    {
                        CanAttack = true;
                        AttackOnce = true;
                    }
                    if (CastCoolDown < 0 && TargetAI.isAttacking == false && TargetAI.Cast == false && Cast == false && isCasting == false && CanCast && Vector3.Distance(transform.position, target.transform.position) > 2)
                    {
                        Cast = true;
                    }
                    if (TargetAI.isCasting && Cast == false && isAttacking == false && !Dodge && CastBlockCoolDown < 0 && CanBlockCast == true)
                    {
                        CastBlock = true;
                    }



                    if (BlockCoolDown < 0)
                    {
                        CanDodge = true;
                    }
                    else
                    {
                        CanDodge = false;
                    }

                    if (explosionHit && !CastBlock)
                    {
                        ExplosionHit();
                    }
                    else if (CastBlock && !isCasting)
                    {
                        CastBlockMovement(TargetAI);
                    }
                    else if (Cast)
                    {
                        CastMovement();
                    }
                    else if (CanAttack && !Dodge && damageTIme < 0)
                    {
                        AttackingMovement();
                    }
                    else if (CanDodge && Dodge && BlockCoolDown < 0 && !isAttacking)
                    {
                        DodgeMovement();
                    }
                    else if (!Dodging && !CanAttack)
                    {
                        CircleMovement();
                    }

                }
                else
                {

                }

                if (target != null)
                {
                    int dist = 0;
                    if(desiredClass == "DragonClass")
                    {
                        dist = 30;
                    }
                    else if(desiredClass == "KnightClass")
                    {
                        dist = 0;
                    }

                    if (Vector3.Distance(transform.position, target.transform.position) > 30 + dist)
                    {
                        anim.SetBool("IsAttacking", false);

                        anim.SetBool("StrafeLeft", false);
                        anim.SetBool("StrafeRight", false);
                        anim.SetBool("DodgeLeft", false);
                        anim.SetBool("DodgeRight", false);
                        anim.SetBool("InCombat", false);
                        anim.SetBool("cast", false);
                        Cast = false;
                        isCasting = false;
                        CastBlock = false;
                        anim.SetBool("CastBlock", false);
                        agent.isStopped = true;
                        stoptime -= DT;
                        anim.SetTrigger("Sword_i");
                        if (stoptime < -.5f)
                        {


                            Engage = false;
                            AtSTop = false;
                            Wander = true;

                            Combat = false;
                            agent.speed = 1.5f;
                            agent.acceleration = 2;
                        }

                    }
                }
            }
            else if (desiredClass == "DragonClass")
            {

            //    TargetAI = target.GetComponent<MoveDragon>();


            //    AttackCooldown -= DT;
            //    BlockCoolDown -= DT;
            //    CastCoolDown -= DT;
            //    CastBlockCoolDown -= DT;
            //    if (TargetAI.IsAlive && target != null)
            //    {
            //        if (TargetAI.isAttacking == false && CanAttack == false && isAttacking == false && !Dodge && AttackCooldown < 0 && TargetAI.Cast == false && Cast == false)
            //        {
            //            CanAttack = true;
            //            AttackOnce = true;
            //        }
            //        if (CastCoolDown < 0 && TargetAI.isAttacking == false && TargetAI.Cast == false && Cast == false && isCasting == false && CanCast && Vector3.Distance(transform.position, target.transform.position) > 2)
            //        {
            //            Cast = true;
            //        }
            //        if (TargetAI.isCasting && Cast == false && isAttacking == false && !Dodge && CastBlockCoolDown < 0 && CanBlockCast == true)
            //        {
            //            CastBlock = true;
            //        }



            //        if (BlockCoolDown < 0)
            //        {
            //            CanDodge = true;
            //        }
            //        else
            //        {
            //            CanDodge = false;
            //        }

            //        if (explosionHit && !CastBlock)
            //        {
            //            ExplosionHit();
            //        }
            //        else if (CastBlock && !isCasting)
            //        {
            //            CastBlockMovement(TargetAI);
            //        }
            //        else if (Cast)
            //        {
            //            CastMovement();
            //        }
            //        else if (CanAttack && !Dodge && damageTIme < 0)
            //        {
            //            AttackingMovement();
            //        }
            //        else if (CanDodge && Dodge && BlockCoolDown < 0 && !isAttacking)
            //        {
            //            DodgeMovement();
            //        }
            //        else if (!Dodging && !CanAttack)
            //        {
            //            CircleMovement();
            //        }

            //    }
            //    else
            //    {

            //    }

            //    if (target != null)
            //    {
            //        if (Vector3.Distance(transform.position, target.transform.position) > 30)
            //        {
            //            anim.SetBool("IsAttacking", false);

            //            anim.SetBool("StrafeLeft", false);
            //            anim.SetBool("StrafeRight", false);
            //            anim.SetBool("DodgeLeft", false);
            //            anim.SetBool("DodgeRight", false);
            //            anim.SetBool("InCombat", false);
            //            anim.SetBool("cast", false);
            //            Cast = false;
            //            isCasting = false;
            //            CastBlock = false;
            //            anim.SetBool("CastBlock", false);
            //            agent.isStopped = true;
            //            stoptime -= DT;
            //            anim.SetTrigger("Sword_i");
            //            if (stoptime < -.5f)
            //            {


            //                Engage = false;
            //                AtSTop = false;
            //                Wander = true;

            //                Combat = false;
            //                agent.speed = 1.5f;
            //                agent.acceleration = 2;
            //            }

            //        }
            //    }
            }
        }

    }
    public void WatchTargetDragon()
    {
        if (target != null)
        {
            
            if (desiredClass == "DragonClass")
            {

                TargetAIDrago = target.GetComponent<Dragon.MoveDragon>();
                
                AttackCooldown -= DT;
                BlockCoolDown -= DT;
                CastCoolDown -= DT;
                CastBlockCoolDown -= DT;
                if (TargetAIDrago.IsAlive && target != null)
                {
                    if (TargetAIDrago.IsLanded )
                    {
                        CanAttack = true;
                        AttackOnce = true;
                    }
                    if (CastCoolDown < 0 && TargetAIDrago.isAttacking == false && TargetAIDrago.Cast == false && Cast == false && isCasting == false && CanCast && Vector3.Distance(transform.position, target.transform.position) > 2)
                    {
                        Cast = true;
                    }
                    if (TargetAIDrago.isCasting && Cast == false && isAttacking == false && !Dodge && CastBlockCoolDown < 0 && CanBlockCast == true)
                    {
                        CastBlock = true;
                    }
                    if (BlockCoolDown < 0)
                    {
                        CanDodge = true;
                    }
                    else
                    {
                        CanDodge = false;
                    }

                    if (explosionHit && !CastBlock)
                    {
                        ExplosionHit();
                    }
                    else if (CastBlock && !isCasting)
                    {
                        CastBlockMovement(TargetAI);
                    }
                    else if (Cast)
                    {
                        CastMovement();
                    }
                    else if (CanAttack && !Dodge && damageTIme < 0)
                    {
                        AttackingMovement();
                    }
                    else if (CanDodge && Dodge && BlockCoolDown < 0 && !isAttacking)
                    {
                        DodgeMovement();
                    }
                    else if (!Dodging && !CanAttack)
                    {
                        CircleMovement();
                    }

                }
                else
                {

                }

                if (target != null)
                {
                    if (Vector3.Distance(transform.position, target.transform.position) > 30)
                    {
                        anim.SetBool("IsAttacking", false);

                        anim.SetBool("StrafeLeft", false);
                        anim.SetBool("StrafeRight", false);
                        anim.SetBool("DodgeLeft", false);
                        anim.SetBool("DodgeRight", false);
                        anim.SetBool("InCombat", false);
                        anim.SetBool("cast", false);
                        Cast = false;
                        isCasting = false;
                        CastBlock = false;
                        anim.SetBool("CastBlock", false);
                        agent.isStopped = true;
                        stoptime -= DT;
                        anim.SetTrigger("Sword_i");
                        if (stoptime < -.5f)
                        {


                            Engage = false;
                            AtSTop = false;
                            Wander = true;

                            Combat = false;
                            agent.speed = 1.5f;
                            agent.acceleration = 2;
                        }

                    }
                }
            }
        }
    }
    public void CircleMovement()
    {
        //anim.SetFloat("Speed", 1);
        isAttacking = false;
        if (Vector3.Distance(transform.position, target.transform.position) < 5 && retargeting == false)
        {


            if (circle == true )
            {
                circleTime -= DT;
                if (circleR && damageTIme < 0)
                {
                    
                    circlePos = right;
                    agent.speed = 1.5f;
                    agent.acceleration = 2;
                    anim.SetBool("StrafeRight", true);
                    anim.SetBool("StrafeLeft", false);
                }
                else if (!circleR && damageTIme < 0)
                {
                    //anim.SetFloat("Speed", 1);
                    agent.speed = 1.5f;
                    agent.acceleration = 2;
                    circlePos = left;
                    anim.SetBool("StrafeRight", false);
                    anim.SetBool("StrafeLeft", true);
                }

                if (circleTime < 0)
                {
                    if (circleR)
                    {

                        circleR = false;
                        circleTime = Random.Range(2, 15);
                    }
                    else
                    {
                        circleR = true;
                        circleTime = Random.Range(2, 15);
                    }
                }
                if (damageTIme < 0)
                {
                    agent.isStopped = false;
                    agent.destination = circlePos.position;
                }
            }
        }
        else
        {
            agent.speed = 1.5f;
            agent.acceleration = 2;

            anim.SetBool("StrafeRight", false);
            anim.SetBool("StrafeLeft", false);
            agent.destination = target.transform.position;
            agent.isStopped = false;
            if (Vector3.Distance(transform.position, target.transform.position) > 4.9f)
            {
                retargeting = true;
            }
            else if (Vector3.Distance(transform.position, target.transform.position) < 3f)
            {
                retargeting = false;
            }
        }
    }
    public void CombatMovement()
    {

      
        if (CacheOnce)
        {
            targetsAlive = targets.Length;
            stoptime = 3;
            anim.SetTrigger("Sword_i");
            CacheOnce = false;

            anim.SetBool("InCombat", true);

            agent.isStopped = true;
            drawTime = 2;
           CastCoolDown =  Random.Range(15.0f, 20.0f);
        }

      
        damageTIme -= DT;
       
        if (drawTime < 0)
        {
            if (desiredClass == "KnightClass")
            {
                WatchTargetKnight();
            }
            else if(desiredClass == "DragonClass")
            {
                WatchTargetDragon();
            }
        }
        else
        {
            drawTime -= Time.deltaTime;
        }
    }
    public float TargetTimeSearch;
    void Update ()
    {
        //DT = Time.deltaTime;
        //TargetTimeSearch -= DT;
        if (IsAlive)
        {
            DT = Time.deltaTime;
            target = EnemyTracker();

            if (target != null && !explosionHit)
            {

                var fwd = transform.forward;
                var direct = (target.transform.position-transform.position).normalized;
                var lkat = Vector3.Slerp(fwd, direct, DT * 5);

                
                
                transform.LookAt(lkat + transform.position, Vector3.up);
               
            }


            if (!Engage)
            {
                Wanderer();
            }
            else
            {
                if (!Combat)
                {
                    Engager();
                }
                else
                {
                    CombatMovement();
                }
            }
        }

        // feed information to the animator

        // transform movement into local space
        Vector3 dir = transform.InverseTransformVector(agent.velocity);
        dir.Normalize();
    
        anim.SetFloat("DirX", dir.x);
        anim.SetFloat("DirZ", dir.z);



        anim.SetFloat("Speed", agent.velocity.magnitude / 8.0f);
    }
}
