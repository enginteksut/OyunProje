using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ChaseAndAttack : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 3.5f;
    public Animator animator;

    private NavMeshAgent agent;
    private bool isAttacking = false;
    private float attackDistance = 2.0f;
    private PlayerHp playerHp;

    public float viewDistance = 20f;
    public float viewHeightOffset = 1.5f;

    private float lostSightTimer = 0f;
    private float lostSightThreshold = 1.0f; // Oyuncu 1 saniye boyunca görünmezse idle ol

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = chaseSpeed;

        if (player != null)
            playerHp = player.GetComponent<PlayerHp>();
    }

    void Update()
    {
        if (player == null || agent == null) return;

        bool playerVisible = CanSeePlayer();
        float distance = Vector3.Distance(player.position, transform.position);

        // Oyuncu görünüyorsa zamanlayıcıyı sıfırla
        if (playerVisible)
            lostSightTimer = 0f;
        else
            lostSightTimer += Time.deltaTime;

        // Eğer oyuncu hala görünüyorsa veya çok yeni kaybolduysa
        if (playerVisible || lostSightTimer < lostSightThreshold)
        {
            if (!isAttacking && distance > attackDistance)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);

                SetAnimState(walking: true);
            }
            else if (!isAttacking && distance <= attackDistance)
            {
                agent.ResetPath();
                agent.isStopped = true;

                SetAnimState(attacking: true);
                StartCoroutine(Attack());
            }
            else if (isAttacking)
            {
                agent.isStopped = true;
                RotateTowardsPlayer();

                SetAnimState(); // sadece saldırı animasyonunun kendi bool'u açık
            }
        }
        else
        {
            // Oyuncu uzun süredir görünmüyorsa idle'a geç
            agent.isStopped = true;
            SetAnimState(idle: true);
        }
    }

    void SetAnimState(bool walking = false, bool attacking = false, bool idle = false)
    {
        animator.SetBool("isWalking", walking);
        animator.SetBool("isAttacking", attacking);
        animator.SetBool("isIdle", idle);
    }

    bool CanSeePlayer()
    {
        Vector3 origin = transform.position + Vector3.up * viewHeightOffset;
        Vector3 direction = (player.position - origin).normalized;

        if (Vector3.Distance(transform.position, player.position) > viewDistance)
            return false;

        Ray ray = new Ray(origin, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, viewDistance))
        {
            if (hit.transform == player)
                return true;
        }
        return false;
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        agent.isStopped = true;

        SetAnimState(attacking: true);

        yield return new WaitForSeconds(1.5f); // hasar anı
        DealDamage();

        yield return new WaitForSeconds(1.0f); // animasyon süresi

        animator.SetBool("isAttacking", false);

        yield return new WaitForSeconds(0.5f); // saldırı sonrası bekleme
        isAttacking = false;
    }

    void DealDamage()
    {
        if (playerHp != null)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if (distance <= attackDistance)
            {
                playerHp.TakeDamage(100f);
                Debug.Log("Hasar verildi!");
            }
        }
    }

    void RotateTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }
}
