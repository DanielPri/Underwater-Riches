using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sharkController : MonoBehaviour
{

    [SerializeField] public float movementSpeed = 1f;
    [SerializeField] float warpOffset = 0.5f;
    [SerializeField] float speedVariationMultiplier = 1f;
    [SerializeField] float lifespan = 25;
    [SerializeField] float maxScaleFactor = 2f;
    Transform LeftBound;
    Transform RightBound;
    enum Direction {Left, Right};
    Direction moveDirection = Direction.Right;
    float spawnTime;
    public Action deathHandler;

    void Awake() {
         LeftBound = GameObject.FindGameObjectWithTag("Left").transform;
         RightBound = GameObject.FindGameObjectWithTag("Right").transform;
    }   

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
        //sharks have slightly different speed
        movementSpeed += UnityEngine.Random.value * speedVariationMultiplier;
        //Some random sharks go right
        //the rest go left
        if (UnityEngine.Random.value > 0.5f)
        {
            moveDirection = Direction.Left;
            movementSpeed *= -1;

            GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
        transform.localScale = transform.localScale * UnityEngine.Random.Range(0.5f,1f) * maxScaleFactor;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().enemySpeedHandler += movementSpeedIncrease;
    }

    private void movementSpeedIncrease(float speedIncrease)
    {
        if(movementSpeed >= 0)
        {
            movementSpeed += speedIncrease;
        }
        else
        {
            movementSpeed -= speedIncrease;
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
            Despawn();
            
        }
        //Handle Right Warp
        if (transform.position.x > RightBound.position.x && moveDirection == Direction.Right)
        {
            transform.position = new Vector2(LeftBound.position.x - warpOffset, transform.position.y);
            Despawn();

        }
    }

    private void Despawn()
    {
        if(lifespan <= Time.time - spawnTime)
        {
            Destroy(gameObject);
            deathHandler();
        }
    }
}
