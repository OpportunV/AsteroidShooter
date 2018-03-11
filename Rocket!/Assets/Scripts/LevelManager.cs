using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

    #region Singleton
    public static LevelManager instance;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Debug.LogWarning("more than one instance of LevelManager");
            Destroy(gameObject);
        }
        currentLevel = PlayerPrefs.GetInt("currentLevel");
    }

    #endregion

    public GameObject player, enterPortal, exitPortal, temp, enemyPrefab;
    public bool isQuitting = false, isPause = false, isGameOver = false;
    public int currentLevel, enemiesCount, enemiesMultiplyer;
    public float halfSideOfLevel;
    public GameObject[] enemiesPrefabs;
    public Vector2 levelGrowMultiplyer;
    public IngameCanvasManager ingameCanvasManager;
    public int currentPlayerSkin = 1;

    void Start() {
        OnExitPortal();
        currentPlayerSkin = PlayerPrefs.GetInt("currentPlayerSkin");
    }

    void SetUpLevel(int levelIndex) {
        halfSideOfLevel += Random.Range(levelIndex * levelGrowMultiplyer.x, levelIndex * levelGrowMultiplyer.y);
        
        enemiesCount += levelIndex * enemiesMultiplyer;

        int enemiesLenght = enemiesPrefabs.Length;

        for (int i = 0; i < enemiesCount; i++) {
            enemyPrefab = enemiesPrefabs[Random.Range(0, enemiesLenght)];
            temp = Instantiate(enemyPrefab, GetRandomPos(4f), Quaternion.identity);
        }

        temp = Instantiate(enterPortal, GetRandomPos(0.6f * halfSideOfLevel), Quaternion.identity);
        Debug.Log(levelIndex);
    }

    Vector3 GetRandomPos(float deadZoneValue) {
        float xPos, yPos;
        do {
            xPos = Random.Range(-halfSideOfLevel + 1.5f, halfSideOfLevel - 1.5f);
            yPos = Random.Range(-halfSideOfLevel + 1.5f, halfSideOfLevel - 1.5f);
        } while (-deadZoneValue < xPos && xPos < deadZoneValue && -deadZoneValue < yPos && yPos < deadZoneValue);

        return new Vector3(xPos, yPos, 0f);
    }



    void OnApplicationQuit() {
        isQuitting = true;
        // PlayerPrefs.SetInt("currentLevel", 0);
    }

    public void OnGameOver() {
        isGameOver = true;
        int topLevel = PlayerPrefs.GetInt("topLevel");
        PlayerPrefs.SetInt("topLevel", Mathf.Max(topLevel, currentLevel));
        PlayerPrefs.SetInt("currentLevel", 0);
        ingameCanvasManager.OnGameOver();
    }

    public void OnEnterPortal() {
        Time.timeScale = 0f;
        isPause = true;
        currentLevel++;
        int topLevel = PlayerPrefs.GetInt("topLevel");
        PlayerPrefs.SetInt("topLevel", Mathf.Max(topLevel, currentLevel));
        PlayerPrefs.SetInt("currentLevel", currentLevel);
    }

    public void OnExitPortal() {
        SetUpLevel(currentLevel);
        Destroy(Instantiate(exitPortal, Vector3.zero, Quaternion.identity), 1.5f);
        Time.timeScale = 1f;
        isPause = false;
    }
}