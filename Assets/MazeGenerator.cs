using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(NavMeshSurface))]
public class MazeGenerator : MonoBehaviour
{

    public int width = 11;
    public int height = 11;


    public Material wallMat;
    public Material floorMat;


    public Text winUIText;
    public TextMeshProUGUI winTMP;
    public Animator playerAnimator;


    public Transform cameraPivot;
    public Camera mainCamera;


    public GameObject enemyPrefab;
    public Transform player;


    public float loseGracePeriod = 4f;



    public float spawnDistance = 5f;

    public float loseDistance = 1.5f;
    public AudioSource bgmSource;


    private NavMeshSurface surface;
    private float wallSize = 3f;
    private GameObject enemyInstance;
    private bool canDetectLose = false;
    private bool isGameOver = false;

    private int[,] Maze;
    private Stack<Vector2> stack = new Stack<Vector2>();
    private List<Vector2> dirs = new List<Vector2>
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
    }

    void Start()
    {
        GenerateMaze();


        surface.collectObjects = CollectObjects.Children;
        surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
        surface.BuildNavMesh();

        SpawnEnemyBehindPlayer();
        Invoke(nameof(EnableLoseDetection), loseGracePeriod);
    }

    void Update()
    {
        if (!canDetectLose || isGameOver || enemyInstance == null || player == null)
            return;

        float dist = Vector3.Distance(player.position, enemyInstance.transform.position);
        if (dist <= loseDistance)
        {
            isGameOver = true;

            if (winUIText != null)
            {
                winUIText.text = "You Lose!";
                winUIText.gameObject.SetActive(true);
            }
            if (winTMP != null)
            {
                winTMP.text = "You Lose!";
                winTMP.gameObject.SetActive(true);
            }
            playerAnimator?.SetTrigger("Lose");
            Time.timeScale = 0f;
        }
    }

    private void EnableLoseDetection()
    {
        canDetectLose = true;
    }

    void GenerateMaze()
    {
        CreateMazeGeometry();
        SetupExitTrigger();
    }

    void SpawnEnemyBehindPlayer()
    {
        // 이미 스폰된 경우 중복 방지
        if (enemyInstance != null)
            return;

        if (enemyPrefab == null || player == null)
            return;

        Vector3 spawnPos = player.position - player.forward * spawnDistance;
        spawnPos.y = 1f;
        if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, wallSize, NavMesh.AllAreas))
            spawnPos = hit.position;


        enemyInstance = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        var ec = enemyInstance.GetComponent<EnemyAI>();
        if (ec != null)
        {
            ec.target = player;
            ec.bgmSource = bgmSource;
        }
    }

    private void CreateMazeGeometry() { /* ... */ }
    private void SetupExitTrigger() { /* ... */ }
}
