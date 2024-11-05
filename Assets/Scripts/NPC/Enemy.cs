
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Chasing,

}

public class Enemy : MonoBehaviour,IDamagable
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
        Debug.Log(agent.remainingDistance);
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
        animator.SetBool("Moving", aiState != AIState.Idle);
        animator.SetBool("Chasing", aiState == AIState.Chasing);
        

    }

    private IEnumerator StartEuemy()  // ó�� �ֳʹ� ��ȯ ��� �� ������ ����
    {
        yield return new WaitForSeconds(6f);
        SetState(AIState.Wandering);
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
        animator.SetBool("Attack", false);
        if (playerDistance > detectDistace && agent.remainingDistance <= agent.stoppingDistance)
        {
            WanderingLocation();

        }
        if (playerDistance < detectDistace)
        {
            SetState(AIState.Chasing);

        }

    }
    private void ChasingUpdate() // ��ǥ���� ���󰡴� ���� ��ǥ ���̰Ÿ��� ���� ���� ����
    {
        animator.SetBool("Attack", false);
        agent.SetDestination(CharacterManager.Instance.Player.transform.position + Vector3.up);


        if (agent.remainingDistance > detectDistace)
        {

            SetState(AIState.Wandering);
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {

            SetState(AIState.Attacking);


        }
    }


    private void AttackingUpdate() // ������ �ذ� �ڷ�ƾ���� �ۼ�
    {
        agent.velocity = Vector3.zero;
        // ������ ���� �Ǹ� ������Ʈ�� ������Ѵ�
        StartCoroutine(nameof(Attacking));

        if (playerDistance >= attackDistance)
        {
            StopCoroutine(nameof(Attacking));
            SetState(AIState.Chasing);
        }
    }
    private void WanderingLocation()
    {
        if (playerDistance > detectDistace) // �÷��̾� ��ġ�� ���� ���� �ۿ� �ִٸ�
        {
            agent.SetDestination(GetWanderLocation());
        }


    }
    Vector3 GetWanderLocation()  // ��� ���������� ���� �ִ� ���� ������ �������� �������ִ� �ڵ�
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < detectDistace)  // do while ������ �ٲ㺸�� ��������...
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    private IEnumerator Attacking()  // �÷��̾ ���� ������ ������ ���� ���ݸ��� ���� �ִϸ��̼� 
    {


        animator.SetBool("Attack", true);
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, attackDistance, targetMask))
        {
            hit.collider.GetComponent<IDamagable>().GetDamage(damage);
        }
        yield return new WaitForSeconds(attackRate);
        
    }

    public void GetDamage(float damage) // �÷��̾� ���� �������� �ִ� �ż��� ü���� �ٴڳ��� ���� ���°� �ȴ�
    {
        health -= damage;
        StartCoroutine(nameof(Hurt));
        if (health <= 0)
        {
            StartCoroutine(nameof(Die));
        }

    }

    private IEnumerator Hurt() // �ǰ� ���
    {
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        animator.SetBool("GetDamage", true);
        yield return new WaitForSeconds(3.5f);
        animator.SetBool("GetDamage", false);

    }
    private IEnumerator Die() // ���� �ִϸ��̼� ����� 5�� �� ���� ������Ʈ ����
    {
        animator.SetTrigger("Die");
        agent.ResetPath();
        yield return new WaitForSeconds(5f);
        GameObject.Destroy(gameObject);
    }
}









