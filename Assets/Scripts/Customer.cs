using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour {
	/// <summary>
	/// Type of drink ordered by the customer.
	/// </summary>
	public string Order { get; private set; } = "";
	/// <summary>
	/// Has the customer been served?
	/// </summary>
	public bool IsServed { get; private set; } = false;

	private int orderNumber = 0;
	private float drinkTime = 40.0f;
	private bool hasOrdered = false;

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
	[SerializeField]
	private GameObject cash = null;
	private GameObject heldBeverage = null;

	private void Awake() {
		manager = GameObject.FindGameObjectWithTag("Customer Manager").GetComponent<CustomerManager>();
	}

	private void Start() {
		sceneExit = GameObject.FindGameObjectWithTag("Exit").GetComponent<Transform>().position;
		beverageClass = GameObject.FindGameObjectWithTag("BeverageClass").GetComponent<Beverages>();
		// Select a semi random drink order for customer.
		orderNumber = Random.Range(0, beverageClass.beverageKeys.Length);
		Order = beverageClass.beverageKeys[orderNumber];

		// Try setting a queue position.
		if (!manager.Queue.AddToQueue(gameObject, false)) {
			customerState = State.Leaving;
			return;
		}

		// Set queue position as target destination.
		GetComponent<NavMeshAgent>().SetDestination(manager.Queue.GetPosition(gameObject));
	}

	/// <summary>
	/// Updates the customer's behaviour state.
	/// </summary>
	private void Update() {
		// Reset the orientation.
		transform.rotation = Quaternion.LookRotation(-Vector3.up, Vector3.forward);

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
				IsServed = true;

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
				GetComponent<NavMeshAgent>().SetDestination(sceneExit);
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
			// Change the mug's tag so player/other customers can't interact with it any more.
			heldBeverage.tag = "Delivered";
			customerState = State.Served;
			Pay();
			manager.Queue.RemoveFromQueue(gameObject);

			// Update the remaining customer's queue positions.
			foreach (KeyValuePair<GameObject, Vector3> element in manager.Queue.Queue) {
				element.Key.gameObject.GetComponent<NavMeshAgent>().SetDestination(manager.Queue.GetPosition(element.Key));
			}

			Vector3 chairPosition = manager.FindChair().gameObject.transform.position;
			GetComponent<NavMeshAgent>().SetDestination(chairPosition);
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

	private void Pay() {
		Vector3 ordservingMat = GameObject.FindGameObjectWithTag("ServingMat").transform.position;
		Instantiate(cash, ordservingMat + Vector3.up, Quaternion.identity);
	}
}
