using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SegmentSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> possibleSegments = new List<GameObject>();
    [SerializeField] private float segmentSpawnInterval;
    [SerializeField] private Transform spawnPosition;

    private float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SpawnSegment();
    }

    void SpawnSegment()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= segmentSpawnInterval)
        {
            spawnTimer = 0;

            // 
            int randIndex = Random.Range(0, possibleSegments.Count);
            Instantiate(possibleSegments[randIndex], spawnPosition.position, spawnPosition.rotation, spawnPosition);
        }
    }
}
