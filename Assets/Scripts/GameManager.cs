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
    [SerializeField] float enemySpeedIncreasePerLevel = 1f;
    float elapsedLevelDuration;
    float nearlyElapsedLevelDuration;
    float elapsedGoldSpawnDuration;
    float elapsedEnemySpawnDuration;
    int maxSharkQty = 1;
    int currentSharkQty = 0;
    int currentGoldQty = 0;
    int maxOctopusQty = 2;
    int currentOctopusQty = 0;
    float speedIncrease = 0;
    int extralives = 2;

    [Header("Gold")]
    [SerializeField] GameObject goldSpawns;
    [SerializeField] GameObject goldBar;
    [SerializeField] GameObject goldBarMedium;
    [SerializeField] GameObject goldLarge;
    GameObject[] golds;
    Transform[] goldSpawnLocations;

    [Header("Characters")]
    [SerializeField] GameObject enemySpawns;
    [SerializeField] GameObject shark;
    [SerializeField] GameObject octopus;
    [SerializeField] GameObject player;
    
    TextMeshProUGUI scoreText;
    int score = 0;
    TextMeshProUGUI levelText;
    TextMeshProUGUI livesText;
    int currentLevel = 1;
    public Action<float> enemySpeedHandler;
    GameObject currentLife;

    // Start is called before the first frame update
    void Start()
    {

        currentLife = Instantiate(player, GameObject.FindGameObjectWithTag("Ship").transform.position, Quaternion.identity);
        elapsedLevelDuration = levelDuration;
        nearlyElapsedLevelDuration = levelDuration - 5f;
        golds =  new GameObject[] {goldBar, goldBarMedium, goldLarge};
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        levelText = GameObject.FindGameObjectWithTag("Level").GetComponent<TextMeshProUGUI>();
        livesText = GameObject.FindGameObjectWithTag("Lives").GetComponent<TextMeshProUGUI>();

        currentLife.GetComponent<Player>().scoreHandler += DisplayScore;
        currentLife.GetComponent<Player>().playerDeath += HandleDeath;
        InitGold();
    }

    private void HandleDeath()
    {
        if(extralives > 0)
        {
            extralives--;
            livesText.text = "Extra Lives remaining: " + extralives;
            currentLife = Instantiate(player, GameObject.FindGameObjectWithTag("Ship").transform.position, Quaternion.identity);
            currentLife.GetComponent<Player>().scoreHandler += DisplayScore;
            currentLife.GetComponent<Player>().playerDeath += HandleDeath;
        }
        else
        {
            levelText.text = "Game Over.";
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleLevel();
        HandleGold();
        HandleEnemies();
    }

    private void HandleLevel()
    {
        if (elapsedLevelDuration <= Time.time)
        {
            elapsedLevelDuration += levelDuration;
            currentLevel++;
            DisplayLevel();
            maxSharkQty = currentLevel + 4;
            speedIncrease = enemySpeedIncreasePerLevel - (enemySpeedIncreasePerLevel / currentLevel);
        }
        //this will always happen 5 seconds before the next level
        if (nearlyElapsedLevelDuration <= Time.time)
        {
            nearlyElapsedLevelDuration += levelDuration;
            enemySpeedHandler?.Invoke(speedIncrease);
        }
    }
    
    private void HandleEnemies()
    {
        //sharks
        Transform[] enemySpawnLocations = enemySpawns.GetComponentsInChildren<Transform>();
        
        if (elapsedEnemySpawnDuration <= Time.time)
        {
            //shark logic
            if (currentSharkQty < maxSharkQty)
            {
                GameObject newShark = GameObject.Instantiate(shark, enemySpawnLocations[UnityEngine.Random.Range(0, enemySpawnLocations.Length)].position, Quaternion.identity);
                sharkController newSharkController = newShark.GetComponent<sharkController>();
                newSharkController.deathHandler += SharkDecrement;
                newSharkController.movementSpeed += speedIncrease;
                currentSharkQty++;                
            }
            //octopus
            else if(currentOctopusQty < maxOctopusQty)
            {
                GameObject newOctopus = GameObject.Instantiate(octopus, enemySpawnLocations[UnityEngine.Random.Range(0, enemySpawnLocations.Length)].position, Quaternion.identity);
                octopusController newOctopusController = newOctopus.GetComponent<octopusController>();
                newOctopusController.deathHandler += OctopusDecrement;
                newOctopusController.movementSpeed += speedIncrease;
                currentOctopusQty++;
            }
            elapsedEnemySpawnDuration += enemySpawnFrequency;
        }
    }

    private void InitGold()
    {
        goldSpawnLocations = goldSpawns.GetComponentsInChildren<Transform>();
        ShuffleArray(goldSpawnLocations);
        elapsedGoldSpawnDuration = 0;
    }

    private void HandleGold()
    {
        if (elapsedGoldSpawnDuration <= Time.time && currentGoldQty < goldSpawnLocations.Length)
        {
            elapsedGoldSpawnDuration += goldSpawnFrequency;
            GameObject newgold = UnityEngine.Object.Instantiate(golds[UnityEngine.Random.Range(0, golds.Length)], goldSpawnLocations[UnityEngine.Random.Range(0, goldSpawnLocations.Length)].position, Quaternion.identity);
            newgold.GetComponent<goldController>().despawnHandler += GoldDecrement;
            currentGoldQty++;
        }
    }

    void DisplayScore(int value)
    {
        currentGoldQty--;
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

    void SharkDecrement()
    {
        currentSharkQty--;
    }
    void OctopusDecrement()
    {
        currentOctopusQty--;
    }
    void GoldDecrement()
    {
        currentGoldQty--;
    }
}