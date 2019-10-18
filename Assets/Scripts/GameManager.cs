using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{


    [Header("Game settings")]
    [SerializeField] float levelDuration = 15f;
    [SerializeField] float goldSpawnFrequency = 5f;
    [SerializeField] float enemySpawnFrequency = 5f;
    [SerializeField] float minimumEnemyDuration = 5f;

    [Header("Gold")]
    [SerializeField] GameObject goldSpawns;
    [SerializeField] GameObject goldBar;
    [SerializeField] GameObject goldBarMedium;
    [SerializeField] GameObject goldLarge;
    GameObject[] golds;

    [Header("Characters")]
    [SerializeField] GameObject enemySpawns;
    [SerializeField] GameObject shark;
    [SerializeField] GameObject octopus;
    [SerializeField] GameObject player;
    
    TextMeshProUGUI scoreText;
    int score = 0;
    TextMeshProUGUI levelText;
    int currentLevel = 1;

    // Start is called before the first frame update
    void Start()
    {
        golds =  new GameObject[] {goldBar, goldBarMedium, goldLarge};
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        levelText = GameObject.FindGameObjectWithTag("Level").GetComponent<TextMeshProUGUI>();
        player.GetComponent<Player>().scoreHandler += DisplayScore;
    }

    // Update is called once per frame
    void Update()
    {
        HandleLevel();
    }

    private void HandleLevel()
    {
        if(levelDuration <= Time.time)
        {
            levelDuration += levelDuration;
            currentLevel++;
            DisplayLevel();
        }
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
        ShuffleArray(goldSpawnLocations);

        int qtyOfGold = Mathf.Clamp(currentLevel, 0, goldSpawnLocations.Length);
        Debug.Log("Amount of gold to spawn: " + qtyOfGold);
        for (int i = 0; i < qtyOfGold; i++)
        {
            Debug.Log("spawning gold at: " + goldSpawnLocations[i]);
            UnityEngine.Object.Instantiate(golds[UnityEngine.Random.Range(0, golds.Length)], goldSpawnLocations[i]);
        }
    }

    void DisplayScore(int value)
    {
        score += value;
        scoreText.text = "Score: " + score;
    }

    void DisplayLevel()
    {
        levelText.text = "Level " + currentLevel; 
    }

   

    void ShuffleArray<T>(T[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            // Pick a new index higher than current for each item in the array
            int r = i + UnityEngine.Random.Range(0, n - i);

            // Swap item into new spot
            T t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }
}