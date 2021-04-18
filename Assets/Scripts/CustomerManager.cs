using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour {
	public ObjectQueue Queue { get { return queue; } private set { } }
	public Transform QueueStart { get; private set; } = null;

	private int spawnTime = 0;
	private int lastSpawnTime = 0;
	private const int maxCustomers = 35;

	private LinkedList<GameObject> customers = new LinkedList<GameObject>();
	private LinkedList<GameObject> emptyChairs = new LinkedList<GameObject>();
	private LinkedList<GameObject> occupiedChairs = new LinkedList<GameObject>();

	private Clock clock = null;
	private GameStateManager gameStateManager = null;
	[SerializeField]
	private ObjectQueue queue = null;
	
	[SerializeField]
	private Transform spawn = null;

	[SerializeField]
	private GameObject customerPrefab = null;

	public GameObject FindChair() {
		GameObject emptyChair = emptyChairs.First.Value;
		emptyChairs.Remove(emptyChair);
		occupiedChairs.AddLast(emptyChair);
		return occupiedChairs.Last.Value;
	}

	private void Start() {
		clock = GameObject.FindGameObjectWithTag("Clock").GetComponent<Clock>();
		gameStateManager = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateManager>();
		QueueStart = Queue.transform;
		GameObject[] chairs = GameObject.FindGameObjectsWithTag("Chair");

		for (int i = 0; i < chairs.Length; ++i) {
			emptyChairs.AddLast(chairs[i]);
		}

		spawnTime = clock.MaxLength / maxCustomers;
	}

	private void Update() {
		if (gameStateManager.GameState == GameStateManager.GameStates.WaitingToStart) {
			// Wait until player presses button to start game.
			return;
		}

		// Check a customer hasn't already spawned at this time and their spawn time has elapsed.
		if (spawnTime != 0 && lastSpawnTime != (int)clock.CurrentTime && (int)clock.CurrentTime % spawnTime == 0) {
			// Spawn customer and add them to the queue.
			customers.AddLast(Instantiate(customerPrefab, spawn.position, Quaternion.identity));
			lastSpawnTime = (int)clock.CurrentTime;
		}
	}
}
