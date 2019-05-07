using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePositionFollowScript : MonoBehaviour
{
    public Transform[] positions;

    public float minDistance = 1.5f;
    public float movementSpeed = 2.0f;
    private int  positionIndex = 0;
    

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(positions[positionIndex]);

        if(Vector3.Distance(transform.position, positions[positionIndex].position) <= minDistance )
        {
            positionIndex++;
            //With this approach the positions have to be placed in a circular manner
            if(positionIndex >= positions.Length)
            {
                positionIndex = 0;
            }
        }

        transform.position += transform.forward * movementSpeed * Time.deltaTime;
    }
}
