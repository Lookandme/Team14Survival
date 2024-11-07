
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

public class Enemy : MonoBehaviour, IDamagable
{
    [Header("Stats")]
    public float health;
    public float walkSpeed;
    public float runSpeed;

    [Header("AI")]
    private AIState aiState;
    public float detectDistace;
    public float flashSpeed;
    private Coroutine coroutine;


    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;


    [Header("Combat")]
    public float damage;
    public float attackRate;
    public float lastAttackTime;
    public float attackDistance;

    private float playerDistance;


    private NavMeshAgent agent;
    private Animator animator;
    public LayerMask targetMask;
    private Animation animation;
    PlayerConditions playerConditions;
    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshes;




    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        animation = GetComponentInChildren<Animation>();
        



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
                agent.velocity = Vector3.zero;
                StartCoroutine(nameof(StartEuemy));
                break;
            case AIState.Wandering:
                WanderingUpdate();
                break;
            case AIState.Attacking:
                agent.velocity = Vector3.zero;
                AttackingUpdate();
                break;
            case AIState.Chasing:
                ChasingUpdate();

                break;

        }
        animator.SetBool("Moving", aiState != AIState.Wandering == false);
        animator.SetBool("Chasing", aiState != AIState.Chasing == false);



    }

    private IEnumerator StartEuemy()  // ó�� �ֳʹ� ��ȯ ��� �� ������ ����
    {
        yield return new WaitForSeconds(6f);
        if (playerDistance > detectDistace)
        {
            SetState(AIState.Wandering);
        }
        else SetState(AIState.Chasing);

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
        animator.SetBool("Moving", true);
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
        animator.SetBool("Chasing", true);
        agent.transform.LookAt(CharacterManager.Instance.Player.transform.position);
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


    private void AttackingUpdate() // 
    {
        transform.LookAt( CharacterManager.Instance.Player.transform.position);

        // ������ ���� �Ǹ� ������Ʈ�� ������Ѵ�
        if (Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;

            animator.speed = 1;
            animator.SetTrigger("Attack");
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, attackDistance, targetMask))
            {
                hit.collider.GetComponent<IDamagable>().GetDamage(damage);
            }
        }

        if (playerDistance >= attackDistance)
        {

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



    public void GetDamage(float damage) // �÷��̾� ���� �������� �ִ� �ż��� ü���� �ٴڳ��� ���� ���°� �ȴ�
    {
        health -= damage;
        Hurt();
        if (health <= 0)
        {

            StartCoroutine(nameof(Die));
        }

    }


    private void Hurt() // �ǰ� ���
    {
        SetState(AIState.Idle);
        agent.velocity = Vector3.zero;
        animator.SetTrigger("GetDamage");
        Flash();




    }
    private IEnumerator Die() // ���� �ִϸ��̼� ����� 5�� �� ���� ������Ʈ ����
    {
        aiState = AIState.Idle;
        agent.velocity = Vector3.zero; // �׾����� ��ü�� �����̰��� ���� // ���� �ذ� �ȵ� ���� ���
        animator.SetBool("Die", true);

        yield return new WaitForSeconds(3f);
        GameObject.Destroy(gameObject);
    }

    public void Flash()
    {
        // �̹� �ڷ�ƾ�� ���� ���̸� ����
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        skinnedMeshes.materials[0].color = Color.red;

        coroutine = StartCoroutine(FadeAway());
    }

    private IEnumerator FadeAway()
    {
        Color originalColor = skinnedMeshes.materials[0].color;

        Color targetColor = Color.white;

        float elapsedTime = 0f;
        float duration = flashSpeed; // ���� ���ϴ� �� �ɸ��� �ð�

        while (elapsedTime < duration)
        {
            skinnedMeshes.materials[0].color = Color.Lerp(Color.red, targetColor, elapsedTime / duration);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        skinnedMeshes.materials[0].color = targetColor;
    }
}











