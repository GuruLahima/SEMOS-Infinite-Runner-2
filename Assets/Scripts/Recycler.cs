using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recycler : MonoBehaviour
{
    public ObjectPooler objectPooler;
    private void OnTriggerEnter(Collider other)
    {
        objectPooler.RecycleObject(other.gameObject);
        // Destroy(other.gameObject);
    }
}
