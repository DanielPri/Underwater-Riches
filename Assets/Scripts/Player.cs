using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float verticalSpeed = 1;
    [SerializeField] float horizontalSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float horizontalDisplacement = Input.GetAxis("Horizontal") * verticalSpeed * Time.deltaTime;
        float verticalDisplacement = Input.GetAxis("Vertical") * horizontalSpeed * Time.deltaTime;
        transform.Translate(horizontalDisplacement, verticalDisplacement, 0);

    }
}
