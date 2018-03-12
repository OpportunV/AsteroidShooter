using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerLevels : MonoBehaviour {

    public GameObject explosionEffect, ultiWeaponPrefab;
    public ParticleSystem mainPS, leftPS, rightPS, stopPSLeft, stopPSRight;
    public AnimationCurve engineCurve;
    public Image healthBarImage;
    public Text healthText;
    public float maxVelocityAxis = 4f, angularSpeed, force;
    public int maxHealth = 200;
    private bool oldSchoolControls = false;

    private WeaponManager weaponManager;
    private Rigidbody rb;
    private GameObject temp;
    private LevelManager levelManager;
    private Vector3 prevRotAmount = Vector3.zero;
    private float angVel = 10f, angVelZ;
    private int currentHealth;

    void Start() {
        oldSchoolControls = PlayerPrefs.GetInt("oldSchoolControls") == 0;
        rb = GetComponent<Rigidbody>();
        weaponManager = GetComponent<WeaponManager>();
        mainPS = weaponManager.playerSkins[weaponManager.currentPlayerSkinIndex].mainPS;
        leftPS = weaponManager.playerSkins[weaponManager.currentPlayerSkinIndex].leftPS;
        rightPS = weaponManager.playerSkins[weaponManager.currentPlayerSkinIndex].rightPS;
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

        if (oldSchoolControls) {
            rb.angularDrag = 30f;
        } else {
            rb.angularDrag = 0f;
        }
        TakeDamage(0);
    }

    void FixedUpdate() {
        if (levelManager.isGameOver) {
            Destroy(gameObject);
            return;
        }

        if (oldSchoolControls) {

            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0f;

            Vector3 rotAmount = Vector3.Cross((target - transform.position).normalized, transform.up);
            Vector3 deltaRotAmount = rotAmount - prevRotAmount;
            prevRotAmount = rotAmount;


            if (Mathf.Abs(deltaRotAmount.z) < 0.05f * Mathf.Deg2Rad) {
                rightPS.Stop();
                leftPS.Stop();
            }
            else {
                if (deltaRotAmount.z > 0f) {
                    rightPS.Stop();
                    leftPS.Play();
                }
                else {
                    leftPS.Stop();
                    rightPS.Play();
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

                var em = mainPS.emission;
                em.rateOverTime = engineCurve.Evaluate(force.magnitude);
                mainPS.Play(false);
            }
            else {
                mainPS.Stop(false);
            }

        } else {

            angVelZ = rb.angularVelocity.z;

            if (Input.GetKey(KeyCode.D)) {
                leftPS.Play();
                angVelZ -= angularSpeed * Time.fixedDeltaTime;
            }
            else {
                leftPS.Stop();
            }

            if (Input.GetKey(KeyCode.A)) {
                rightPS.Play();
                angVelZ += angularSpeed * Time.fixedDeltaTime;
            }
            else {
                rightPS.Stop();
            }

            rb.angularVelocity = new Vector3(0f, 0f, angVelZ);

            if (Input.GetKey(KeyCode.W)) {
                mainPS.Play();
                rb.AddForce(transform.up * force, ForceMode.Acceleration);
            }
            else {
                mainPS.Stop();
            }

            if (Input.GetKey(KeyCode.S)) {
                stopPSLeft.Play();
                stopPSRight.Play();
                rb.AddForce(-rb.transform.up * force * 0.5f, ForceMode.Acceleration);
            }
            else {
                stopPSLeft.Stop();
                stopPSRight.Stop();
            }
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
            tempDamage = Mathf.Max(Random.Range(21, 40), tempDamage);
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