
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Fleeing
}

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;

    [Header("AI")]
    private AIState aiState;
    public float detectDistace;
    public float safeDistace;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    private float playerDistance;

    

  


    private NavMeshAgent agent;
    private Animator animator;
    private SkinnedMeshRenderer[] skinnedMeshes;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        skinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    private void Start()
    {
        SetState(AIState.Wandering);
    }
    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player. transform.position);
        Debug.Log(playerDistance);
        animator.SetBool("Moving", aiState != AIState.Idle);
        




        switch (aiState)
        {
            case AIState.Idle:
                PassiveUpdate();
                break;
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
            case AIState.Fleeing:
                FleeingUpdate();
                break;
        }
    }


    private void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
            case AIState.Fleeing:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        animator.speed = agent.speed / walkSpeed;
    }

    //private void UpdateTurningAnimation()    // 방향 전환 시점을 좀 더 명확하게 할 필요가 있음 개선 사항
    //{
    //    float currentPositionX = transform.position.x;

    //    if (currentPositionX > previousPositionX)
    //    {
    //        // 오른쪽으로 이동 중
    //        animator.SetBool("TurningRight", true);
           
    //    }
    //    else if (currentPositionX < previousPositionX)
    //    {
    //        // 왼쪽으로 이동 중
    //        animator.SetBool("TurningLeft", true);
           
    //    }
    //    else
    //    {
    //        // 움직이지 않을 때는 방향 모두 false로 설정
    //        animator.SetBool("TurningRight", false);
    //        animator.SetBool("TurningLeft", false);
    //    }

    //    // 이전 위치 업데이트
    //    previousPositionX = currentPositionX;
    //}

    private void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderingLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));

        }
        if (playerDistance < detectDistace)
        {
            SetState(AIState.Attacking);
        }

    }

    void AttackingUpdate()
    {
        if (playerDistance > attackDistance)
        {
            agent.isStopped = false;
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
            {
                agent.SetDestination(CharacterManager.Instance.Player.transform.position);
            }
            else
            {
                SetState(AIState.Fleeing);
            }
        }
        else
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
               // CharacterManager.Instance.Player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage);  데미지 입히는 부분
                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
        }
    }
    private void FleeingUpdate()
    {

    }

    private void WanderingLocation()
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        if (playerDistance > detectDistace)
        {
            agent.SetDestination(GetWanderLocation());
        }
        
      
    }
    Vector3 GetWanderLocation()  // 평소 움직임을 갈수 있는 범위 내에서 랜덤으로 지정해주는 코드
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance,NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistace)  // do while 문으로 바꿔보자
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
            Die();


       
    }

    private void Die()
    {
       Destroy(gameObject);
    }
}
