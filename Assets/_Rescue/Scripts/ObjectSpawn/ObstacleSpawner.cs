using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject startSpawnPos;
    public ObstacleSpawned obstaclePrefab;
    public int poolSize = 10;
    public int numberOfObstacles = 6;
    public float areaWidth = 10f;
    public float areaHeight = 40f;
    public float minDistance = 6f;
    public float maxDistance = 8f;

    private List<ObstacleSpawned> objectPool = new List<ObstacleSpawned>();
    private Vector3 spawnStartPosition => startSpawnPos.transform.position;
    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Start()
    {
        InitializePool();
        SpawnObstacles();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            ResetAllObstacles();
            SpawnObstacles();
        }
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            ObstacleSpawned obj = Instantiate(obstaclePrefab, transform);
            obj.gameObject.SetActive(false);
            objectPool.Add(obj);
        }
    }

    ObstacleSpawned GetPooledObject()
    {
        foreach (ObstacleSpawned obj in objectPool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                return obj;
            }
        }

        ObstacleSpawned newObj = Instantiate(obstaclePrefab);
        newObj.gameObject.SetActive(false);
        objectPool.Add(newObj);
        return newObj;
    }

    void SpawnObstacles()
    {
        int spawnedCount = 0;
        spawnedPositions.Clear();

        Vector3 currentSpawnPosition = spawnStartPosition;

        while (spawnedCount < numberOfObstacles)
        {
            float randomDistance = Random.Range(minDistance, maxDistance);

            currentSpawnPosition.z += randomDistance;

            float randomX = Random.Range(-areaWidth / 2, areaWidth / 2);
            randomX += currentSpawnPosition.x;
            Vector3 randomPosition = new Vector3(randomX, 0, currentSpawnPosition.z);

            if (IsPositionValid(randomPosition))
            {
                ObstacleSpawned obj = GetPooledObject();
                if (obj != null)
                {
                    obj.OnSpwaned(randomPosition);

                    obj.gameObject.SetActive(true);
                    spawnedPositions.Add(randomPosition);
                    spawnedCount++;
                }
            }
        }
    }

    bool IsPositionValid(Vector3 position)
    {
 
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minDistance)
            {
                return false;
            }
        }
        return true;
    }

    public void ResetAllObstacles()
    {
        foreach (ObstacleSpawned obj in objectPool)
        {
            obj.gameObject.SetActive(false);
        }
    }
}
