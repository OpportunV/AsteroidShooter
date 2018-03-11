using UnityEngine;

[CreateAssetMenu(fileName = "New Enforcer", menuName = "Scriptable/Enforcer")]
public class Enforcer : ScriptableObject {

    public GameObject prefab, pickUpPrefab;
    public new string name;
    public string text;
    public Color color;
}
