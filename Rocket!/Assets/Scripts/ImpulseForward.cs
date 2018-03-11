using UnityEngine;

public class ImpulseForward : MonoBehaviour {

    public float speed = 20f;
    public GameObject explosionPrefab;

    private Rigidbody rb;
    private LevelManager levelManager;
    private WeaponManager weaponManager;

    void Start() {
        levelManager = LevelManager.instance;
        weaponManager = levelManager.player.GetComponent<WeaponManager>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.up * speed;
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag != "Enemy") {
            return;
        }
        var ac = other.GetComponent<AsteroidControllerLevels>();
        if (ac != null && weaponManager != null) {
            ac.OnHit(weaponManager.weapons[0].Damage);
        }
        Destroy(gameObject);
    }

    void OnDestroy() {
        if (levelManager.isQuitting || levelManager.isPause || levelManager.isGameOver) {
            return;
        }
        Destroy(Instantiate(explosionPrefab, transform.position, transform.rotation), 1f);
    }
}
