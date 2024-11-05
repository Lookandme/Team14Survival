
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Chasing,

}

public class Enemy : MonoBehaviour, IDamagable
{
    [Header("Stats")]
    public float health;
    public float walkSpeed;
    public float runSpeed;

    [Header("AI")]
    private AIState aiState;
    public float detectDistace;
   

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public float damage;
    public float attackRate;
    private float lastAttackTime = 0f;
    public float attackDistance;

    private float playerDistance;
    private bool isAttacking = false; 

    private NavMeshAgent agent;
    private Animator animator;
    private SkinnedMeshRenderer[] skinnedMeshes;
    public LayerMask targetMask;
    PlayerConditions playerConditions;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        skinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    private void Start()
    {

        StartCoroutine(nameof(StartEuemy));


    }
    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);
        Debug.Log(playerDistance);
        switch (aiState)
        {
            case AIState.Idle:
                StartCoroutine(nameof(StartEuemy));
                break;
            case AIState.Wandering:
                WanderingUpdate();
               
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
            case AIState.Chasing:
                ChasingUpdate();
                
                break;



        }
        ChangedAnimation();

    }

    private IEnumerator StartEuemy()  // 처음 애너미 소환 모션 후 움직임 시작
    {
        if(health != 0)
        {
            
            SetState(AIState.Idle);
           
            yield return new WaitForSeconds(6f);
            SetState(AIState.Wandering);
            Debug.Log("@@@");
        }
        else
        {
            animator.SetBool("Die",true);
            
            
        }
    }
    private void ChangedAnimation()
    {
        if(aiState == AIState.Wandering)
        {
            animator.SetBool("Moving", true);
        }
        else animator.SetBool("Moving", false);

        if (aiState != AIState.Chasing)
        {
            animator.SetBool("Chasing", true);
        } 
        else animator.SetBool("Chasing", false);

        //if (aiState != AIState.Attacking)
        //{
        //    
        //    agent.isStopped = false;
        //}
        //else
        //{
        //    
        //    agent.isStopped = true;
        //}
    }


    private void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                break;
            case AIState.Attacking:
                agent.speed = walkSpeed;
                break;
            case AIState.Chasing:
                agent.speed = runSpeed;
                break;


        }

        animator.speed = agent.speed / walkSpeed;
    }



    private void WanderingUpdate()
    {
        if (playerDistance > detectDistace && agent.remainingDistance <= 0.1f)
        {
            WanderingLocation();

        }
        if (playerDistance < detectDistace)
        {
            SetState(AIState.Chasing);

        }

    }
    private void ChasingUpdate() // 목표물을 따라가는 로직 목표 사이거리에 따라 상태 갱신
    {
        animator.ResetTrigger("Attack");
        agent.SetDestination(CharacterManager.Instance.Player.transform.position + Vector3.up);
        Debug.Log($"destination {agent.destination}");

        if (playerDistance > detectDistace)
        {
            SetState(AIState.Wandering);
        }

        if (playerDistance < attackDistance)
        {
            SetState(AIState.Attacking);
            agent.ResetPath();
            
        }
    }


    private void AttackingUpdate() // 문제점 공격에 딜레이가 없는상태 수정 필요 !!
    {
       
        if (Time.time - lastAttackTime >= attackRate)   // 공격을 수행할 수 있는지 체크
        {
            agent.ResetPath();
            animator.SetTrigger("Attack");

            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, attackDistance, targetMask))
            {
                hit.collider.GetComponent<IDamagable>().GetDamage(damage);
                
            }

            
            lastAttackTime = Time.time;  // 마지막 공격 시간 업데이트
        }
        if(playerDistance > attackDistance) // 공격 범위를 벗어났다면?
        {
            SetState(AIState.Chasing);
        }
    }
    private void WanderingLocation()
    {
        if (playerDistance > detectDistace) // 플레이어 위치가 감지 범위 밖에 있다면
        {
            agent.SetDestination(GetWanderLocation());
        }


    }
    Vector3 GetWanderLocation()  // 평소 움직임으로 갈수 있는 범위 내에서 랜덤으로 지정해주는 코드
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistace)  // do while 문으로 바꿔보자 언젠가는...
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    public void GetDamage(float damage) // 플레이어 한테 데미지를 주는 매서드
    {
        animator.SetTrigger("GetDamage");
        health -= damage;
        if (health <= 0)
            Die();

    }


    


    private void Die()
    {
        SetState(AIState.Idle);

        

       GameObject.Destroy(gameObject);

    }
}
