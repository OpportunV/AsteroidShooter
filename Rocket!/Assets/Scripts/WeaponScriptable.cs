using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable/Weapon")]
public class WeaponScriptable : ScriptableObject {

    public new string name;
    public float startFireRate, fireRate, maxFireRate, duration, destroyTime;
    public GameObject prefab;
    public Color color;
    public Sprite icon;
    public Sprite iconActive;
    public int minDamage, maxDamage, opensAtLevel;

    public int Damage {
        get {
            return Random.Range(minDamage, maxDamage);
        }
    }

    public void ModifyFireRate(float value) {
        fireRate += value;
        fireRate = Mathf.Min(maxFireRate, fireRate);
    }

    public void Refresh() {
        fireRate = startFireRate;
    }
}
