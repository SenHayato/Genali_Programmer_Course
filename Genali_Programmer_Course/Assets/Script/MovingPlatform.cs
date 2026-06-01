using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("MovePoint")]
    [SerializeField] GameObject[] movePoints;
    [SerializeField] Transform platform;
    [SerializeField] float moveSpeed;
    [SerializeField] float offSet;
    [SerializeField] int moveIndex = 0;

    void Start()
    {
        
    }

    //Logika ping pong
    void MovePlatform()
    {
        if (movePoints.Length == 0) return;

        Transform target = movePoints[moveIndex].transform;

        platform.position = Vector3.MoveTowards(platform.position, target.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(platform.position, target.position) < offSet)
        {
            moveIndex++;

            if (moveIndex >= movePoints.Length)
            {
                moveIndex = 0; //reset
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        MovePlatform();
    }
}
