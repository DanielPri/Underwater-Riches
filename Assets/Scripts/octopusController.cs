using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class octopusController : MonoBehaviour
{

    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float speedVariationMultiplier = 1f;
    Transform LeftBound;
    Transform RightBound;
    [SerializeField] SpriteRenderer spriteRenderer;
    enum Direction {Left, Right};
    Direction moveDirection = Direction.Right;

    // Start is called before the first frame update
    void Start()
    {
        LeftBound = GameObject.FindGameObjectWithTag("Left").transform;
        RightBound = GameObject.FindGameObjectWithTag("Right").transform;
        //octopus... octopi... octopuses? have slightly different speed
        movementSpeed += UnityEngine.Random.value * speedVariationMultiplier;
        //Some random octys go right
        //the rest go left
        if (UnityEngine.Random.value > 0.5f)
        {
            moveDirection = Direction.Left;
            movementSpeed *= -1;

            if (spriteRenderer == null)
            { 
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
            spriteRenderer.flipX = false;
        }
        Debug.Log(moveDirection);
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
            movementSpeed *= -1;
            spriteRenderer.flipX = true;
            moveDirection = Direction.Right;
        }
        //Handle Right Warp
        if (transform.position.x > RightBound.position.x && moveDirection == Direction.Right)
        {
            movementSpeed *= -1;
            spriteRenderer.flipX = false;
            moveDirection = Direction.Left;
        }
    }
}
