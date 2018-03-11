using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour {

	void Start() {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * Random.Range(0.2f, 4f);
    }

}
