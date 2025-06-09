using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;
    private Animator anim;

  
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = -Mathf.Infinity;
    private MazeGenerator mazeGen;


    public AudioSource bgmSource;
    public AudioClip attackBGM;       // 공격 시 BGM
    public bool chanebgmattack = true;
    private float spawnTime;
    public float attackPer = 5f;
    private static bool globalAttackBGMPlayed = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        spawnTime = Time.time;

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


            if (chanebgmattack && bgmSource != null && attackBGM != null && Time.time >= spawnTime + attackPer)
            {
              
                bgmSource.clip = attackBGM;
                bgmSource.Play();
                chanebgmattack = false;
                globalAttackBGMPlayed = true;
            }
        }
    }
}
