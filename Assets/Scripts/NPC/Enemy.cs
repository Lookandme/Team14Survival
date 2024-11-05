
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
    public float attackDistance;

    private float playerDistance;
     

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
        agent.stoppingDistance = attackDistance;

    }
    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);
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
            yield return new WaitForSeconds(6f);
            SetState(AIState.Wandering);
    }
    private void ChangedAnimation()
    {
        if(aiState == AIState.Wandering)
        {
            animator.SetBool("Moving", true);
        }
        else animator.SetBool("Moving", false);

        if (aiState == AIState.Chasing)
        {
            animator.SetBool("Chasing", true);
        } 
        else animator.SetBool("Chasing", false);

        
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
        
        if (playerDistance > detectDistace && agent.remainingDistance <= agent.stoppingDistance)
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
        
        agent.SetDestination(CharacterManager.Instance.Player.transform.position + Vector3.up);
        

        if (agent.remainingDistance > detectDistace)
        {
            SetState(AIState.Wandering);
        }

        if (agent.remainingDistance <= attackDistance)
        {
            SetState(AIState.Attacking);
           
            
        }
    }


    private void AttackingUpdate() // 문제점 해결 코루틴으로 작성
    {
        agent.velocity = Vector3.zero; // 어택이 시작 되면 에이젼트는 멈춰야한다
        StartCoroutine(nameof(Attacking));
        if (playerDistance > detectDistace)
        {
            StopCoroutine(nameof(Attacking));
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

    private IEnumerator Attacking()  // 플레이어가 공격 범위에 들어오면 공격 간격마다 공격 애니메이션 
    {
        while (playerDistance < attackDistance)
        {
            animator.SetTrigger("Attack");

            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, attackDistance, targetMask))
            {
                hit.collider.GetComponent<IDamagable>().GetDamage(damage);
            }
            yield return new WaitForSeconds(attackRate);
        }


    }

    public void GetDamage(float damage) // 플레이어 한테 데미지를 주는 매서드 체력이 바닥나면 죽음 상태가 된다
    {
        animator.SetTrigger("GetDamage");
        health -= damage;
        if (health <= 0)
            StartCoroutine(nameof(Die));
        
    }
    private IEnumerator Die() // 죽음 애니메이션 재생후 5초 후 게임 오브젝트 삭제
    {
        animator.SetBool("Die", true);
        yield return new WaitForSeconds(5f);
        GameObject.Destroy(gameObject);
    }





}
