using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE;

    public TextMeshProUGUI waveTimerText;
    public List<GameObject> enemeyPrefabs = new List<GameObject>();
    public GameObject coconutPrefab;
    public GameObject pauseScreen;
    public GameObject learnScreen;

    [Header ("Wave Settings")]
    public float waveDuration = 10f;
    public float enemySpeedIncrease = 0f;
    public int islandRemovaleAmmount = 4;
    public int newEnemyAmmount = 1;
    public bool paused = false;
    public bool ready = false;

    private float waveTimer = 0f;
    // Start is called before the first frame update
    void Awake()
    {
        if(INSTANCE == null)
        {
            INSTANCE = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Update()
    {
        if(!paused && ready)
        {
            UpdateTimer();
        }
    }

    public void StartGame()
    {
        StartNewWave();
        ready = true;
        Destroy(learnScreen);
    }

    private void UpdateTimer()
    {
        waveTimer += Time.deltaTime;

        if(waveTimer >= waveDuration)
        {
            StartNewWave();
            waveTimer = 0f;
        }
        float countdown = (float)Math.Round(waveDuration - waveTimer, 2);
        waveTimerText.text = countdown.ToString();
    }

    private void StartNewWave()
    {
        for (int i = 0; i < newEnemyAmmount; i++)
        {
            SpawnNewEnemy();
        }
        SpawnNewCoconut();

        UpgradeWave();
    }

    private void UpgradeWave()
    {
        newEnemyAmmount++;
        waveDuration += 0.3f;
        enemySpeedIncrease += 0.1f;
    }

    private void SpawnNewEnemy()
    {
        if(enemeyPrefabs.Count > 0)
        {
            int rand = UnityEngine.Random.Range(0, enemeyPrefabs.Count);
            GameObject enemyPrefab = enemeyPrefabs[rand];

            List<Vector3> spawnPoints = new List<Vector3>();

            foreach(Transform child in transform)
            {
                spawnPoints.Add(child.position);
            }

            rand = UnityEngine.Random.Range(0, spawnPoints.Count);
            float randX = UnityEngine.Random.Range(-1, 1);
            float randY = UnityEngine.Random.Range(-1, 1);
            Vector3 randVec = new Vector3(randX, randY, 0);

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoints[rand] + randVec, Quaternion.identity);
            newEnemy.GetComponent<Enemy>().speed += enemySpeedIncrease;
        }
    }

    private void SpawnNewCoconut()
    {
        float randX = UnityEngine.Random.Range(Player.instance.gameBounds.bounds.min.x, Player.instance.gameBounds.bounds.max.x);
        float randY = UnityEngine.Random.Range(Player.instance.gameBounds.bounds.min.y, Player.instance.gameBounds.bounds.max.y);

        Instantiate(coconutPrefab, new Vector3(randX, randY, 0), Quaternion.identity);
    }

    public void PauseGame()
    {
        paused = !paused;
        pauseScreen.SetActive(paused);
    }

    public void ReloadScene()
    {
        IslandPart.island = new List<IslandPart>();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
