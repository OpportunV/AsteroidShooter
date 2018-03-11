using UnityEngine;

public class MissileForward : MonoBehaviour {

    public float speed = 10f;
    [Range(1f, 5f)] public float explodeRadius = 3f;
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
        if (other.CompareTag("Player") || other.CompareTag("Portal")) {
            return;
        }
        Destroy(gameObject);
    }

    void OnDestroy() {
        if (levelManager.isQuitting || levelManager.isPause || levelManager.isGameOver) {
            return;
        }
        Destroy(Instantiate(explosionPrefab, transform.position, transform.rotation), 1f);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explodeRadius);
        foreach (Collider col in colliders) {
            if (col.tag == "Enemy") {
                float dist = Vector3.Distance(transform.position, col.transform.position);
                float dmg = (float)weaponManager.weapons[4].Damage / dist;
                var ac = col.GetComponent<AsteroidControllerLevels>();
                if (ac != null) {
                    ac.OnHit((int)dmg);
                }
            }
            if (col.tag == "Player") {
                float dist = Vector3.Distance(transform.position, col.transform.position);
                float dmg = (float)weaponManager.weapons[4].Damage / dist;
                var pcl = col.GetComponentInParent<PlayerControllerLevels>();
                if (pcl != null) {
                    pcl.TakeDamage((int)dmg);
                }
            }
        }
    }
}
