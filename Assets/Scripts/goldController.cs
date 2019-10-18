using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldController : MonoBehaviour
{
    [SerializeField]
    float lifespan = 15f;
    float spawnTime;
    public Action despawnHandler;
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent == null)
        {
            Despawn();
        }
    }

    private void Despawn()
    {
        if (lifespan <= Time.time - spawnTime)
        {
            Destroy(gameObject);
            despawnHandler();
        }
    }
}
