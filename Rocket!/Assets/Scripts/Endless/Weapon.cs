using UnityEngine;

[System.Serializable]
public class Weapon {

    public string type;
    public float fireRate;
    public GameObject prefab;
    public float timeToChange;
    public Color weaponColor;
    public int damage;

    public void ChangeFireRate() {
        switch (type.ToLower()) {
            case "green":
                fireRate -= 0.02f;
                fireRate = Mathf.Clamp(fireRate, 0.05f, 0.2f);
                break;

            case "purple":
                fireRate -= 0.02f;
                fireRate = Mathf.Clamp(fireRate, 0.3f, 0.7f);
                break;

            case "blue":
                fireRate -= 0.1f;
                fireRate = Mathf.Clamp(fireRate, 1.5f, 2.5f);
                break;

            case "gray":
                fireRate -= 0.05f;
                fireRate = Mathf.Clamp(fireRate, 0.6f, 1f);
                break;

            case "yellow":
                fireRate -= 0.03f;
                fireRate = Mathf.Clamp(fireRate, 0.4f, 0.6f);
                break;
        }
    }

    public void ChangeDuration() {
        timeToChange += 5f;
    }
}
