using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject player;
    [SerializeField] PlayerHealth playerScript;
    [SerializeField] float chaseRange = 15f, attackInterval;
    float attackRange;
    NavMeshAgent navMeshAgent;
    float distanceToTarget, attackTimer;
    Animator animator;
    [SerializeField] AnimationClip idleAnim, runAnim, attackAnim;
    [SerializeField] PlayerSO playerSO;
    [SerializeField] ParticleSystem muzzleFlash;
    AudioSource audioSource;
    [SerializeField] AudioClip attackSFX;
    public bool isProvoked;
   
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        attackTimer = attackInterval;
        attackRange = navMeshAgent.stoppingDistance;
    }

    void Update()
    {
        distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= attackRange && attackTimer >= attackInterval)
        {
            Attack();
            attackTimer = 0;
        }
        else if ((distanceToTarget <= chaseRange && distanceToTarget > attackRange) || isProvoked)
        {
            Chase();
        }
        else if (distanceToTarget > chaseRange)
        {
            AnimationController(idleAnim.name);
        }

        if (attackTimer < attackInterval)
            attackTimer += Time.deltaTime;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }

    void AnimationController(string clip)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName(clip))
        {
            animator.Play(clip);
        }
    }

private void Chase()
{
    navMeshAgent.SetDestination(target.position);
    AnimationController(runAnim.name);
}

public void Attack()
{
    AnimationController(attackAnim.name);
    transform.LookAt(player.transform.position);
    if (muzzleFlash) muzzleFlash.Play();
    if (attackSFX && !audioSource.isPlaying)
    {
        audioSource.PlayOneShot(attackSFX);
    }
}
}