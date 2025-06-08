using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    [HideInInspector] public Transform target;
    private Animator anim;

    [Header("공격 설정")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = -Mathf.Infinity;
    private MazeGenerator mazeGen;

    [Header("BGM 설정")]
    public AudioSource bgmSource;
    public AudioClip attackBGM;       // 공격 시 BGM
    public bool changeBGMOnAttack = true;
    private float spawnTime;
    public float attackGracePeriod = 5f;
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


            if (changeBGMOnAttack && bgmSource != null && attackBGM != null && Time.time >= spawnTime + attackGracePeriod)
            {
                Debug.Log("공격 트리거 실행됨!");
                bgmSource.clip = attackBGM;
                bgmSource.Play();
                changeBGMOnAttack = false;
                globalAttackBGMPlayed = true;
            }
        }
    }
}
