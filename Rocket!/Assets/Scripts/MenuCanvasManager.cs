using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCanvasManager : MonoBehaviour {

    public Text topLevel;
    public Text currentLevel;

    void Start() {
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
}
