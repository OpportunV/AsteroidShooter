using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponManager : MonoBehaviour {

    [System.Serializable]
    public class PlayerSkin {
        public GameObject playerSkin;
        public Transform[] weaponPositions;
        public ParticleSystem mainPS, leftPS, rightPS;
    }

    public PlayerSkin[] playerSkins;
    public int currentPlayerSkinIndex;
    public Transform currentWeaponPosition;
    public int currentWeaponIndex;
    public WeaponScriptable[] weapons;
    public IngameCanvasManager ingameCanvasManager;

    private WeaponScriptable currentWeapon;
    private PlayerSkin currentPlayerSkin;
    private GameObject tempShot;
    private float nextShotTime = 0f;
    private int curLvl, weaponPositionsLenght, weaponPosCounter = 0;

    void Start() {
        curLvl = LevelManager.instance.currentLevel;
        if (curLvl == 0) {
            foreach (var weap in weapons) {
                weap.Refresh();
            }
        }
        SetCurrentWeapon(currentWeaponIndex);
        SetupPlayerSkin();
    }

    void FixedUpdate() {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextShotTime &&
            !EventSystem.current.IsPointerOverGameObject()) {

            if (currentWeapon.name == "LaserBlue") {
                foreach (var curWeapPos in currentPlayerSkin.weaponPositions) {
                    tempShot = Instantiate(currentWeapon.prefab, curWeapPos.position, curWeapPos.rotation, curWeapPos);
                    Destroy(tempShot, Time.deltaTime);
                }                
            } else {
                tempShot = Instantiate(currentWeapon.prefab, currentWeaponPosition.position, currentWeaponPosition.rotation);
                Destroy(tempShot, currentWeapon.destroyTime);
                nextShotTime = Time.time + 1f / currentWeapon.fireRate;
                GetNextWeaponPosition();
            }
        }

        

        if (Input.GetKey(KeyCode.Alpha1) && weapons[0].opensAtLevel <= curLvl) {
            SetCurrentWeapon(0);
        }
        if (Input.GetKey(KeyCode.Alpha2) && weapons[1].opensAtLevel <= curLvl) {
            SetCurrentWeapon(1);
        }
        if (Input.GetKey(KeyCode.Alpha3) && weapons[2].opensAtLevel <= curLvl) {
            SetCurrentWeapon(2);
        }
        if (Input.GetKey(KeyCode.Alpha4) && weapons[3].opensAtLevel <= curLvl) {
            SetCurrentWeapon(3);
        }
        if (Input.GetKey(KeyCode.Alpha5) && weapons[4].opensAtLevel <= curLvl) {
            SetCurrentWeapon(4);
        }
    }

    private void GetNextWeaponPosition() {
        weaponPosCounter++;
        currentWeaponPosition = currentPlayerSkin.weaponPositions[weaponPosCounter % weaponPositionsLenght];
    }

    public void SetupPlayerSkin() {
        currentPlayerSkin = playerSkins[currentPlayerSkinIndex];
        weaponPositionsLenght = currentPlayerSkin.weaponPositions.Length;
        currentWeaponPosition = playerSkins[currentPlayerSkinIndex].weaponPositions[0];

        for (int i = 0, n = playerSkins.Length; i < n; i++) {
            playerSkins[i].playerSkin.SetActive(false);
        }

        playerSkins[currentPlayerSkinIndex].playerSkin.SetActive(true);
    }

    public void SetCurrentWeapon(int index) {
        currentWeaponIndex = index;
        currentWeapon = weapons[currentWeaponIndex];
        ingameCanvasManager.OnWeaponChanged(index);
    }
}
