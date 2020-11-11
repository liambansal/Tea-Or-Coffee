using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour {
	public string Order { get; private set; } = "";
	public bool OpenOrder { get; private set; } = true;

	private int orderNumber = 0;

	private string[] possibleOrder = {
		"Tea",
		"Coffee"
	};

	private Vector3 queuePosition = Vector3.zero;

	private CustomerManager manager = null;

	[SerializeField]
	private Transform exit;

	private GameObject heldObject = null;

	private void Awake() {
		manager = GameObject.FindGameObjectWithTag("Customer Manager").GetComponent<CustomerManager>();
		orderNumber = Random.Range(0, 2);
	}

	private void Start() {
		switch (orderNumber.ToString()) {
			case "zero": {
				Order = possibleOrder[orderNumber];
				break;
			}
			case "one": {
				Order = possibleOrder[orderNumber];
				break;
			}
		}

		if (manager.queue.Find(gameObject).Previous != null) {
			queuePosition =
				manager.queue.Find(gameObject).Previous.Value.GetComponent<Customer>().queuePosition +
				(manager.queueDirection *
				(GetComponent<CapsuleCollider>().radius *
				2));
		} else {
			// Calculate the last position in the queue.
			queuePosition = manager.queueStart.position;
		}

		gameObject.GetComponent<NavMeshAgent>().SetDestination(queuePosition);
	}

	private void Update() {
		if (!OpenOrder) {
			if (heldObject) {
				heldObject.transform.rotation = Quaternion.identity;
				heldObject.transform.position = transform.position + transform.forward;
			}

			gameObject.GetComponent<NavMeshAgent>().SetDestination(exit.position);
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (OpenOrder &&
			(other.gameObject.CompareTag("Tea") ||
			other.gameObject.CompareTag("Coffee"))) {
			GameObject player = GameObject.FindGameObjectWithTag("Red");
			// Update the player's score.
			GameObject.FindGameObjectWithTag(player.tag + " Score").GetComponent<Score>().IncrementScore();
			heldObject = other.gameObject;
			// Set the mug's parent to the customer.
			heldObject.transform.SetParent(gameObject.transform, true);
			OpenOrder = false;
			// Change the mug's tag so players can't interact with it after it delivered to a 
			// customer.
			heldObject.tag = "Delivered";
		}
	}
}
