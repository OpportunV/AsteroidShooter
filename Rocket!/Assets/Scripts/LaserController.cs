using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

    public LineRenderer laser;
    public float maxDistance;
    public GameObject hitPrefab;

    private RaycastHit hit, hitFixed;
    private Vector3 dir;
    private WeaponManager weaponManager;
    private GameObject temp;

    void Start() {
        weaponManager = LevelManager.instance.player.GetComponent<WeaponManager>();
    }

    void Update() {
        dir = transform.up;

        if (Physics.Raycast(transform.position, dir, out hit, maxDistance)) {
            if (hit.collider.CompareTag("Weapon")) {
                SetUpLaser(hit.point, true);
                Destroy(hit.collider.gameObject);
            } else if (hit.collider.tag == "Enemy") {
                SetUpLaser(hit.point, true);
            } else {
                SetUpLaser(transform.position + dir * maxDistance, false);
            }
        } else {
            SetUpLaser(transform.position + dir * maxDistance, false);
        }
    }

    void FixedUpdate() {
        if (Physics.Raycast(transform.position, dir, out hit, maxDistance)) {
            if (hit.collider.tag == "Enemy") {
                AsteroidControllerLevels astCon = hit.collider.GetComponent<AsteroidControllerLevels>();
                if (astCon != null) {
                    astCon.OnHit(weaponManager.weapons[2].Damage);
                }
            }
        }
    }

    void SetUpLaser(Vector3 pos2, bool isHitting) {
        laser.SetPosition(0, transform.position);
        laser.SetPosition(1, pos2);

        if (isHitting) {
            temp = Instantiate(hitPrefab, pos2, Quaternion.identity);
            Destroy(temp, Time.deltaTime);
        }
    }
}
