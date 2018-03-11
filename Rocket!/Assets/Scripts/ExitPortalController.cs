using UnityEngine;

public class ExitPortalController : MonoBehaviour {

    private LevelManager levelManager;

    void Start() {
        levelManager = LevelManager.instance;
    }

	void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            levelManager.ingameCanvasManager.OnEnterPortal();
        }
    }
}
