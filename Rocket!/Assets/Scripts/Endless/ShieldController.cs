using UnityEngine;
using System.Collections;

public class ShieldController : MonoBehaviour {

    public ParticleSystem shieldActive;
    public ParticleSystem shieldDissapear;
    public int shieldCharges;
    public bool isShielded;

    private GUIController gUI;

	void Start () {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0) {
            return;
        }
        gUI = GameController.instance.gUIController;
        isShielded = false;
	}
	
    public void SetUpShield() {
        if (shieldCharges == 0) {
            shieldActive.Play();
            isShielded = true;
            StartCoroutine(SetShielded(true));
        }
        shieldCharges++;
        UpdateUI();
    }

    public void UpdateUI() {
        gUI.shieldText.text = shieldCharges.ToString();
    }

    void OnTriggerEnter(Collider other) {
        if (shieldCharges == 0) {
            return;
        }
        if (other.CompareTag("Enemy")) {
            if (shieldCharges == 1) {
                shieldActive.Stop();
                StartCoroutine(SetShielded(false));
            }
            shieldCharges--;
            UpdateUI();
            shieldDissapear.Play();

            Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);
            foreach (Collider col in colliders) {
                if (col.CompareTag("Enemy")) {
                    col.GetComponent<AsteroidController>().OnHit(150);
                }
            }
        }
    }

    IEnumerator SetShielded(bool state) {
        yield return 0;
        isShielded = state;
        yield break;
    }
}
