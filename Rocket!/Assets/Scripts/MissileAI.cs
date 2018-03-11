using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MissileAI : MonoBehaviour {

    public float rotSpeed = 100f, speed = 10f;
    [Range(1f, 5f)] public float explodeRadius = 3f;
    public GameObject explosionPrefab;

    private Transform target;
    private Rigidbody rb;
    private LevelManager levelManager;
    private WeaponManager weaponManager;

    void Start () {
        levelManager = LevelManager.instance;
        weaponManager = levelManager.player.GetComponent<WeaponManager>();
        rb = GetComponent<Rigidbody>();
        FindEnemy();
    }

	void FixedUpdate () {
        rb.velocity = transform.up * speed;

        if (target == null) {
            FindEnemy();
        }

        Vector3 dir = target.position - transform.position;
        Vector3 rotAmount = Vector3.Cross(dir.normalized, transform.up);
        rb.angularVelocity = -rotAmount * rotSpeed;
    }

    void FindEnemy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length > 0) {
            float minDist = float.PositiveInfinity;
            foreach (GameObject enemy in enemies) {
                float newDist = Vector3.Distance(enemy.transform.position, transform.position);
                if (newDist < minDist) {
                    minDist = newDist;
                    target = enemy.transform;
                }
            }
        }
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
                float dmg = (float)weaponManager.weapons[3].Damage / dist;
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
