using UnityEngine;
using UnityEngine.UI;

public class AsteroidControllerLevels : MonoBehaviour {

    public RectTransform healthBar;
    public Image healthBarImage;
    public int currentHealth = 200, healthModifyer;
    public GameObject explosion;

    private Rigidbody rb;
    private LevelManager levelManager;
    private WeaponManager weaponManager;
    private GameObject temp;
    private int startHealth;

	void Start() {
        levelManager = LevelManager.instance;
        weaponManager = levelManager.player.GetComponent<WeaponManager>();
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Random.insideUnitSphere;
        rb.AddForce(Random.insideUnitSphere * Random.Range(10f, 50f));
        currentHealth += healthModifyer * levelManager.currentLevel;
        startHealth = currentHealth;
	}

    void FixedUpdate() {
        healthBar.localRotation = Quaternion.identity;
        healthBar.rotation = Quaternion.identity;
    }

    void OnParticleCollision(GameObject other) {
        if (other.CompareTag("LaserPurple")) {
            OnHit(weaponManager.weapons[1].Damage);
        }
    }

    public void OnHit(int damage) {
        currentHealth -= damage;
        healthBarImage.fillAmount = (float)currentHealth / startHealth;
        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }

    void OnDestroy() {
        if (levelManager.isQuitting || levelManager.isPause || levelManager.isGameOver) {
            return;
        }

        temp = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        Destroy(temp, 1.1f);
    }

}
