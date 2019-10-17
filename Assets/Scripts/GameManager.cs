using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject goldSpawns;
    [SerializeField] GameObject goldBar;

    // Start is called before the first frame update
    void Start()
    {
        Transform[] spawnLocations = goldSpawns.GetComponentsInChildren<Transform>();
        foreach(Transform location in spawnLocations)
        {
            Debug.Log("spawning gold at: " + location.position.x + "," + location.position.y);
            Object.Instantiate(goldBar, location.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
