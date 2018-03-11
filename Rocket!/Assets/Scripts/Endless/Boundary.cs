using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour {

	void OnTriggerExit(Collider other) {
		Destroy (other.gameObject);
	}
}
