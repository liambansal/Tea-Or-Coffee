using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour {
	public string Order { get; private set; } = "";
	public bool OpenOrder { get; private set; } = false;
	public Vector3 queuePosition { get; private set; } = Vector3.zero;

	private int orderNumber = 0;
	private float drinkTime = 8;
	private bool spawnedCard = false;
	private string[] possibleOrder = {
		"Tea",
		"Coffee",
		"BuildersBrew"
	};

	private Vector3 sceneExit = Vector3.zero;

	private enum State {
		Roaming,
		Ordering,
		Served,
		Leaving
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
		orderNumber = Random.Range(0, possibleOrder.Length);
		Order = possibleOrder[orderNumber];
	}

	private void Update() {
		switch (customerState) {
			case State.Roaming: {
				if (queuePosition == Vector3.zero) {
					if (!manager.AddToQueue(this)) {
						// Don't enter cafe since queue is full.
						customerState = State.Leaving;
					}

					queuePosition = manager.UpdateQueuePosition(this);
					gameObject.GetComponent<NavMeshAgent>().SetDestination(queuePosition);
				}

				// Check customer is first in queue and at target destination.
				if (queuePosition == manager.QueueStart.position &&
					Mathf.Approximately(transform.position.x, GetComponent<NavMeshAgent>().destination.x) &&
					Mathf.Approximately(transform.position.z, GetComponent<NavMeshAgent>().destination.z)) {
					customerState = State.Ordering;
				}

				break;
			}
			case State.Ordering: {
				if (!spawnedCard) {
					float forwardDistance = 1.5f;
					orderCard = Instantiate(orderCard, transform.position + Vector3.right * forwardDistance, Quaternion.identity);
					orderCard.GetComponent<OrderCard>().SetOrder(beverageClass.recipes[Order]);
					spawnedCard = true;
					OpenOrder = true;
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
		if (OpenOrder &&
			(collider.gameObject.CompareTag("Tea") ||
			collider.gameObject.CompareTag("Coffee") ||
			collider.gameObject.CompareTag("BuildersBrew"))) {
			heldBeverage = collider.gameObject;
			// Set the mug's parent to the customer.
			heldBeverage.transform.SetParent(gameObject.transform, true);
			OpenOrder = false;
			// Change the mug's tag so players can't interact with it after it delivered to a 
			// customer.
			heldBeverage.tag = "Delivered";
			customerState = State.Served;
			Vector3 chairPosition = manager.FindChair().gameObject.transform.position;
			// Get chair position.
			gameObject.GetComponent<NavMeshAgent>().SetDestination(chairPosition);
			manager.RemoveFromQueue(gameObject);
			LinkedListNode<GameObject> iterator = manager.Queue.First;

			for (int i = 0; i < manager.Queue.Count; ++i, iterator = iterator.Next) {
				/// Update the remaining customers positions.
				iterator.Value.GetComponent<Customer>().queuePosition = manager.UpdateQueuePosition(iterator.Value.GetComponent<Customer>());
				iterator.Value.GetComponent<NavMeshAgent>().SetDestination(iterator.Value.GetComponent<Customer>().queuePosition);
			}
		}

		if (collider.gameObject.CompareTag("Exit")) {
			if (spawnedCard) {
				Destroy(orderCard);
			}

			Destroy(gameObject);
		}
	}
}
