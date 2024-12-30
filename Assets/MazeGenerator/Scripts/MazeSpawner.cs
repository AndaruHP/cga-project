using UnityEngine;
using UnityEngine.AI;
using System.Collections;

///<summary>
// Game object that creates a maze and instantiates it in the scene
///</summary>
public class MazeSpawner : MonoBehaviour
{
    public enum MazeGenerationAlgorithm
    {
        PureRecursive,
        RecursiveTree,
        RandomTree,
        OldestTree,
        RecursiveDivision,
    }

    public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
    public bool FullRandom = false;
    public int RandomSeed = 12345;
    public GameObject Floor = null;
    public GameObject Wall = null;
    public GameObject Pillar = null;
    public int Rows = 5;
    public int Columns = 5;
    public float CellWidth = 5;
    public float CellHeight = 5;
    public bool AddGaps = true;
    public GameObject GoalPrefab = null;
    public GameObject AmmoPrefab;
    public GameObject BatteryPrefab;
    public int NumberOfAmmo = 8;
    public int NumberOfBatteries = 6;
    public GameObject KeyPrefab;

    private BasicMazeGenerator mMazeGenerator = null;
    private ItemSpawner itemSpawner;
    public GameObject ExitDoorPrefab;

    void Start()
    {
        if (!FullRandom)
        {
            Random.seed = RandomSeed;
        }
        switch (Algorithm)
        {
            case MazeGenerationAlgorithm.PureRecursive:
                mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveTree:
                mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RandomTree:
                mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.OldestTree:
                mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveDivision:
                mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
                break;
        }
        mMazeGenerator.GenerateMaze();

        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                float x = column * (CellWidth + (AddGaps ? .2f : 0));
                float z = row * (CellHeight + (AddGaps ? .2f : 0));
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
                GameObject tmp;

                // Instantiate Floor
                tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.identity) as GameObject;
                tmp.transform.parent = transform;

                // Instantiate Walls based on cell data
                // Inside MazeSpawner.cs, in the Start() method where walls are instantiated:

                // For right wall
                if (cell.WallRight)
                {
                    tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;
                    tmp.name = $"WallPrefab(Clone) ({row},{column}) Right";
                    tmp.transform.parent = transform;
                    // Add NavMeshObstacle
                    AddNavMeshObstacle(tmp);
                }

