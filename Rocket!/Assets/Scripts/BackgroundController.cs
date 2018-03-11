using UnityEngine;

public class BackgroundController : MonoBehaviour {

    public float parralax;
    private MeshRenderer mr;
    private Material material;
    private Vector2 offset;

    void Start() {
        mr = GetComponent<MeshRenderer>();
        material = mr.material;
    }

	void LateUpdate() {
        offset.x = transform.position.x / parralax;
        offset.y = transform.position.y / parralax;

        material.mainTextureOffset = offset;
    }
}
