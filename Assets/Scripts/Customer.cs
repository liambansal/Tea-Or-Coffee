using UnityEngine;
using UnityEngine.AI;

public class Customer : CustomerManager {
	public string Order { get; private set; } = "";
	public bool OpenOrder { get; private set; } = true;

	private int orderNumber = 0;

	/// <summary>
	/// The diameter/longest length of a customer's collider.
	/// </summary>
	[SerializeField]
	private float customerLength = 1.0f;
	/// <summary>
	/// Amount of space between customers.
	/// </summary>
	[SerializeField]
	private float margin = 1.0f;

	private bool spawnedCard = false;

	private string[] possibleOrder = {
		"Tea",
		"Coffee"
	};

	private Vector3 queuePosition = Vector3.zero;
	private Vector3 sceneExit = Vector3.zero;

	private CustomerManager manager = null;

	[SerializeField]
	private GameObject orderCard = null;
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

		SetQueuePosition();
		sceneExit = GameObject.FindGameObjectWithTag("Exit").GetComponent<Transform>().position;
	}

	private void Update() {
		if (!OpenOrder) {
			if (heldObject) {
				heldObject.transform.rotation = Quaternion.identity;
				heldObject.transform.position = transform.position + transform.forward;
			}

			gameObject.GetComponent<NavMeshAgent>().SetDestination(sceneExit);
		} else {
			if (!spawnedCard && 
				queuePosition == manager.QueueStart.position &&
				transform.position.x == GetComponent<NavMeshAgent>().destination.x &&
				transform.position.z == GetComponent<NavMeshAgent>().destination.z) {
				orderCard.GetComponent<OrderCard>().SetOrder(Beverages.beverages["Tea"]);
				float forwardDistance = 1.5f;
				orderCard = Instantiate(orderCard, transform.position + transform.forward * forwardDistance, Quaternion.identity);
				spawnedCard = true;
			}
		}
	}

	private void OnTriggerEnter(Collider collider) {
		if (OpenOrder &&
			(collider.gameObject.CompareTag("Tea") ||
			collider.gameObject.CompareTag("Coffee"))) {
			GameObject player = GameObject.FindGameObjectWithTag("Red");
			// Update the player's score.
			GameObject.FindGameObjectWithTag(player.tag + " Score").GetComponent<Score>().IncrementScore();
			heldObject = collider.gameObject;
			// Set the mug's parent to the customer.
			heldObject.transform.SetParent(gameObject.transform, true);
			OpenOrder = false;
			// Change the mug's tag so players can't interact with it after it delivered to a 
			// customer.
			heldObject.tag = "Delivered";
		}

		if (collider.gameObject.CompareTag("Exit")) {
			if (Queue.Contains(gameObject)) {
				UpdateQueue();
			}

			Destroy(orderCard);
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Sets a queue position for the customer and set's it as their target destination.
	/// </summary>
	public void SetQueuePosition() {
		if (Queue.Find(gameObject).Previous != null) {
			queuePosition =
				Queue.Find(gameObject).Previous.Value.GetComponent<Customer>().queuePosition +
				manager.queueDirection *
				(customerLength +
				margin);
		} else {
			// Calculate the last position in the queue.
			queuePosition = manager.QueueStart.position;
		}

		gameObject.GetComponent<NavMeshAgent>().SetDestination(queuePosition);
	}
}
