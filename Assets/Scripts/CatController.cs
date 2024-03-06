using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    [SerializeField] private float threshold;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveDistance;
    [SerializeField] private List<Vector3> positions = new List<Vector3>();

    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        // initialize the possible positions
        positions.Add(transform.position - new Vector3(moveDistance, 0, 0));
        positions.Add(transform.position);
        positions.Add(transform.position + new Vector3(moveDistance, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (!isMoving)
        {

            // moving left
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StartCoroutine(MoveHorizontally());
            }

            // moving right


            // jumping

            // sliding


        }
    }

    IEnumerator MoveHorizontally()
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, positions[0]) > threshold)
        {

            transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);

            yield return null;
        }
    }
}
