using UnityEngine;

public class Road : MonoBehaviour {
	[SerializeField]
	private int movementSpeed = 8;

	private Rigidbody rb = null;

	private GameObject roadStart = null;

	private void Awake() {
		rb = GetComponent<Rigidbody>();
	}

	private void Start() {
		roadStart = GameObject.FindGameObjectWithTag("RoadStart");
		rb.AddForce(Vector3.forward * movementSpeed, ForceMode.VelocityChange);
	}

	private void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.CompareTag("RoadEnd")) {
			transform.position = roadStart.transform.position;
		}
	}
}
