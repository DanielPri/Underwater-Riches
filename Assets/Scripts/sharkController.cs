using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sharkController : MonoBehaviour
{

    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float warpOffset = 0.5f;
    [SerializeField] Transform LeftBound;
    [SerializeField] Transform RightBound;
    enum Direction {Left, Right};
    Direction moveDirection = Direction.Right;

    // Start is called before the first frame update
    void Start()
    {
        //Some random sharks go right
        //the rest go left
        if(UnityEngine.Random.value > 0.5f)
        {
            moveDirection = Direction.Left;
            movementSpeed *= -1;
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(movementSpeed*Time.deltaTime, 0, 0, Space.World);
        HandleWarp();
    }

    private void HandleWarp()
    {
        //Handle Left Warp
        if(transform.position.x < LeftBound.position.x && moveDirection == Direction.Left)
        {
            transform.position = new Vector2(RightBound.position.x + warpOffset, transform.position.y);
            
        }
        //Handle Right Warp
        if (transform.position.x > RightBound.position.x && moveDirection == Direction.Right)
        {
            transform.position = new Vector2(LeftBound.position.x - warpOffset, transform.position.y);

        }
    }
}
