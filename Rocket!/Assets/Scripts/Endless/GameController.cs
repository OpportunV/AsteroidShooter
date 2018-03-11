using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    #region Singleton
    public static GameController instance;

    void Awake() {
        if (instance != null) {
            Debug.LogError("Game controller has more than 1 instance");
            return;
        }
        instance = this;
    }
    #endregion

    public GameObject[] asteroids;
    public GameObject player, popUpPrefab;
    public Enforcer durationEn, firerateEn;
    public GUIController gUIController;
    public bool gameOver, pause;
    public Vector3 leftDown, rightUp, weaponScale;
    public int score, ultiWeaponCount;
    public float currentWeaponTime, fullWeaponTime, ultiWeaponChargeTime, shieldChargeTime;
    public float gameTimer;

    void Start() {
        ultiWeaponChargeTime = 110f;
        shieldChargeTime = 60f;
        ultiWeaponCount = 0;
        pause = false;
        gameOver = false;
        gameTimer = 0f;
        score = 0;
        leftDown = Camera.main.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        rightUp = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
        StartCoroutine(SpawnAsteroids());
        StartCoroutine(SpawnFirerateEnforcer());
        StartCoroutine(SpawnDurationEnforcer());
        StartCoroutine(UltiWeapon());
        StartCoroutine(ShieldCharger());
    }

    void FixedUpdate() {
        if (gameOver) {
            return;
        }
        gameTimer += Time.fixedDeltaTime;
        gUIController.timerText.text = gameTimer.ToString("0.0");
        gUIController.waveFillImage.fillAmount = gameTimer % ultiWeaponChargeTime / ultiWeaponChargeTime;
        gUIController.shieldFillImage.fillAmount = gameTimer % shieldChargeTime / shieldChargeTime;
        gUIController.weaponImage.GetComponent<RectTransform>().localScale = new Vector3(
            currentWeaponTime / fullWeaponTime, weaponScale.y, weaponScale.z);
        currentWeaponTime -= Time.fixedDeltaTime;

        if (currentWeaponTime <= 0f) {
            StartCoroutine(player.GetComponent<PlayerController>().WeaponSwitcher());
        }
    }

    public void OnGameOver() {
        gameOver = true;
        int maxScore, savedScore = PlayerPrefs.GetInt("MaxScore");
        float maxTime, savedTime = PlayerPrefs.GetFloat("MaxTime");
        maxScore = score > savedScore ? score : savedScore;
        maxTime = gameTimer > savedTime ? gameTimer : savedTime;
        maxTime = float.Parse(maxTime.ToString("0.0"));
        PlayerPrefs.SetInt("MaxScore", maxScore);
        PlayerPrefs.SetFloat("MaxTime", maxTime);
        gUIController.gameOverMenu.SetActive(true);
        gUIController.maxScore.SetActive(true);
        gUIController.maxScore.GetComponent<Text>().text = "Max Score: " + maxScore;
        gUIController.maxTime.SetActive(true);
        gUIController.maxTime.GetComponent<Text>().text = "Max Time: " + maxTime;
    }

    public void SetScore() {
        score++;
        gUIController.scoreText.text = score.ToString();
    }

    public void SetTimer(Color color, float time) {
        gUIController.weaponImage.color = color;
        weaponScale = gUIController.weaponImage.GetComponent<RectTransform>().localScale;
        currentWeaponTime = time;
        fullWeaponTime = time;
    }

    public void SetFirerate(float x) {
        gUIController.firerateText.text = (1 / x).ToString("0.00");
    }

    public void UpdateUltiGUI() {
        gUIController.waveText.text = ultiWeaponCount.ToString();
    }

    IEnumerator SpawnAsteroids() {
        while (!gameOver) {
            Vector3 spawnPos;
            do {
                spawnPos = new Vector3(Random.Range(leftDown.x * 2, rightUp.x * 2), Random.Range(leftDown.y * 2, rightUp.y * 2), 0);
            } while ((spawnPos - Vector3.zero).magnitude < (rightUp.x - leftDown.x));

            Instantiate(asteroids[Random.Range(0, asteroids.Length)], spawnPos, Quaternion.identity);
            float waitTime;
            waitTime = 1f - score / 500f;
            waitTime = Mathf.Clamp(waitTime, 0.4f, 0.9f);
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator SpawnFirerateEnforcer() {
        yield return new WaitForSeconds(Random.Range(20f, 25f));
        while (!gameOver) {
            Instantiate(firerateEn.prefab, new Vector3(
                Random.Range(leftDown.x, rightUp.x), Random.Range(leftDown.y, rightUp.y), player.transform.position.z),
                Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(20f, 25f));
        }
    }

    IEnumerator SpawnDurationEnforcer() {
        yield return new WaitForSeconds(Random.Range(35f, 40f));
        while (!gameOver) {
            Instantiate(durationEn.prefab, new Vector3(
                Random.Range(leftDown.x, rightUp.x), Random.Range(leftDown.y, rightUp.y), player.transform.position.z),
                Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(35f, 40f));
        }
    }

    IEnumerator UltiWeapon() {
        yield return new WaitForSeconds(ultiWeaponChargeTime);
        while (!gameOver) {
            ultiWeaponCount++;
            UpdateUltiGUI();
            GameObject popUp = Instantiate(popUpPrefab, Vector3.zero, Quaternion.identity);
            popUp.GetComponent<PopUpController>().SetUp("Ultimate available! \n (SPACE)", Color.yellow);
            Destroy(popUp, 1f);
            yield return new WaitForSeconds(ultiWeaponChargeTime);
        }
    }

    IEnumerator ShieldCharger() {
        while (!gameOver) {
            player.GetComponentInChildren<ShieldController>().SetUpShield();
            GameObject popUp = Instantiate(popUpPrefab, Vector3.zero, Quaternion.identity);
            popUp.GetComponent<PopUpController>().SetUp("Shield up!", Color.blue);
            Destroy(popUp, 1f);
            yield return new WaitForSeconds(shieldChargeTime);
        }
    }
}
