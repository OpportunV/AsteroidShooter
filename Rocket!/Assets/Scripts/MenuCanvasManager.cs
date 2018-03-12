using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCanvasManager : MonoBehaviour {

    public Text topLevel;
    public Text currentLevel;

    void Start() {
        PlayerPrefs.SetInt("oldSchoolControls", 0);
        GetComponent<Rigidbody>().velocity = Random.onUnitSphere;
        topLevel.text = "TOP LEVEL " + PlayerPrefs.GetInt("topLevel");
        currentLevel.text = "CURRENT LEVEL " + PlayerPrefs.GetInt("currentLevel");
    }
	
    public void OnPlayLevelsButton() {
        SceneManager.LoadScene(1);
    }

    public void OnPlayEndlessButton() {
        Debug.Log("Endless!");
    }

    public void OnQuitButton() {
        Debug.Log("Quitting!");
        Application.Quit();
    }

    public void OnHardControlsChanged(bool value) {
        if (value) {
            PlayerPrefs.SetInt("oldSchoolControls", 1);
        } else {
            PlayerPrefs.SetInt("oldSchoolControls", 0);
        }
    }
}
