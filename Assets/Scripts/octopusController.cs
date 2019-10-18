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
    SpriteRenderer spriteRenderer;
    enum Direction {Left, Right};
    Direction moveDirection = Direction.Right;

    void Awake() {
         LeftBound = GameObject.FindGameObjectWithTag("Left").transform;
         RightBound = GameObject.FindGameObjectWithTag("Right").transform;
    }   

    // Start is called before the first frame update
    void Start()
    {
        //sharks have slightly different speed
        movementSpeed += UnityEngine.Random.value * speedVariationMultiplier;
        //Some random sharks go right
        //the rest go left
        if (UnityEngine.Random.value > 0.5f)
        {
            moveDirection = Direction.Left;
            movementSpeed *= -1;

            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.flipX = false;
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
            movementSpeed *= -1;
            spriteRenderer.flipX = true;
        }
        //Handle Right Warp
        if (transform.position.x > RightBound.position.x && moveDirection == Direction.Right)
        {
            movementSpeed *= -1;
            spriteRenderer.flipX = false;
        }
    }
}
