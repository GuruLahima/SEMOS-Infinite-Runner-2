using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler
{
    private List<GameObject> objectPool = new List<GameObject>();

    /// <summary>
    /// Generates poolSize amount of objects of type objectPrefab 
    /// and disables them.
    /// </summary>
    /// <param name="poolSize"></param>
    /// <param name="objectPrefab"></param>
    public void InitializePool(int poolSize, List<GameObject> possiblePrefabs)
    {
        for (int i = 0; i < poolSize; i++)
        {
            int randIndex = Random.Range(0, possiblePrefabs.Count);
            GameObject tempObj = MonoBehaviour.Instantiate(possiblePrefabs[randIndex]);
            tempObj.SetActive(false);
            objectPool.Add(tempObj);
        }

    }

    public GameObject GetObject()
    {
        if (objectPool.Count > 0)
        {
            int randIndex = Random.Range(0, objectPool.Count);
            GameObject fetchedObject = objectPool[randIndex];
            objectPool.Remove(fetchedObject);
            return fetchedObject;
        }
        else
        {
            return null;
        }
    }

    public void RecycleObject(GameObject obj)
    {
        if (obj)
        {

            objectPool.Add(obj);
            // obj.transform.parent = null;
            obj.SetActive(false);
        }
    }

}
