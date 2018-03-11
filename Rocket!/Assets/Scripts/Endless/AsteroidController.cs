using UnityEngine;

public class AsteroidController : MonoBehaviour
{
	public GameObject explosion;
    public int currentHealth;
    public RectTransform healthImage, canvas;

    private int startHealth;
	private GameObject temp;
    private Rigidbody rb;
    private float scaleValue;
    private PlayerController player;
    private float scale;
    public bool isChild = false;

    void Start()
    {
        player = GameController.instance.player.GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody>();
        scale = transform.localScale.z;
        if (!isChild) {
            startHealth = Random.Range(50, 120);
            currentHealth = startHealth;
            scaleValue = scale + (float)startHealth / 1000;
            scaleValue = Mathf.Clamp(scaleValue, scale - 0.1f, scale + 0.1f);
            transform.localScale = Vector3.one * scaleValue;
        } else {
            startHealth = Random.Range(20, 40);
            currentHealth = startHealth;
            scaleValue = scale - 0.1f;
        }
        transform.localScale = Vector3.one * scaleValue;

        rb.angularVelocity = Random.insideUnitSphere * Random.Range(0.2f, 4f);
        rb.AddForce((new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f))
            - transform.position).normalized * Random.Range(100f, 200f));
    }

    void FixedUpdate() {
        canvas.localRotation = Quaternion.identity;
        canvas.rotation = Quaternion.identity;
    }

	void OnParticleCollision() {
        OnHit();
    }

    public void OnHit() {
        currentHealth -= player.currentWeapon.damage;
        float imgScale = (float)currentHealth / (float)startHealth;
        healthImage.localScale = new Vector3(imgScale, healthImage.localScale.y, healthImage.localScale.z);
        if (currentHealth <= 0) {
            GameController.instance.SetScore();
            if (scaleValue.Equals(scale + 0.1f) && Random.Range(0, 11) == 0) {
                for (int i = 0, n = Random.Range(1, 4); i < n; i++) {
                    Instantiate(GameController.instance.asteroids[0], transform.position, transform.rotation)
                        .GetComponent<AsteroidController>().isChild = true;
                }
            }
            DestroingAsteroid();
        }
    }

    public void OnHit(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            GameController.instance.SetScore();
            if (scaleValue.Equals(scale + 0.1f) && Random.Range(0, 11) == 0) {
                for (int i = 0, n = Random.Range(1, 4); i < n; i++) {
                    Instantiate(GameController.instance.asteroids[0], transform.position, transform.rotation)
                        .GetComponent<AsteroidController>().isChild = true;
                }
            }
            DestroingAsteroid();
        } else {
            float imgScale = (float)currentHealth / (float)startHealth;
            healthImage.localScale = new Vector3(imgScale, healthImage.localScale.y, healthImage.localScale.z);
        }
    }

    public void DestroingAsteroid() {
        temp = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        temp.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        Destroy(gameObject);
        Destroy(temp, 1.1f);
    }

}