                // For front wall
                if (cell.WallFront)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.identity) as GameObject;
                    tmp.name = $"WallPrefab(Clone) ({row},{column}) Front";
                    tmp.transform.parent = transform;
                    // Add NavMeshObstacle
                    AddNavMeshObstacle(tmp);
                }

                // For left wall
                if (cell.WallLeft)
                {
                    tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;
                    tmp.name = $"WallPrefab(Clone) ({row},{column}) Left";
                    tmp.transform.parent = transform;
                    // Add NavMeshObstacle
                    AddNavMeshObstacle(tmp);
                }

                // For back wall
                if (cell.WallBack)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;
                    tmp.name = $"WallPrefab(Clone) ({row},{column}) Back";
                    tmp.transform.parent = transform;
                    // Add NavMeshObstacle
                    AddNavMeshObstacle(tmp);
                }

                // TEST 2 COIN
                // // Instantiate Goal if cell is marked as goal
                // if (cell.IsGoal && GoalPrefab != null)
                // {
                //     tmp = Instantiate(GoalPrefab, new Vector3(x, 1, z), Quaternion.identity) as GameObject;
                //     tmp.transform.parent = transform;
                // }
            }
        }

        // Remove outer wall to create exit at top-right corner
        RemoveOuterWalls();

        // Optionally, create a visual exit indicator
        CreateExitIndicator();
        
        // After all maze elements are created
        GetComponent<NavMeshBuilder>()?.RebuildNavMesh();

        // After generating the maze and walls, spawn items
        SpawnItems();

        // Generate Exit
        InstantiateExitDoor();
    }

    /// <summary>
    /// Removes specific outer walls to create exit(s).
    /// </summary>
    /// 

    void InstantiateExitDoor()
    {
        int exitRow = Rows - 1;
        int exitColumn = Columns - 1;

        float x = exitColumn * (CellWidth + (AddGaps ? 0.2f : 0));
        float z = exitRow * (CellHeight + (AddGaps ? 0.2f : 0));

        Vector3 exitPosition = new Vector3(36, 2, 38);

        GameObject exitDoor = Instantiate(ExitDoorPrefab, exitPosition, Quaternion.identity) as GameObject;
        exitDoor.transform.parent = transform;

        exitDoor.transform.Rotate(0, 0, 0);
    }

    private void RemoveOuterWalls()
    {
        // Membuat exit di pojok kanan atas maze
        int exitRow = Rows - 1; // Baris terakhir (atas)
        int exitColumn = Columns - 1; // Kolom terakhir (kanan)

        // Akses sel maze tempat exit akan dibuat
        MazeCell exitCell = mMazeGenerator.GetMazeCell(exitRow, exitColumn);
        exitCell.WallFront = false; // Menghapus tembok depan

        // Membangun nama tembok berdasarkan konvensi penamaan baru
        string exitWallName = $"WallPrefab(Clone) ({exitRow},{exitColumn}) Front";
        GameObject exitWall = GameObject.Find(exitWallName);

        if (exitWall != null)
        {
            Destroy(exitWall);
            Debug.Log($"Exit wall at ({exitRow}, {exitColumn}) Front removed to create exit.");
        }
        else
        {
            Debug.LogWarning($"Exit wall at ({exitRow}, {exitColumn}) Front not found. Check wall naming conventions.");
        }

        // Jika Anda juga ingin menghapus tembok kanan untuk penuh pojok, Anda bisa menambahkan kode berikut:
        /*
        MazeCell exitRightCell = mMazeGenerator.GetMazeCell(exitRow, exitColumn);
        exitRightCell.WallRight = false; // Menghapus tembok kanan

        string exitRightWallName = $"WallPrefab(Clone) ({exitRow},{exitColumn}) Right";
        GameObject exitRightWall = GameObject.Find(exitRightWallName);

        if (exitRightWall != null)
        {
            Destroy(exitRightWall);
            Debug.Log($"Exit wall at ({exitRow}, {exitColumn}) Right removed to create exit.");
        }
        else
        {
            Debug.LogWarning($"Exit wall at ({exitRow}, {exitColumn}) Right not found. Check wall naming conventions.");
        }
        */
    }

    /// <summary>
    /// (Optional) Instantiates a visual indicator for the exit.
    /// </summary>
    private void CreateExitIndicator()
    {
        // TEST 2 COIN
        // // Menempatkan GoalPrefab di posisi exit (pojok kanan atas)
        // int exitRow = Rows - 1; // Baris terakhir (atas)
        // int exitColumn = Columns - 1; // Kolom terakhir (kanan)

        // float x = exitColumn * (CellWidth + (AddGaps ? .2f : 0));
        // float z = exitRow * (CellHeight + (AddGaps ? .2f : 0));

        // if (GoalPrefab != null)
        // {
        //     GameObject exitIndicator = Instantiate(GoalPrefab, new Vector3(x, 1, z), Quaternion.identity) as GameObject;
        //     exitIndicator.transform.parent = transform;
        //     Debug.Log($"Exit indicator placed at ({exitRow}, {exitColumn}).");
        // }
    }

    private void AddNavMeshObstacle(GameObject wall)
    {
        NavMeshObstacle obstacle = wall.AddComponent<NavMeshObstacle>();
        obstacle.carving = true;
        
        // Get the wall's collider size
        Vector3 wallSize = wall.GetComponent<Collider>().bounds.size;
        
        // Adjust the obstacle size based on wall rotation
        float wallRotationY = wall.transform.rotation.eulerAngles.y;
        if (Mathf.Approximately(wallRotationY, 90f) || Mathf.Approximately(wallRotationY, 270f))
        {
            // For walls rotated 90 or 270 degrees (facing along X axis)
            obstacle.size = new Vector3(wallSize.z, wallSize.y, wallSize.x);
        }
        else
        {
            // For walls at 0 or 180 degrees (facing along Z axis)
            obstacle.size = wallSize;
        }
        
        obstacle.center = Vector3.zero;
    }

    private void SpawnItems()
    {
        itemSpawner = gameObject.AddComponent<ItemSpawner>();
        itemSpawner.ammoPrefab = AmmoPrefab;
        itemSpawner.batteryPrefab = BatteryPrefab;
        itemSpawner.keyPrefab = KeyPrefab;
        itemSpawner.numberOfAmmo = NumberOfAmmo;
        itemSpawner.numberOfBatteries = NumberOfBatteries;
        
        itemSpawner.SpawnItems(Rows, Columns, CellWidth, CellHeight, AddGaps);
    }
}