using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable/Weapon")]
public class WeaponScriptable : ScriptableObject {

    public new string name;
    public float startFireRate, fireRate, maxFireRate, destroyTime;
    public GameObject prefab;
    public Color color;
    public Sprite icon;
    public Sprite iconActive;
    public int minDamage, maxDamage, startMinDamage, startMaxDamage, opensAtLevel;

    public int Damage {
        get {
            return Random.Range(minDamage, maxDamage);
        }
    }

    public void ModifyFireRate(float value) {
        fireRate += value;
        fireRate = Mathf.Min(maxFireRate, fireRate);
    }

    public void ModifyDamage(int value) {
        minDamage += value;
        maxDamage += value;
    }

    public void ModifyDamage(float percentage) {
        minDamage = (int)(minDamage + percentage * minDamage);
        maxDamage = (int)(maxDamage + percentage * maxDamage);
    }

    public void Refresh() {
        fireRate = startFireRate;
        minDamage = startMinDamage;
        maxDamage = startMaxDamage;
    }
}
