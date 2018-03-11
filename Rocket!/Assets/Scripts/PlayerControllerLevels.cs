using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerLevels : MonoBehaviour {

    public GameObject explosionEffect, ultiWeaponPrefab;
    public ParticleSystem reactiveParticles, leftReactiveParticles, rightReactiveParticles;
    public AnimationCurve engineCurve;
    public Image healthBarImage;
    public Text healthText;
    public float maxVelocityAxis = 4f;
    public int maxHealth = 200;

    private WeaponManager weaponManager;
    private Rigidbody rb;
    private GameObject temp;
    private LevelManager levelManager;
    private Vector3 prevRotAmount = Vector3.zero;
    private float angVel = 10f;
    private int currentHealth;

    void Start() {
        rb = GetComponent<Rigidbody>();
        weaponManager = GetComponent<WeaponManager>();
        reactiveParticles = weaponManager.playerSkins[weaponManager.currentPlayerSkinIndex].mainPS;
        leftReactiveParticles = weaponManager.playerSkins[weaponManager.currentPlayerSkinIndex].leftPS;
        rightReactiveParticles = weaponManager.playerSkins[weaponManager.currentPlayerSkinIndex].rightPS;
        levelManager = LevelManager.instance;
        currentHealth = maxHealth;
        if (levelManager.currentLevel > 0) {
            rb.velocity = new Vector3(
                PlayerPrefs.GetFloat("playerVelocityX"),
                PlayerPrefs.GetFloat("playerVelocityY"),
                0f
                );
            weaponManager.SetCurrentWeapon(PlayerPrefs.GetInt("currentWeapon"));
            currentHealth = PlayerPrefs.GetInt("currentHealth");
        }
        TakeDamage(0);
    }

    void FixedUpdate() {
        if (levelManager.isGameOver) {
            Destroy(gameObject);
            return;
        }

        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0f;

        Vector3 rotAmount = Vector3.Cross((target - transform.position).normalized, transform.up);
        Vector3 deltaRotAmount = rotAmount - prevRotAmount;
        prevRotAmount = rotAmount;


        if (Mathf.Abs(deltaRotAmount.z) < 0.1f * Mathf.Deg2Rad) {
            rightReactiveParticles.Stop();
            leftReactiveParticles.Stop();
        }
        else {
            if (deltaRotAmount.z > 0f) {
                rightReactiveParticles.Stop();
                leftReactiveParticles.Play();
            } else {
                leftReactiveParticles.Stop();
                rightReactiveParticles.Play();
            }
        }

        float zAngle = -Mathf.Atan2(target.x - transform.position.x, target.y - transform.position.y) * 180f / Mathf.PI;
        rb.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, zAngle), angVel);

        if (Input.GetKey(KeyCode.Mouse1)) {
            Vector3 force = target - transform.position;

            if (rb.velocity.x >= maxVelocityAxis) {
                force.x = Mathf.Clamp(force.x, force.x, 0f);
                rb.velocity = new Vector3(
                    Mathf.Clamp(rb.velocity.x, -maxVelocityAxis, maxVelocityAxis),
                    rb.velocity.y,
                    0f
                    );
            }

            if (rb.velocity.y >= maxVelocityAxis) {
                force.y = Mathf.Clamp(force.y, force.y, 0f);
                rb.velocity = new Vector3(
                    rb.velocity.x,
                    Mathf.Clamp(rb.velocity.y, -maxVelocityAxis, maxVelocityAxis),
                    0f
                    );
            }

            if (rb.velocity.x <= -maxVelocityAxis) {
                force.x = Mathf.Clamp(force.x, 0f, force.x);
                rb.velocity = new Vector3(
                    Mathf.Clamp(rb.velocity.x, -maxVelocityAxis, maxVelocityAxis),
                    rb.velocity.y,
                    0f
                    );
            }

            if (rb.velocity.y <= -maxVelocityAxis) {
                force.y = Mathf.Clamp(force.y, 0f, force.y);
                rb.velocity = new Vector3(
                    rb.velocity.x,
                    Mathf.Clamp(rb.velocity.y, -maxVelocityAxis, maxVelocityAxis),
                    0f
                    );
            }

            rb.AddForce(force, ForceMode.Acceleration);
            
            var em = reactiveParticles.emission;
            em.rateOverTime = engineCurve.Evaluate(force.magnitude);
            reactiveParticles.Play(false);
        }
        else {
            reactiveParticles.Stop(false);
        }

        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, -levelManager.halfSideOfLevel + 1f, levelManager.halfSideOfLevel - 1f),
            Mathf.Clamp(rb.position.y, -levelManager.halfSideOfLevel + 1f , levelManager.halfSideOfLevel - 1f),
            0f
            );
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.CompareTag("Enemy")) {
            float tempDamage = collision.impulse.magnitude * collision.relativeVelocity.sqrMagnitude * 0.5f;
            tempDamage = Mathf.Max(30f, tempDamage);
            TakeDamage((int)tempDamage);
        }
    }

    public void TakeDamage(int damage) {
        currentHealth -= Mathf.Max(damage, 0);
        currentHealth = Mathf.Max(currentHealth, 0);
        healthBarImage.fillAmount = (float)currentHealth / maxHealth;
        healthText.text = string.Format("{0} / {1}", currentHealth, maxHealth);
        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }

    public void SaveData() {
        PlayerPrefs.SetFloat("playerVelocityX", rb.velocity.x);
        PlayerPrefs.SetFloat("playerVelocityY", rb.velocity.y);
        PlayerPrefs.SetInt("currentWeapon", weaponManager.currentWeaponIndex);
        PlayerPrefs.SetInt("currentHealth", currentHealth);
    }

    void OnDestroy() {
        if (levelManager.isQuitting || levelManager.isPause || levelManager.isGameOver) {
            return;
        }
        levelManager.OnGameOver();
        temp = Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().velocity = rb.velocity;
        Destroy(temp, 1.1f);
    }
}