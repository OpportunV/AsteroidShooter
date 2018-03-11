using UnityEngine;

public class CameraController : MonoBehaviour {

    public Vector3 camOffset;
    public Transform target;
    public float scrollSpeed, scrollMin, scrollMax;

    private Vector2 camLimitX, camLimitY, camSide;
    private LevelManager levelManager;
    private float scroollWheelValue, inGamePanelHeight;

    void Start() {
        levelManager = LevelManager.instance;
        Camera.main.orthographicSize = Mathf.Clamp(PlayerPrefs.GetFloat("cameraSize"), scrollMin, scrollMax);
        inGamePanelHeight = (Camera.main.ScreenToWorldPoint(new Vector3(0, 84, 0)) - 
                             Camera.main.ScreenToWorldPoint(Vector3.zero)).y;
    }

    void LateUpdate() {
        if (target != null) {
            transform.position = target.position + camOffset;
        }

        if (levelManager == null) {
            return;
        }

        var orthographicSize = Camera.main.orthographicSize;

        scroollWheelValue = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroollWheelValue) > 0.05f) {
            orthographicSize -= scroollWheelValue * scrollSpeed;
            orthographicSize = Mathf.Clamp(orthographicSize, scrollMin, scrollMax);
            Camera.main.orthographicSize = orthographicSize;
            PlayerPrefs.SetFloat("cameraSize", orthographicSize);
        }

        camSide = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight, 0f)) -
            Camera.main.ScreenToWorldPoint(Vector3.zero);

        camLimitX = new Vector2(-levelManager.halfSideOfLevel + camSide.x / 2, levelManager.halfSideOfLevel - camSide.x / 2);
        camLimitY = new Vector2(-levelManager.halfSideOfLevel + camSide.y / 2 - inGamePanelHeight, levelManager.halfSideOfLevel - camSide.y / 2);

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, camLimitX.x, camLimitX.y),
            Mathf.Clamp(transform.position.y, camLimitY.x, camLimitY.y),
            transform.position.z
            );
    }
}
