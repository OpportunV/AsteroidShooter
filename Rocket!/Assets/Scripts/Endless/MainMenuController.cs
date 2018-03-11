using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    public GameObject howToPlayPanel, howToPlayText;
    private bool onProcess = false;
    public Text maxScoreText, maxTimeText;

    void Start() {
        maxScoreText.text = "Max Score: " + PlayerPrefs.GetInt("MaxScore");
        maxTimeText.text = "Max Time: " + PlayerPrefs.GetFloat("MaxTime");
    }

    public void OnButtonPlay() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void OnButtonQuit() {
        Application.Quit();
    }

    public void OnButtonHowToPlay() {
        if (onProcess) {
            return;
        }
        if (howToPlayPanel.activeInHierarchy) {
            howToPlayText.SetActive(false);
            onProcess = true;
            Invoke("TurnOffPanel", 0.501f);
            howToPlayPanel.GetComponent<Animator>().SetTrigger("OnDestroy");
        } else {
            howToPlayPanel.SetActive(true);
        }
        
    }

    void TurnOffPanel() {
        howToPlayPanel.SetActive(false);
        onProcess = false;
    }
}
