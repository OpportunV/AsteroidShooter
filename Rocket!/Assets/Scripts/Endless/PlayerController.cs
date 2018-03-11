using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject explosionEffect, ultiWeaponPrefab;
	public Transform plasmaGun;
    public ParticleSystem reactiveParticles;
    public Weapon[] weapons;
    public Weapon currentWeapon;
    public AnimationCurve engineCurve;

    private float angVel = 10f, startTime = 0f;
    private GameController gameController;
    private ShieldController shieldController;

    GameObject temp;

	void Start() { 
        gameController = GameController.instance;
        shieldController = GetComponentInChildren<ShieldController>();
        currentWeapon = weapons[Random.Range(0, weapons.Length)];
        //currentWeapon = weapons[4];
        gameController.SetFirerate(currentWeapon.fireRate);
        gameController.SetTimer(currentWeapon.weaponColor, currentWeapon.timeToChange);
	}
	
	void FixedUpdate() {
        if (gameController.pause) {
            return;
        }
		Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		target.z = 0f;
        float zAngle = -Mathf.Atan2(target.x - transform.position.x, target.y - transform.position.y) * 180f / Mathf.PI;
        GetComponent<Rigidbody>().rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, zAngle), angVel);

        if (Input.GetKey(KeyCode.Mouse1)) {
            Vector3 force = target - transform.position;
            GetComponent<Rigidbody>().AddForce(force);
            var em = reactiveParticles.emission;
            em.rateOverTime = engineCurve.Evaluate(force.magnitude);
			reactiveParticles.Play();
		} else {
			reactiveParticles.Stop();
		}

        if (Input.GetKeyDown(KeyCode.Space) && gameController.ultiWeaponCount > 0) {
            gameController.ultiWeaponCount--;
            gameController.UpdateUltiGUI();
            Destroy(Instantiate(ultiWeaponPrefab, transform.position, transform.rotation), 1.5f);
            Collider[] colliders = Physics.OverlapSphere(transform.position, 30f);
            foreach (Collider col in colliders) {
                if (col.tag == "Enemy") {
                    float dist = Vector3.Distance(transform.position, col.transform.position);
                    float dmg = 3000f / dist;
                    col.GetComponent<AsteroidController>().OnHit((int)dmg);
                }
            }
        }

		if (Input.GetKey(KeyCode.Mouse0) && Time.time > startTime) {
            switch (currentWeapon.type) {
                case "blue":
                    temp = Instantiate(currentWeapon.prefab, plasmaGun.position, plasmaGun.rotation, plasmaGun);
                    Destroy(temp, 1.5f);
                    break;

                case "gray":
                    temp = Instantiate(currentWeapon.prefab, plasmaGun.position, plasmaGun.rotation);
                    break;

                case "yellow":
                    temp = Instantiate(currentWeapon.prefab, plasmaGun.position, plasmaGun.rotation);
                    break;

                default:
                    temp = Instantiate(currentWeapon.prefab, plasmaGun.position, plasmaGun.rotation);
                    Destroy(temp, 2f);
                    break;
            }
            if (Random.Range(0, 50) == 0) {
                Instantiate(weapons[3].prefab, plasmaGun.position, plasmaGun.rotation);
                GameObject popUp = Instantiate(gameController.popUpPrefab, Vector3.zero, Quaternion.identity);
                popUp.GetComponent<PopUpController>().SetUp("Free Missile!", Color.grey);
                Destroy(popUp, 1f);
            }
			startTime = Time.time + currentWeapon.fireRate;
		}

        GetComponent<Rigidbody>().position = new Vector3 (
			Mathf.Clamp(transform.position.x, gameController.leftDown.x, gameController.rightUp.x),
			Mathf.Clamp(transform.position.y, gameController.leftDown.y, gameController.rightUp.y), 0f);
	}

	void OnTriggerEnter(Collider other) {
        if (other.tag == "Enforcer") {
            if (other.GetComponent<EnforcerController>().type == "duration") {
                currentWeapon.ChangeDuration();
                gameController.currentWeaponTime += 5f;
                gameController.fullWeaponTime += 5f;
                GameObject popUp = Instantiate(gameController.popUpPrefab, Vector3.zero, Quaternion.identity);
                popUp.GetComponent<PopUpController>().SetUp(gameController.durationEn.text, gameController.durationEn.color);
                Destroy(popUp, 1f);
                Destroy(Instantiate(gameController.durationEn.pickUpPrefab, other.transform.position, Quaternion.identity), 1.1f);
                Destroy(other.gameObject);
                return;
            }

            if (other.GetComponent<EnforcerController>().type == "firerate") {
                currentWeapon.ChangeFireRate();
                gameController.SetFirerate(currentWeapon.fireRate);
                GameObject popUp = Instantiate(gameController.popUpPrefab, Vector3.zero, Quaternion.identity);
                popUp.GetComponent<PopUpController>().SetUp(gameController.firerateEn.text, gameController.firerateEn.color);
                Destroy(popUp, 1f);
                Destroy(Instantiate(gameController.firerateEn.pickUpPrefab, other.transform.position, Quaternion.identity), 1.1f);
                Destroy(other.gameObject);
            }
            return;
        }
        if (other.tag == "Enemy" && !shieldController.isShielded) {
            gameController.OnGameOver();
            other.GetComponent<AsteroidController>().DestroingAsteroid();
            temp = Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity);
            temp.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            Destroy(gameObject);
            Destroy(temp, 1.1f);
        }
	}

	public IEnumerator WeaponSwitcher() {
        Weapon nextWeapon;
        do {
            nextWeapon = weapons[Random.Range(0, weapons.Length)];
        } while (nextWeapon == currentWeapon);
        currentWeapon = nextWeapon;
        gameController.SetFirerate(currentWeapon.fireRate);
        gameController.SetTimer(currentWeapon.weaponColor, currentWeapon.timeToChange);
        yield break;
	}
}