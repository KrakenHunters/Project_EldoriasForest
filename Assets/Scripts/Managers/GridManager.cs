using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField]
    private int gridWidth = 1200;
    [SerializeField]
    private int gridHeight = 600;

    [SerializeField]
    private PlayerController playerPrefab;

    [SerializeField]
    private List<BaseObject> borderPrefabs;

    [SerializeField]
    private List<BaseObject> innerBorderPrefabs;

    [SerializeField]
    private List<BaseObject> centerPrefabs;

    public List<BaseObject> templePrefabs;
    public List<BaseObject> villageTeleportPrefabs;
    public List<BaseObject> enemyPrefabs;
    public BaseObject enemySpotPrefab;

    private BaseObject[,] grid;

    [HideInInspector]
    public Dictionary<int, List<AISpot>> enemySpots = new Dictionary<int, List<AISpot>>();


    // Dictionary to track positions and counts for each type
    private Dictionary<int, Dictionary<string, List<Vector3>>> objectPositions = new Dictionary<int, Dictionary<string, List<Vector3>>>();
    private Dictionary<int, Dictionary<string, int>> maxObjectCounts = new Dictionary<int, Dictionary<string, int>>
    {
        { 1, new Dictionary<string, int> { { "Temple", 3 }, { "VillageTeleport", 1 }, { "EnemySpot", 4 }, { "Enemy", 50 } } },
        { 2, new Dictionary<string, int> { { "Temple", 3 }, { "VillageTeleport", 1 }, { "EnemySpot", 4 }, { "Enemy", 70 } } },
        { 3, new Dictionary<string, int> { { "Temple", 4 }, { "VillageTeleport", 1 }, { "EnemySpot", 4 }, { "Enemy", 80 } } }
    };
    private Dictionary<int, Dictionary<string, int>> minObjectCounts = new Dictionary<int, Dictionary<string, int>>
    {
        { 1, new Dictionary<string, int> { { "Temple", 2 }, { "VillageTeleport", 1 }, { "EnemySpot", 3 }, { "Enemy", 45 } } },
        { 2, new Dictionary<string, int> { { "Temple", 3 }, { "VillageTeleport", 1 }, { "EnemySpot", 3 }, { "Enemy", 60 } } },
        { 3, new Dictionary<string, int> { { "Temple", 3 }, { "VillageTeleport", 1 }, { "EnemySpot", 3 }, { "Enemy", 70 } } },
    };

    [SerializeField]
    private float minDistanceBetweenEnemies = 10f; // Minimum distance between objects

    [SerializeField]
    private float minDistanceBetweenObjects = 70f; // Minimum distance between objects


    [SerializeField]
    private float innerBorderSpawnChance = 0.05f; // 5% chance to spawn an object
    [SerializeField]
    private float centerSpawnChance = 0.01f; // 1% chance to spawn an object

    private int playerSpawnPosY;
    private Vector3 playerSpawnPos;

    public bool gridDone = false;

    void Start()
    {
        playerSpawnPosY = Mathf.RoundToInt(Random.Range(7, gridHeight - 8));
        grid = new BaseObject[gridWidth, gridHeight];
        InitializeObjectPositions();
        PopulateGrid();
        EnsureMinimumObjects();
        gridDone = true;
    }

    void InitializeObjectPositions()
    {
        foreach (var tier in new[] { 1, 2, 3 })
        {
            objectPositions[tier] = new Dictionary<string, List<Vector3>>();
            foreach (var type in new[] { "Temple", "VillageTeleport", "EnemySpot", "Enemy" })
            {
                objectPositions[tier][type] = new List<Vector3>();
            }
        }
    }

    void PopulateGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3(x, 0, y);

                if (x == 7 && y == playerSpawnPosY)
                {
                    Vector3 playerPosition = new Vector3(position.x + 2, position.y, position.z);
                    playerSpawnPos = playerPosition;
                    playerPrefab.gameObject.transform.position = playerPosition;
                    BaseObject spawnedObject = Instantiate(GetPrefabByTag("VillageTeleport"), position, Quaternion.identity);
                    TrackerUIManager.Instance.villages.Add(spawnedObject.transform);
                    spawnedObject.tier = 1;

                    objectPositions[1]["VillageTeleport"].Add(position);

                }

                if (IsBorder(x, y))
                {
                    grid[x, y] = SpawnObject("border", position);
                }
                else if (IsInnerBorder(x, y))
                {
                    if (Random.value <= innerBorderSpawnChance)
                    {
                        grid[x, y] = SpawnObject("inner_border", position);
                    }
                }
                else if (IsCenter(x, y))
                {
                    if (Random.value <= centerSpawnChance)
                    {
                        grid[x, y] = SpawnObject("center", position);
                    }
                }
            }
        }
    }

    void EnsureMinimumObjects()
    {
        foreach (var tier in minObjectCounts.Keys)
        {
            foreach (var kvp in minObjectCounts[tier])
            {
                string objectTag = kvp.Key;
                int minCount = kvp.Value;

                while (GetObjectCount(tier, objectTag) < minCount)
                {
                    Vector3 position = GetRandomPositionInTier(tier);
                    if (CanSpawnBaseObject(tier, objectTag, position))
                    {
                        BaseObject prefabToSpawn = GetPrefabByTag(objectTag);
                        if (prefabToSpawn != null)
                        {
                            BaseObject spawnedObject = Instantiate(prefabToSpawn, position, Quaternion.identity);
                            spawnedObject.tier = tier;
                            objectPositions[tier][objectTag].Add(position);
                        }
                    }
                }
            }
        }
    }

    bool IsBorder(int x, int y)
    {
        return x <= 2 || y <= 2 || x >= gridWidth - 2 || y >= gridHeight - 2;
    }

    bool IsInnerBorder(int x, int y)
    {
        return (x <= 12 || y <= 5 || x >= gridWidth - 5 || y >= gridHeight - 5) && !IsBorder(x, y);
    }

    bool IsCenter(int x, int y)
    {
        return !IsBorder(x, y) && !IsInnerBorder(x, y);
    }

    int GetTier(Vector3 position)
    {
        if (position.x < gridWidth / 3)
        {
            return 1;
        }
        else if (position.x < 2 * gridWidth / 3)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }

    BaseObject SpawnObject(string region, Vector3 position)
    {
        BaseObject prefabToSpawn = null;
        int tier = GetTier(position);

        switch (region)
        {
            case "border":
                prefabToSpawn = GetRandomPrefab(borderPrefabs);
                break;
            case "inner_border":
                prefabToSpawn = GetRandomPrefab(innerBorderPrefabs);
                break;
            case "center":
                prefabToSpawn = GetRandomPrefab(centerPrefabs);
                break;
        }

        if (prefabToSpawn != null)
        {
            string objectTag = prefabToSpawn.tag;
            if (CanSpawnBaseObject(tier, objectTag, position))
            {
                BaseObject spawnedObject = Instantiate(prefabToSpawn, position, Quaternion.identity);
                spawnedObject.tier = tier;
                if (spawnedObject.GetComponent<AISpot>())
                {
                    // Check if the dictionary contains the key
                    if (enemySpots.ContainsKey(tier))
                    {
                        // If the key exists, add the new spot to the existing list
                        enemySpots[tier].Add(spawnedObject.GetComponent<AISpot>());
                    }
                    else
                    {
                        // If the key does not exist, create a new list, add the spot, and add the list to the dictionary
                        List<AISpot> newList = new List<AISpot> { spawnedObject.GetComponent<AISpot>() };
                        enemySpots.Add(tier, newList);
                    }

                }
               if(spawnedObject.CompareTag("VillageTeleport"))
                TrackerUIManager.Instance.villages.Add(spawnedObject.transform);
                return spawnedObject;
            }
        }

        return null;
    }

    bool CanSpawnBaseObject(int tier, string objectTag, Vector3 position)
    {

        if (tier == 1 && objectTag == "VillageTeleport")
        {
            return false;
        }

        if ((Vector3.Distance(position, playerSpawnPos) <= 25f && objectTag == "Enemy") || (Vector3.Distance(position, playerSpawnPos) <= 5f))
        {
            return false;
        }

        if (!objectPositions[tier].ContainsKey(objectTag))//Checking if its a prop
        {
            return true;
        }

        if (objectPositions[tier][objectTag].Count >= maxObjectCounts[tier][objectTag])
        {
            return false;
        }


        foreach (Vector3 existingPosition in objectPositions[tier][objectTag])
        {
            if (Vector3.Distance(position, existingPosition) < minDistanceBetweenObjects && objectTag != "Enemy")
            {
                return false;
            }
            else if (Vector3.Distance(position, existingPosition) < minDistanceBetweenEnemies && objectTag == "Enemy")
            {
                return false;
            }
        }

        objectPositions[tier][objectTag].Add(position);


        return true;
    }


    BaseObject GetRandomPrefab(List<BaseObject> prefabs)
    {
        if (prefabs.Count == 0)
        {
            return null;
        }
        int index = Random.Range(0, prefabs.Count);
        return prefabs[index];
    }

    int GetObjectCount(int tier, string objectTag)
    {
        if (!objectPositions.ContainsKey(tier) || !objectPositions[tier].ContainsKey(objectTag))
        {
            return 0;
        }
        return objectPositions[tier][objectTag].Count;
    }

    Vector3 GetRandomPositionInTier(int tier)
    {
        int xMin, xMax;

        if (tier == 1)
        {
            xMin = 12;
            xMax = gridWidth / 3;
        }
        else if (tier == 2)
        {
            xMin = gridWidth / 3;
            xMax = 2 * gridWidth / 3;
        }
        else
        {
            xMin = 2 * gridWidth / 3;
            xMax = gridWidth;
        }

        int x = Random.Range(xMin, xMax);
        int y = Random.Range(6, gridHeight - 6);
        return new Vector3(x, 0, y);
    }

    BaseObject GetPrefabByTag(string objectTag)
    {
        switch (objectTag)
        {
            case "Temple":
                return GetRandomPrefab(templePrefabs);
            case "VillageTeleport":
                return GetRandomPrefab(villageTeleportPrefabs);
            case "Enemy":
                return GetRandomPrefab(enemyPrefabs);
            case "EnemySpot":
                return enemySpotPrefab;
            default:
                return null;
        }
    }
}
