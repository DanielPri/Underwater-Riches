using System;
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
            if(GetChildWithTag("AirTank") != null)
            {
                Destroy(GetChildWithTag("AirTank"));
            }
            else
            {
                HandleDeath();
            }
        }
    }

    private void HandleDeath()
    {
        Destroy(GetChildWithTag("Player"));
    }

    private void HandleGold(Collider2D collider)
    {
        if (collider.tag == "Gold" && !carrying)
        {
            collider.transform.parent = transform;
            collider.enabled = false;
            carrying = true;
        }
        if (collider.tag == "Ship" && carrying)
        {
            //Debug.Log("EEEEEEEEEEEEEY MACARAEENENA");
            Destroy(GetChildWithTag("Gold"));
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
