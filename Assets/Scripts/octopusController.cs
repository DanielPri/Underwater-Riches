using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class octopusController : MonoBehaviour
{

    [SerializeField] public float movementSpeed = 1f;
    [SerializeField] float speedVariationMultiplier = 1f;
    [SerializeField] float maxScaleFactor = 2f;
    Transform LeftBound;
    Transform RightBound;
    [SerializeField] SpriteRenderer spriteRenderer;
    enum Direction {Left, Right};
    Direction moveDirection = Direction.Right;
    [SerializeField] float lifespan = 50f;
    float spawnTime;
    public Action deathHandler;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
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
        transform.localScale = transform.localScale * UnityEngine.Random.Range(0.5f, 1f) * maxScaleFactor;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().enemySpeedHandler += movementSpeedIncrease;
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
            Despawn();
        }
        //Handle Right Warp
        if (transform.position.x > RightBound.position.x && moveDirection == Direction.Right)
        {
            movementSpeed *= -1;
            spriteRenderer.flipX = false;
            moveDirection = Direction.Left;
            Despawn();
        }
    }

    private void Despawn()
    {
        if (lifespan <= Time.time - spawnTime)
        {
            Destroy(gameObject);
            deathHandler();
        }
    }

    private void movementSpeedIncrease(float speedIncrease)
    {
        if (movementSpeed >= 0)
        {
            movementSpeed += speedIncrease;
        }
        else
        {
            movementSpeed -= speedIncrease;
        }
    }
}
