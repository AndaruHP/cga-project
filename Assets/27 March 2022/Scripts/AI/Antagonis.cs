using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AntagonisMove
{
    Patrol, Chase, Wait
}

public class Antagonis : MonoBehaviour
{
    public bool canSeePlayer, isAttack, isDeath;
    public ATGData aTGData;
    public Transform[] Patrol;
    public Transform target, PFOV;
    [HideInInspector] public float v_radius;
    public AntagonisMove antagonisMove;
    public LayerMask playerMask, obstructionMask;
    private float rateWaiting, maxWaiting = 5, ratetimeToWait, rateChasing, animationMoveValue;
    private float attackCooldown = 1f;
    private float currentAttackCooldown;
    private NavMeshAgent nma;
    private Animator animator;
    private int patrolIndex;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        patrolIndex = Random.Range(0, Patrol.Length);
        nma = GetComponent<NavMeshAgent>();
        StartCoroutine(FOVRoutine());
    }

    private void Update()
    {
        if (isDeath)
        {
            nma.isStopped = true;
            return;
        }

        if (isAttack)
        {
            nma.isStopped = true;
            return;
        }

        if (currentAttackCooldown > 0)
        {
            currentAttackCooldown -= Time.deltaTime;
        }

        if (animationMoveValue != nma.velocity.magnitude)
            animationMoveValue = Mathf.Lerp(animationMoveValue, nma.velocity.magnitude, 15 * Time.deltaTime);

        animator.SetFloat("Walk", animationMoveValue);
        animator.SetBool("Looking Around", antagonisMove == AntagonisMove.Wait);

        switch (antagonisMove)
        {
            case AntagonisMove.Chase:
                Chasing();
                break;
            case AntagonisMove.Patrol:
                Patroling();
                break;
            case AntagonisMove.Wait:
                Waiting();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (isDeath || isAttack) return;

        switch (antagonisMove)
        {
            case AntagonisMove.Chase:
                if (nma.destination != target.position) nma.destination = target.position;
                break;
            case AntagonisMove.Patrol:
                if (nma.destination != Patrol[patrolIndex].position) nma.destination = Patrol[patrolIndex].position;
                break;
            case AntagonisMove.Wait:
                if (nma.destination != transform.position) nma.destination = transform.position;
                break;
        }
    }

    private void TryAttack()
    {
        if (currentAttackCooldown <= 0 && !isAttack)
        {
            isAttack = true;
            nma.isStopped = true;
            currentAttackCooldown = attackCooldown;
            animator.SetTrigger("AttackTrigger"); // Gunakan trigger untuk memulai animasi serangan

            // Cari pemain di sekitar musuh
            Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, playerMask); // Periksa dalam jangkauan serangan
            if (colliders.Length > 0)
            {
                PlayerHealth playerHealth = colliders[0].GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(10); // Memberikan damage setelah serangan selesai
                }
            }

            StartCoroutine(AttackComplete());
        }
    }

    private IEnumerator AttackComplete()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // Peroleh durasi animasi serangan dengan benar
        float attackDuration = stateInfo.length;

        // Tunggu durasi animasi serangan sebelum melanjutkan
        yield return new WaitForSeconds(attackDuration);

        // Setelah animasi selesai, reset kondisi isAttack
        isAttack = false;
        nma.isStopped = false;
    }

    private void Waiting()
    {
        if (rateWaiting > maxWaiting)
        {
            patrolIndex = Random.Range(0, Patrol.Length);
            SwitchMoveMode("Patrol");
        }
        else
        {
            rateWaiting += Time.deltaTime;
        }
        v_radius = aTGData.radius;
    }

    private void Chasing()
    {
        nma.speed = aTGData.chaseSpeed;
        v_radius = aTGData.radius * 0.65f;

        if (!canSeePlayer)
        {
            if (rateChasing < aTGData.chaseTime)
            {
                rateChasing += Time.deltaTime;
            }
            else
            {
                SwitchMoveMode("Wait");
            }
        }
    }

    private void Patroling()
    {
        nma.speed = aTGData.walkSpeed;
        v_radius = aTGData.radius;
        if (Vector3.Distance(transform.position, new Vector3(Patrol[patrolIndex].position.x, transform.position.y, Patrol[patrolIndex].position.z)) < 0.15f)
        {
            SwitchMoveMode("Wait");
        }

        if (ratetimeToWait > Random.Range(aTGData.timeToWait / 2, aTGData.timeToWait))
        {
            SwitchMoveMode("Wait");
        }
        else
        {
            ratetimeToWait += Time.deltaTime;
        }
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.3f);
        while (!isDeath)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }

    private void FieldOfViewCheck()
    {
        if (isDeath || isAttack) return;

        Collider[] radiusChecks = Physics.OverlapSphere(transform.position, 1f, playerMask);
        if (radiusChecks.Length > 0)
        {
            SwitchMoveMode("Chase");
        }

        Collider[] rangeChecks = Physics.OverlapSphere(PFOV.position, v_radius, playerMask);
        if (rangeChecks.Length > 0)
        {
            target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - PFOV.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < aTGData.angle / 2)
            {
                float distanceToTarget = Vector3.Distance(PFOV.position, target.position);
                if (!Physics.Raycast(PFOV.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    SwitchMoveMode("Chase");
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    private void SwitchMoveMode(string act)
    {
        if (isDeath || isAttack) return;

        if (act == "Chase")
        {
            rateWaiting = ratetimeToWait = rateChasing = 0;
            antagonisMove = AntagonisMove.Chase;
        }
        else if (act == "Wait")
        {
            rateWaiting = ratetimeToWait = rateChasing = 0;
            antagonisMove = AntagonisMove.Wait;
        }
        else if (act == "Patrol")
        {
            rateWaiting = rateChasing = 0;
            antagonisMove = AntagonisMove.Patrol;
        }
    }

    public void OnDeath()
    {
        if (isDeath) return;

        Debug.Log("Soldier has died. Playing Death animation.");

        isDeath = true;
        nma.isStopped = true;
        animator.SetBool("isDeath", true);
        animator.SetBool("isAttack", false);
        animator.SetFloat("Walk", 0f);
        animator.SetBool("Looking Around", false);

        // Memulai coroutine untuk menunggu hingga animasi Death selesai sebelum menghancurkan GameObject
        StartCoroutine(HandleDeath());
    }

    private IEnumerator HandleDeath()
    {
        // Menunggu durasi animasi Death
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float deathDuration = stateInfo.length;

        // Tambahkan buffer waktu jika diperlukan
        yield return new WaitForSeconds(deathDuration + 0.5f);

        Debug.Log("Death animation completed. Destroying GameObject.");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isDeath && !isAttack && currentAttackCooldown <= 0)
        {
            TryAttack();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isDeath)
        {
            if (!isAttack && currentAttackCooldown <= 0)
            {
                TryAttack();
            }
        }
    }
}