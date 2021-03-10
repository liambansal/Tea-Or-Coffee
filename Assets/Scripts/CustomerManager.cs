using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour {
	public LinkedList<GameObject> Queue { get; private set; } = new LinkedList<GameObject>();
	public Transform QueueStart { get; private set; } = null;

	private int spawnTime = 0;
	private int lastSpawnTime = 0;
	private int maxCustomers = 20;
	private int maximumQueuePositions = 5;
	/// <summary>
	/// Distance in between queue positions.
	/// </summary>
	private int queueGap = 3;

	private Vector3 queueDirection = Vector3.zero;

	private LinkedList<GameObject> customers = new LinkedList<GameObject>();
	private LinkedList<GameObject> emptyChairs = new LinkedList<GameObject>();
	private LinkedList<GameObject> occupiedChairs = new LinkedList<GameObject>();

	private Clock clock = null;
	private GameStateManager gameStateManager = null;
	
	[SerializeField]
	private Transform spawn = null;

	[SerializeField]
	private GameObject customerPrefab = null;

	/// <summary>
	/// Adds a customer to the queue.
	/// </summary>
	public bool AddToQueue(Customer customer) {
		if (Queue.Count > maximumQueuePositions) {
			return false;
		}

		Queue.AddLast(customer.gameObject);
		return true;
	}

	public void RemoveFromQueue(GameObject customer) {
		if (Queue.Contains(customer)) {
			Queue.Remove(customer);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <returns> Returns an empty position in the queue or Vector3.zero if there's no room. </returns>
	public Vector3 UpdateQueuePosition(Customer customer) {
		Vector3 queuePosition = Vector3.zero;

		// Check customer is at front of queue.
		if (Queue.Count > 1 && Queue.Find(customer.gameObject).Previous != null) {
			queuePosition = Queue.Find(customer.gameObject).Previous.Value.GetComponent<Customer>().queuePosition + queueDirection * queueGap;
		} else {
			// Get first position in queue.
			queuePosition = QueueStart.position;
		}

		return queuePosition;
	}

	public GameObject FindChair() {
		GameObject emptyChair = emptyChairs.First.Value;
		// Remove chair from empty list.
		emptyChairs.Remove(emptyChair);
		// Add chair to occupied list.
		occupiedChairs.AddLast(emptyChair);
		return occupiedChairs.Last.Value;
	}

	private void Start() {
		clock = GameObject.FindGameObjectWithTag("Clock").GetComponent<Clock>();
		gameStateManager = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateManager>();
		QueueStart = GameObject.FindGameObjectWithTag("QueueStart").transform;
		GameObject[] chairs = GameObject.FindGameObjectsWithTag("Chair");

		for (int i = 0; i < chairs.Length; ++i) {
			emptyChairs.AddLast(chairs[i]);
		}

		queueDirection = (-QueueStart.forward);
		spawnTime = clock.MaxLength / maxCustomers;
	}

	private void Update() {
		if (gameStateManager.GameState == GameStateManager.GameStates.WaitingToStart) {
			// Wait until player presses button to start game.
			return;
		}

		// Check a customer hasn't already spawned at this time and their spawn time has elapsed.
		if (lastSpawnTime != (int)clock.CurrentTime && (int)clock.CurrentTime % spawnTime == 0) {
			// Spawn customer and add them to the queue.
			customers.AddLast(Instantiate(customerPrefab, spawn.position, Quaternion.identity));
			lastSpawnTime = (int)clock.CurrentTime;
		}
	}
}
