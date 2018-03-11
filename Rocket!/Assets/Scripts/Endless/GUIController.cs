using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public Text scoreText, timerText, firerateText, shieldText, waveText;
    public Image weaponImage, shieldFillImage, waveFillImage;
    public GameObject maxScore, maxTime;

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SwitchingPause(pauseMenu.activeInHierarchy); 
        }
	}

    void SwitchingPause(bool paused) {
        if (paused) {
            GameController.instance.pause = false;
            pauseMenu.SetActive(!paused);
            Time.timeScale = 1f;
        } else {
            GameController.instance.pause = true;
            pauseMenu.SetActive(!paused);
            Time.timeScale = 0f;
        }
    }

    public void OnButtonResume() {
        SwitchingPause(pauseMenu.activeInHierarchy);
    }

    public void OnButtonReplay() {
        SwitchingPause(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OnButtonMenu() {
        SwitchingPause(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnButtonQuit() {
        Application.Quit();
    }
}
