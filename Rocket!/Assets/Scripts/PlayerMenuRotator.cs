using UnityEngine;

public class PlayerMenuRotator : MonoBehaviour {

    public Transform target;
    public float forceMultiplyer;

    private Rigidbody rb;

	void Start() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(Random.Range(6.3f, 7.2f), 0f, 0f);
	}
	
	void FixedUpdate() {
        float zAngle = -Mathf.Atan2(target.position.x - transform.position.x, target.position.y - transform.position.y) * 180f / Mathf.PI;
        rb.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, zAngle), 10f);
        rb.AddForce(forceMultiplyer * (target.position - rb.position) / 
            Mathf.Pow((target.position - rb.position).magnitude, 3),ForceMode.Acceleration);
	}
}
