﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float buoyantForce = 1f;
    [SerializeField] float horizontalSpeed = 1f;
    [SerializeField] float moveTimeDelay = 0.5f;

    [Header("Physics")]
    [SerializeField] float waterGravity = 0.5f;
    [SerializeField] float waterImpact = 0.5f;

    Rigidbody2D rb;
    bool canMove = true;
    bool carrying = false;
    float lastTimeMoved = 0f;
    GameObject gold;

    //Callback for communication with gameManager
    public Action<int> scoreHandler;
    public Action playerDeath;

    // -----------------------------------------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        lastTimeMoved = Time.time;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = waterGravity;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }
    
    private void MovePlayer()
    {
        float horizontalDisplacement = Input.GetAxis("Horizontal") * horizontalSpeed * Time.deltaTime;
        transform.Translate(horizontalDisplacement, 0, 0);
        if (Time.time - lastTimeMoved > moveTimeDelay) { 
            if (canMove && (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1")))
            {
                rb.AddForce(Vector2.up * buoyantForce, ForceMode2D.Force);
                lastTimeMoved = Time.time;
            }
        }
        transform.rotation = Quaternion.identity;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        HandleGold(collider);
        HandleBoundaries(collider);
        HandleEnemy(collider);
    }

    private void HandleEnemy(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            //if there are remaining air tanks, lose one
            if(GetChildWithTag("AirTank") != null)
            {
                Destroy(GetChildWithTag("AirTank"));
                //check if you die too, but after a delay since Destroy takes few milliseconds
                StartCoroutine(waitAndDie());
            }
        }
    }

    private IEnumerator waitAndDie()
    {
        //wait half a second for air tank to be destroyed before checking if it is destroyed
        yield return new WaitForSeconds(0.1f);
        if (GetChildWithTag("AirTank") == null)
        {
            Destroy(gameObject);
            playerDeath();
        }
    }
    
    private void HandleGold(Collider2D collider)
    {
        if (collider.tag == "Gold" && !carrying)
        {
            collider.transform.parent = transform;
            collider.enabled = false;
            carrying = true;
            gold = GetChildWithTag("Gold");
            Debug.Log(gold.name);
            if (gold.name.Contains("Gold Bar Medium"))
            {
                rb.mass += 1.5f;
            }
            else if (gold.name.Contains("Gold Large"))
            {
                rb.mass += 2f;
            }
            else
            {
                rb.mass += 1;
            }
        }
        if (collider.tag == "Ship" && carrying)
        {
            if(gold.name.Contains("Gold Bar Medium"))
            {
                scoreHandler(2);
            }
            else if (gold.name.Contains("Gold Large"))
            {
                scoreHandler(10);
            }
            else
            {
                scoreHandler(1);
            }
            Destroy(gold);
            rb.mass = 1;
            carrying = false;
        }
    }
    
    private void HandleBoundaries(Collider2D collider)
    {
        if (collider.tag == "Sky")
        {
            //Debug.Log("Player in the sky");
            canMove = false;
            rb.gravityScale = 1f;
        }
        if (collider.tag == "Left")
        {
            //Debug.Log("Player is in left");
            transform.position = new Vector3(8.5f, transform.position.y, 0);
        }
        if (collider.tag == "Right")
        {
            //Debug.Log("Player is in right");
            transform.position = new Vector3(-9.2f, transform.position.y, 0);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Sky")
        {
            canMove = true;
            rb.gravityScale = waterGravity;
            rb.AddForce(Vector2.up * -rb.velocity.y * waterImpact, ForceMode2D.Impulse);
            //Debug.Log(rb.velocity);
        }
    }

    private GameObject GetChildWithTag(string input)
    {
        foreach (Transform child in transform)
        {
            if(child.tag == input)
            {
                return child.gameObject;
            }
        }
        return null;
    }
}
