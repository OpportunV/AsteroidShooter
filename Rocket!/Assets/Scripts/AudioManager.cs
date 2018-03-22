using UnityEngine;

public class AudioManager : MonoBehaviour {

    #region Singleton and DontDestroy
    public static AudioManager instance;
	void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Debug.LogWarning("more than one instance of AudioManager");
            Destroy(gameObject);
        }
	}
    #endregion

    void Update() {

    }
}
