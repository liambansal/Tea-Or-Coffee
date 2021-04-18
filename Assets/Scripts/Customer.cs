using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public class Customer : MonoBehaviour {
	/// <summary>
	/// Type of drink ordered by the customer.
	/// </summary>
	public string Order { get; private set; } = "";
	/// <summary>
	/// Has the customer been served?
	/// </summary>
	public bool IsServed { get; private set; } = false;

	private static int customersServed = 0;
	private int orderNumber = 0;
	private const float maximumPatienceLeft = 15.0f;
	private float drinkTime = 60.0f;
	private float patienceLeft = 15.0f;

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
	private Player player = null;
	private Coaster coaster = null;

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
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		coaster = GameObject.FindGameObjectWithTag("ServingMat").GetComponent<Coaster>();
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

				const float touchingDistance = 0.1f;

				// If at front of queue.
				if (Vector3.Distance(transform.position, manager.Queue.transform.position) <= touchingDistance) {
					patienceLeft -= Time.deltaTime;

					if (patienceLeft <= 0.0f) {
						LeaveQueue();
						customerState = State.Leaving;
					}

					if (hasOrdered && !IsServed && coaster.Beverage && coaster.Beverage.CompareTag(Order)) {
						TakeDrink(coaster.Beverage);
					}
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
		if (hasOrdered && !IsServed && collider.gameObject.CompareTag(Order) && !collider.transform.IsChildOf(player.gameObject.transform)) {
			TakeDrink(collider.gameObject);
		}

		if (collider.gameObject.CompareTag("Exit")) {
			Destroy(gameObject);
		}
	}

	private void TakeDrink(GameObject mug) {
		heldBeverage = mug;
		// Set the mug's parent to the customer.
		heldBeverage.transform.SetParent(gameObject.transform, true);
		// Change the mug's tag so player/other customers can't interact with it any more.
		heldBeverage.tag = "Delivered";
		customerState = State.Served;
		Pay();
		LeaveQueue();
		Vector3 chairPosition = manager.FindChair().gameObject.transform.position;
		GetComponent<NavMeshAgent>().SetDestination(chairPosition);
	}

	private void LeaveQueue() {
		manager.Queue.RemoveFromQueue(gameObject);
		int customerCounter = 0;

		// Update the remaining customer's queue positions.
		foreach (KeyValuePair<GameObject, Vector3> element in manager.Queue.Queue) {
			element.Key.gameObject.GetComponent<NavMeshAgent>().SetDestination(manager.Queue.GetPosition(element.Key));
			element.Key.gameObject.GetComponent<Customer>().patienceLeft = maximumPatienceLeft + ++customerCounter;
		}
	}

	private void OrderDrink() {
		orderCard = Instantiate(orderCard, Vector3.zero, Quaternion.identity);
		orderCard.GetComponent<OrderCard>().SetOrder(beverageClass.recipes[Order]);
		hasOrdered = true;
	}

	private void Pay() {
		Vector3 servingCoaster = GameObject.FindGameObjectWithTag("ServingMat").transform.position;
		Instantiate(cash, servingCoaster + Vector3.up, Quaternion.identity);
		Text scoreText = GameObject.FindGameObjectWithTag("CustomersServedScore").GetComponent<Text>();
		scoreText.text = (++customersServed).ToString();
	}
}
