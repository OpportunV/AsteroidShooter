using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameCanvasManager : MonoBehaviour {

    public GameObject pauseMenu, gameOverMenu, inPortalMenu, howToPlayPanel;
    public GameObject[] weaponButtons;
    public WeaponManager weaponManager;
    public Text levelDoneText;

    private LevelManager levelManager;
    private int weaponButtonsNumber;

    void Start() {
        levelManager = LevelManager.instance;
        weaponButtonsNumber = weaponButtons.Length;
        for (int i = 0; i < weaponButtonsNumber; i++) {
            weaponButtons[i].SetActive(weaponManager.weapons[i].opensAtLevel <= levelManager.currentLevel);
        }
        Invoke("HowToPlay", 2f);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SetPause(pauseMenu.activeInHierarchy);
        }
    }

    void SetPause(bool paused) {
        if (paused) {
            levelManager.isPause = false;
            pauseMenu.SetActive(!paused);
            Time.timeScale = 1f;
        }
        else {
            levelManager.isPause = true;
            pauseMenu.SetActive(!paused);
            Time.timeScale = 0f;
        }
    }

    public void OnWeaponChanged(int activeWeapon) {
        for (int i = 0; i < weaponButtonsNumber; i++) {
            weaponButtons[i].GetComponent<Image>().sprite = weaponManager.weapons[i].icon;
        }
        weaponButtons[activeWeapon].GetComponent<Image>().sprite = weaponManager.weapons[activeWeapon].iconActive;
    }

    void HowToPlay() {
        if (PlayerPrefs.GetString("isHelpNeeded") == "yes") {
            howToPlayPanel.SetActive(true);
            Time.timeScale = 0f;
            levelManager.isPause = true;
        }
    }

    public void OnGotItButton() {
        PlayerPrefs.SetString("isHelpNeeded", "no");
        Time.timeScale = 1f;
        levelManager.isPause = false;
        howToPlayPanel.SetActive(false);
    }

    public void OnConcedeSureButton() {
        SetPause(pauseMenu.activeInHierarchy);
        levelManager.OnGameOver();
    }

    public void OnResumeButton() {
        SetPause(pauseMenu.activeInHierarchy);
    }

    public void OnGameOver() {
        gameOverMenu.SetActive(true);
    }

    public void OnQuitButton() {
        Application.Quit();
    }

    public void OnMenuButton() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelsMenu");
    }

    public void OnRestartButton() {
        SceneManager.LoadScene(1);
    }

    public void OnGoodsOneButton() {

    }

    public void OnGoodsTwoButton() {

    }

    public void OnGoodsThreeButton() {

    }

    public void OnNextLevelButton() {
        SceneManager.LoadScene(1);
    }

    public void OnEnterPortal() {
        levelDoneText.text = "LEVEL " + levelManager.currentLevel + " DONE!";
        inPortalMenu.SetActive(true);
        if (levelManager.player != null) {
            levelManager.player.GetComponent<PlayerControllerLevels>().SaveData();
        }
        levelManager.OnEnterPortal();
    }
}
