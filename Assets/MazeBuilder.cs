using UnityEngine;
using System.Collections.Generic;

public class MazeBuilder : MonoBehaviour
{
    [Header("미로 크기 (홀수 추천)")]
    public int width = 11;
    public int height = 11;
    [Header("머티리얼")]
    public Material wallMat;
    public Material floorMat;

    [Header("벽 한 칸의 크기")]
    public float wallSize = 3f;

    // 생성된 미로 정보 저장
    private int[,] mazeGrid;
    private Stack<Vector2> tileStack = new Stack<Vector2>();
    private List<Vector2> offsets = new List<Vector2>
    {
        new Vector2(0, 1),
        new Vector2(0, -1),
        new Vector2(1, 0),
        new Vector2(-1, 0)
    };
    private System.Random rnd;
    private Vector2 currentTile;

    public int[,] GetMaze() => mazeGrid; // 외부에서 읽기용

    void Start()
    {
        rnd = new System.Random(System.DateTime.Now.Millisecond + UnityEngine.Random.Range(0, 10000));
        BuildMaze();
    }

    public void BuildMaze()
    {
        // 기존 생성된 벽/바닥 오브젝트 모두 삭제
        while (transform.childCount > 0)
        {
#if UNITY_EDITOR
            DestroyImmediate(transform.GetChild(0).gameObject);
#else
            Destroy(transform.GetChild(0).gameObject);
#endif
        }

        // 1. 미로 배열 초기화 (모두 벽)
        mazeGrid = new int[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                mazeGrid[x, y] = 1;

        // 2. 미로 랜덤 생성 (DFS)
        currentTile = Vector2.one;
        tileStack.Clear();
        tileStack.Push(currentTile);
        mazeGrid = CreateMaze();

        // 3. 출구 생성 (가장자리 하단 오른쪽)
        int exitX = width - 2;
        int exitY = height - 1;
        mazeGrid[exitX, exitY] = 0;

        // 4. 실제 벽, 바닥 프리팹 생성
        float wallHeight = 5f;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (mazeGrid[i, j] == 1)
                {
                    GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall.transform.localScale = new Vector3(wallSize, wallHeight, wallSize);
                    wall.transform.position = new Vector3(i * wallSize, wallHeight * 0.5f, j * wallSize);
                    if (wallMat != null)
                        wall.GetComponent<Renderer>().sharedMaterial = wallMat;
                    wall.transform.parent = transform;
                }
                else
                {
                    GameObject floorTile = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    floorTile.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                    floorTile.transform.localScale = new Vector3(wallSize, wallSize, 1f);
                    floorTile.transform.position = new Vector3(i * wallSize, 0f, j * wallSize);
                    if (floorMat != null)
                        floorTile.GetComponent<Renderer>().sharedMaterial = floorMat;
                    floorTile.transform.parent = transform;
                }
            }
        }
    }

    private int[,] CreateMaze()
    {
        List<Vector2> neighbors;
        while (tileStack.Count > 0)
        {
            mazeGrid[(int)currentTile.x, (int)currentTile.y] = 0;
            neighbors = GetValidNeighbors(currentTile);
            if (neighbors.Count > 0)
            {
                tileStack.Push(currentTile);
                currentTile = neighbors[rnd.Next(neighbors.Count)];
            }
            else
            {
                currentTile = tileStack.Pop();
            }
        }
        return mazeGrid;
    }

    private List<Vector2> GetValidNeighbors(Vector2 centerTile)
    {
        List<Vector2> validNeighbors = new List<Vector2>();
        foreach (var offset in offsets)
        {
            Vector2 toCheck = new Vector2(centerTile.x + offset.x, centerTile.y + offset.y);
            if ((toCheck.x % 2 == 1 || toCheck.y % 2 == 1) &&
                mazeGrid[(int)toCheck.x, (int)toCheck.y] == 1 &&
                HasThreeWallsIntact(toCheck))
            {
                validNeighbors.Add(toCheck);
            }
        }
        return validNeighbors;
    }

    private bool HasThreeWallsIntact(Vector2 tileToCheck)
    {
        int intactCount = 0;
        foreach (var off in offsets)
        {
            Vector2 n = new Vector2(tileToCheck.x + off.x, tileToCheck.y + off.y);
            if (IsInside(n) && mazeGrid[(int)n.x, (int)n.y] == 1)
                intactCount++;
        }
        return intactCount == 3;
    }

    private bool IsInside(Vector2 p)
    {
        return p.x >= 0 && p.y >= 0 && p.x < width && p.y < height;
    }
}
