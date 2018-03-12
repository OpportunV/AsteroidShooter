using UnityEngine;

public class NewPlayerController : MonoBehaviour {

    public ParticleSystem mainPS, leftPS, rightPS, stopPSLeft, stopPSRight;
    public float angularSpeed, force;

    private Rigidbody rb;
    private float angVelZ;

	void Start() {
        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate() {
        angVelZ = rb.angularVelocity.z;

        if (Input.GetKey(KeyCode.D)) {
            leftPS.Play();
            angVelZ -= angularSpeed * Time.fixedDeltaTime;
        } else {
            leftPS.Stop();
        }

        if (Input.GetKey(KeyCode.A)) {
            rightPS.Play();
            angVelZ += angularSpeed * Time.fixedDeltaTime;
        } else {
            rightPS.Stop();
        }

        rb.angularVelocity = new Vector3(0f, 0f, angVelZ);

        if (Input.GetKey(KeyCode.W)) {
            mainPS.Play();
            rb.AddForce(transform.up * force, ForceMode.Acceleration);
        } else {
            mainPS.Stop();
        }

        if (Input.GetKey(KeyCode.S)) {
            stopPSLeft.Play();
            stopPSRight.Play();
            rb.AddForce(-rb.transform.up * force * 0.5f, ForceMode.Acceleration);
        } else {
            stopPSLeft.Stop();
            stopPSRight.Stop();
        }
    }
}
