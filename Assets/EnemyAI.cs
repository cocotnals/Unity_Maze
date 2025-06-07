using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    [HideInInspector]
    public Transform target;      
    private Animator anim;

    [Header("공격 설정")]
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = -Mathf.Infinity;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        
        if (target == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }
    }

    void Update()
    {
        if (target == null) return;

        agent.SetDestination(target.position);

     
        float dist = Vector3.Distance(transform.position, target.position);
        if (anim != null && dist <= attackRange && Time.time > lastAttackTime + attackCooldown)
        {
            anim.SetTrigger("Attack");
            lastAttackTime = Time.time;
        }
    }
}
