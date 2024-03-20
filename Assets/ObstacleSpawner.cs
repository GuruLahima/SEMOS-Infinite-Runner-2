using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> possibleLongObstacles = new List<GameObject>();
    [SerializeField] private List<GameObject> possibleShortObstacles = new List<GameObject>();
    [SerializeField] private List<Transform> possiblePositions = new List<Transform>();

    [SerializeField] private int initialPoolSize;

    private ObjectPooler bigObstaclesPooler;
    private ObjectPooler smallObstaclesPooler;

    private List<GameObject> bigObstacles = new List<GameObject>();
    private List<GameObject> smallObstacles = new List<GameObject>();

    // Start is called before the first frame update
    void OnEnable()
    {
        bigObstaclesPooler = new ObjectPooler();
        smallObstaclesPooler = new ObjectPooler();
        bigObstaclesPooler.InitializePool(initialPoolSize, possibleLongObstacles);
        smallObstaclesPooler.InitializePool(initialPoolSize, possibleShortObstacles);

        SpawnObstacles();
    }

    private void OnDisable()
    {
        // recycle the generated obstacles too when segment is recycled

        foreach (GameObject obstacle in bigObstacles)
        {
            bigObstaclesPooler.RecycleObject(obstacle);
        }
        foreach (GameObject obstacle in smallObstacles)
        {
            smallObstaclesPooler.RecycleObject(obstacle);
        }
    }



    void SpawnObstacles()
    {
        foreach (Transform position in possiblePositions)
        {
            int randomObstacleType = Random.Range(0, 3);
            switch (randomObstacleType)
            {
                case 0:
                    // spawn nothing
                    break;
                case 1:
                    // spawn a big obstacle
                    GameObject bigObstacle = bigObstaclesPooler.GetObject();
                    bigObstacle.transform.position = position.position;
                    bigObstacle.SetActive(true);
                    bigObstacles.Add(bigObstacle);
                    bigObstacle.transform.parent = this.transform;

                    break;
                case 2:
                    // spawn 1-2 small obstacles
                    if (Random.Range(1, 3) == 1)
                    {
                        GameObject smallObstacle = smallObstaclesPooler.GetObject();
                        int randPos = Random.Range(0, 3);
                        if (randPos == 0)
                        {
                            smallObstacle.transform.position = position.position - Vector3.left * 1.8f;
                        }
                        if (randPos == 1)
                        {
                            smallObstacle.transform.position = position.position;
                        }
                        if (randPos == 2)
                        {
                            smallObstacle.transform.position = position.position + Vector3.left * 1.8f;
                        }
                        smallObstacle.SetActive(true);
                        smallObstacles.Add(smallObstacle);

                        smallObstacle.transform.parent = this.transform;

                    }
                    else
                    {
                        GameObject smallObstacle_1 = smallObstaclesPooler.GetObject();
                        int randPos = Random.Range(0, 3);
                        if (randPos == 0)
                        {
                            smallObstacle_1.transform.position = position.position - Vector3.left * 1.8f;
                        }
                        if (randPos == 1)
                        {
                            smallObstacle_1.transform.position = position.position;
                        }
                        if (randPos == 2)
                        {
                            smallObstacle_1.transform.position = position.position + Vector3.left * 1.8f;
                        }

                        GameObject smallObstacle_2 = smallObstaclesPooler.GetObject();
                        List<int> listOfIndices = new List<int>() { 0, 1, 2 };
                        listOfIndices.RemoveAt(randPos);

                        int randPos_2 = listOfIndices[Random.Range(0, 1)];
                        if (randPos == 0)
                        {
                            smallObstacle_2.transform.position = position.position - Vector3.left * 1.8f;
                        }
                        if (randPos == 1)
                        {
                            smallObstacle_2.transform.position = position.position;
                        }
                        if (randPos == 2)
                        {
                            smallObstacle_2.transform.position = position.position + Vector3.left * 1.8f;
                        }

                        smallObstacle_1.SetActive(true);
                        smallObstacle_2.SetActive(true);

                        smallObstacles.Add(smallObstacle_1);
                        smallObstacles.Add(smallObstacle_2);

                        smallObstacle_1.transform.parent = this.transform;
                        smallObstacle_2.transform.parent = this.transform;

                    }
                    break;
                default:
                    break;
            }

        }


    }
}
