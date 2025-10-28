using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;

    [SerializeField] private int waveNumber;
    [SerializeField] private List<Wave> waves;

    [System.Serializable]


    public class Wave
    {
        public GameObject prefab;
        public float spawnTimer;
        public float spawnInterval;
        public int objectsPerWave;
        public int spawnedObjectCount;

    }
    void Update()
    {
        waves[waveNumber].spawnTimer += Time.deltaTime * GameManager.Instance.worldSpeed;
        if (waves[waveNumber].spawnTimer >= waves[waveNumber].spawnInterval)
        {
            SpawnObject();
            waves[waveNumber].spawnTimer = 0;
        }
        if (waves[waveNumber].spawnedObjectCount >= waves[waveNumber].objectsPerWave)
        {
            waves[waveNumber].spawnedObjectCount = 0;
            waveNumber++;
            if (waveNumber >= waves.Count)
            {
                waveNumber = 0;
            }
        }
    }

    private void SpawnObject()
    {
        Instantiate(waves[waveNumber].prefab, RandomSpawnPoint(), transform.rotation, transform);
        waves[waveNumber].spawnedObjectCount++;
    }

    private Vector2 RandomSpawnPoint()
    {
        Vector2 spawnPoint;
        spawnPoint.x = Random.Range(minPos.position.x, maxPos.position.x);
        spawnPoint.y = minPos.position.y;
        return spawnPoint;
    }
}
