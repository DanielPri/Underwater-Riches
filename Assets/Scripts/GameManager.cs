using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject goldSpawns;
    [SerializeField] GameObject goldBar;

    [SerializeField] GameObject enemySpawns;
    [SerializeField] GameObject shark;
    [SerializeField] GameObject octopus;
    [SerializeField] GameObject player;
    TextMeshProUGUI scoreText;
    int score = 0;
    int currentLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        HandleGold();
        HandleEnemies();
        player.GetComponent<Player>().scoreHandler += DisplayScore;
    }

    private void HandleEnemies()
    {
        //sharks
        Transform[] enemySpawnLocations = enemySpawns.GetComponentsInChildren<Transform>();
        int amountOfSharks = 1 + currentLevel;
        for(int i = 0; i < amountOfSharks; i++)
        {
            UnityEngine.Object.Instantiate(shark, enemySpawnLocations[UnityEngine.Random.Range(0, enemySpawnLocations.Length)]);
        }

        //octopus
        UnityEngine.Object.Instantiate(octopus, enemySpawnLocations[UnityEngine.Random.Range(0, enemySpawnLocations.Length)]);
    }

    private void HandleGold()
    {
        Transform[] goldSpawnLocations = goldSpawns.GetComponentsInChildren<Transform>();
        foreach (Transform location in goldSpawnLocations)
        {
            //Debug.Log("spawning gold at: " + location.position.x + "," + location.position.y);
            UnityEngine.Object.Instantiate(goldBar, location.position, Quaternion.identity);
        }
    }

    void DisplayScore()
    {
        score++;
        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
