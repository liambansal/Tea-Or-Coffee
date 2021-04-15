using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour {
	/// <summary>
	/// Type of drink ordered by the customer.
	/// </summary>
	public string Order { get; private set; } = "";
	public bool IsServed { get; private set; } = false;

	private int orderNumber = 0;
	private float drinkTime = 40.0f;
	private bool hasOrdered = false;
	private string[] possibleOrder = {
		"Tea",
		"Coffee",
		"BuildersBrew"
	};

	private Vector3 sceneExit = Vector3.zero;

	private enum State {
		Roaming,
		Waiting,
		Served,
		Leaving,
		Count
	}
	private State customerState = State.Roaming;

	private CustomerManager manager = null;
	private Beverages beverageClass = null;

	[SerializeField]
	private GameObject orderCard = null;
	private GameObject heldBeverage = null;

	private void Awake() {
		manager = GameObject.FindGameObjectWithTag("Customer Manager").GetComponent<CustomerManager>();
	}

	private void Start() {
		sceneExit = GameObject.FindGameObjectWithTag("Exit").GetComponent<Transform>().position;
		beverageClass = GameObject.FindGameObjectWithTag("BeverageClass").GetComponent<Beverages>();
		// Select a semi random drink order for customer.
		orderNumber = Random.Range(0, possibleOrder.Length);
		Order = possibleOrder[orderNumber];

		// Try setting a queue position.
		if (!manager.Queue.AddToQueue(gameObject, false)) {
			customerState = State.Leaving;
			return;
		}

		// Set queue position as target destination.
		gameObject.GetComponent<NavMeshAgent>().SetDestination(manager.Queue.GetPosition(gameObject));
	}

	/// <summary>
	/// Updates the customer's behaviour state.
	/// </summary>
	private void Update() {
		switch (customerState) {
			case State.Roaming: {
				// check if queueing
				if (manager.Queue.Queue.ContainsKey(gameObject)) {
					const float touchingDistance = 0.1f;

					if (GetComponent<NavMeshAgent>().remainingDistance <= touchingDistance) {
						customerState = State.Waiting;
					}
				}

				break;
			}
			case State.Waiting: {
				if (!hasOrdered) {
					OrderDrink();
				}

				break;
			}
			case State.Served: {
				if (heldBeverage) {
					heldBeverage.transform.rotation = Quaternion.identity;
					heldBeverage.transform.position = transform.position + transform.forward;
					drinkTime -= Time.deltaTime;

					if (drinkTime <= 0.0f) {
						customerState = State.Leaving;
					}
				}

				break;
			}
			case State.Leaving: {
				gameObject.GetComponent<NavMeshAgent>().SetDestination(sceneExit);
				break;
			}
			default: {
				break;
			}
		}
	}

	private void OnTriggerEnter(Collider collider) {
		if (hasOrdered && !IsServed && collider.gameObject.CompareTag(Order)) {
			heldBeverage = collider.gameObject;
			// Set the mug's parent to the customer.
			heldBeverage.transform.SetParent(gameObject.transform, true);
			IsServed = true;
			// Change the mug's tag so players can't interact with it after it delivered to a 
			// customer.
			heldBeverage.tag = "Delivered";
			customerState = State.Served;
			manager.Queue.RemoveFromQueue(gameObject);

			// Update the remaining customers positions.
			foreach (KeyValuePair<GameObject, Vector3> element in manager.Queue.Queue) {
				element.Key.gameObject.GetComponent<NavMeshAgent>().SetDestination(manager.Queue.GetPosition(element.Key));
			}

			Vector3 chairPosition = manager.FindChair().gameObject.transform.position;
			// Get chair position.
			gameObject.GetComponent<NavMeshAgent>().SetDestination(chairPosition);
		}

		if (collider.gameObject.CompareTag("Exit")) {
			Destroy(gameObject);
		}
	}

	private void OrderDrink() {
		orderCard = Instantiate(orderCard, Vector3.zero, Quaternion.identity);
		orderCard.GetComponent<OrderCard>().SetOrder(beverageClass.recipes[Order]);
		hasOrdered = true;
	}
}
