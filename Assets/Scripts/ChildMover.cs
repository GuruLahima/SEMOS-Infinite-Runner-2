using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This class moves the children of this object in the desired direction and speed
/// </summary>
public class ChildMover : MonoBehaviour
{
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        MoveChildren();
    }

    void MoveChildren()
    {
        foreach (Transform child in transform)
        {
            if (child == transform)
                continue;

            child.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}
