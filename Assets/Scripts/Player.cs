using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float buoyantForce = 1f;
    [SerializeField] float horizontalSpeed = 1f;
    [SerializeField] float moveTimeDelay = 0.5f;

    bool canMove = true;
    float lastTimeMoved = 0f;

    // Start is called before the first frame update
    void Start()
    {
        lastTimeMoved = Time.time;
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
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * buoyantForce);
                lastTimeMoved = Time.time;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        HandleBoundaries(collider);
        HandleGold(collider);
    }

    private void HandleGold(Collider2D collider)
    {
        if (collider.tag == "Gold")
        {
            Debug.Log("EEEEEEEEEEEEEY MACARAEENENA");
            collider.transform.parent = transform;
        }
    }

    private void HandleBoundaries(Collider2D collider)
    {
        if (collider.tag == "Sky")
        {
            canMove = false;
        }
        if (collider.tag == "Left")
        {
            Debug.Log("Player is in left");
            transform.position = new Vector3(8.5f, transform.position.y, 0);
        }
        if (collider.tag == "Right")
        {
            Debug.Log("Player is in right");
            transform.position = new Vector3(-9.2f, transform.position.y, 0);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Sky")
        {
            canMove = true;
        }
        

    }
}
