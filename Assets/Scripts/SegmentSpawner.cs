using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SegmentSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> possibleSegments = new List<GameObject>();
    [SerializeField] private float segmentSpawnInterval;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Recycler recycler;
    [SerializeField] private int initialPoolSize;

    private ObjectPooler objPooler;

    private float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        objPooler = new ObjectPooler();
        objPooler.InitializePool(initialPoolSize, possibleSegments);

        recycler.objectPooler = objPooler;
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

            GameObject fetchedObject = objPooler.GetObject();
            if (fetchedObject)
            {
                fetchedObject.transform.position = spawnPosition.position;
                fetchedObject.transform.rotation = spawnPosition.rotation;
                fetchedObject.transform.parent = spawnPosition;
                fetchedObject.SetActive(true);
            }

        }
    }
}
