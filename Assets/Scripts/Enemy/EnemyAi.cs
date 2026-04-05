using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    private enum EnemyState { Idle, Chase, Attack, Dying }
    private EnemyState currentState = EnemyState.Idle;

    [Header("Target")]
    public Transform player;

    [Header("AI Settings")]
    public float chaseRange    = 10f;
    public float stopDistance  = 2f;
    public float attackRange   = 2.5f;
    public float rotationSpeed = 5f;

    [Header("Attack Timing")]
    public float attackCooldown = 2.0f;

    [Header("Performance")]
    public float pathUpdateInterval = 0.2f;

    private NavMeshAgent agent;
    private Animator     animator;
    private Health       health;

    private int isRunningHash;
    private int attackHash;
    private int isDyingHash;

    private float lastAttackTime = 0f;
    private float nextPathUpdate = 0f;

    void Awake()
    {
        agent    = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        health   = GetComponent<Health>();

        isRunningHash = Animator.StringToHash("IsRunning");
        attackHash    = Animator.StringToHash("Attack");
        isDyingHash   = Animator.StringToHash("IsDying");

        agent.stoppingDistance = stopDistance;

        if (health != null)
            health.OnDeath += OnEnemyDeath;
    }

    void OnDestroy()
    {
        if (health != null)
            health.OnDeath -= OnEnemyDeath;
    }

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogWarning("[" + gameObject.name + "] Player tidak ditemukan!");
        }

        // Set state awal
        ChangeState(EnemyState.Idle);
    }

    void OnEnemyDeath()
    {
        ChangeState(EnemyState.Dying);
    }

    void Update()
    {
        if (currentState == EnemyState.Dying) return;
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange)
        {
            if (currentState != EnemyState.Attack)
                ChangeState(EnemyState.Attack);
        }
        else if (dist <= chaseRange)
        {
            if (currentState != EnemyState.Chase)
                ChangeState(EnemyState.Chase);
        }
        else
        {
            if (currentState != EnemyState.Idle)
                ChangeState(EnemyState.Idle);
        }

        switch (currentState)
        {
            case EnemyState.Chase:  UpdateChase();  break;
            case EnemyState.Attack: UpdateAttack(); break;
        }
    }

    void ChangeState(EnemyState newState)
    {
        // Cancel invoke sebelumnya
        CancelInvoke(nameof(SetNotRunning));

        // Exit state lama
        switch (currentState)
        {
            case EnemyState.Chase:
                agent.isStopped = true;
                agent.ResetPath();
                break;
            case EnemyState.Attack:
                animator.ResetTrigger(attackHash);
                break;
        }

        currentState = newState;

        // Enter state baru
        switch (currentState)
        {
            case EnemyState.Idle:
                agent.isStopped = true;
                Invoke(nameof(SetNotRunning), 0.15f); // delay kecil
                break;

            case EnemyState.Chase:
                animator.SetBool(isRunningHash, true);
                agent.isStopped = false;
                break;

            case EnemyState.Attack:
                agent.isStopped = true;
                agent.ResetPath();
                Invoke(nameof(SetNotRunning), 0.15f); // delay kecil
                break;

            case EnemyState.Dying:
                animator.SetBool(isRunningHash, false);
                animator.ResetTrigger(attackHash);
                animator.SetTrigger(isDyingHash);
                enabled = false;
                if (agent != null)
                {
                    agent.isStopped = true;
                    agent.enabled   = false;
                }
                break;
        }
    }

    void SetNotRunning()
    {
        animator.SetBool(isRunningHash, false);
    }

    void UpdateChase()
    {
        if (Time.time >= nextPathUpdate)
        {
            nextPathUpdate = Time.time + pathUpdateInterval;
            if (agent.isOnNavMesh)
                agent.SetDestination(player.position);
        }
        RotateTowards(player.position);
    }

    // Tambah variable baru
    private bool comboQueued = false;

    void UpdateAttack()
    {
        RotateTowards(player.position);

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        bool isInAttack1 = state.IsName("Attack1");
        bool isInAttack2 = state.IsName("Attack2");
        bool isInAttack3 = state.IsName("Attack3");
        bool isPlayingAttack = isInAttack1 || isInAttack2 || isInAttack3;

        if (!isPlayingAttack && Time.time >= lastAttackTime + attackCooldown)
        {
            comboQueued = false;
            lastAttackTime = Time.time;
            animator.ResetTrigger(attackHash);
            animator.SetTrigger(attackHash);

            // Aktifkan collider saat attack start
            EnemyAttackCollider hitCol = GetComponentInChildren<EnemyAttackCollider>();
            if (hitCol != null)
                hitCol.StartDamage();
        }
        // Combo transition
        else if (isPlayingAttack)
        {
            float normalizedTime = state.normalizedTime % 1f;

            if (normalizedTime >= 0.6f && normalizedTime <= 0.85f)
            {
                if (!comboQueued)
                {
                    comboQueued = true;
                    animator.ResetTrigger(attackHash);
                    animator.SetTrigger(attackHash);
                }
            }
            else if (normalizedTime < 0.6f)
            {
                comboQueued = false;
            }
        }
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRotation,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    void OnDrawGizmosSelected()
    {
        if (player == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stopDistance);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, player.position);
    }

    // ─── Debug (hapus setelah selesai) ────────────
    void OnGUI()
    {
        if (player == null) return;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        float dist = Vector3.Distance(transform.position, player.position);

        string txt =
            $"EnemyState : {currentState}\n" +
            $"Distance   : {dist:F1}\n" +
            $"IsRunning  : {animator.GetBool(isRunningHash)}\n" +
            $"IsIdle     : {info.IsName("Idle")}\n" +
            $"IsStdRun   : {info.IsName("Standard Run")}\n" +
            $"IsAttack1  : {info.IsName("Attack1")}\n" +
            $"InTransit  : {animator.IsInTransition(0)}";

        GUI.color = Color.black;
        GUI.Label(new Rect(11, 11, 300, 150), txt);
        GUI.color = Color.yellow;
        GUI.Label(new Rect(10, 10, 300, 150), txt);
    }
}