using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns customers and handles some miscellaneous customer related tasks.
/// </summary>
public class CustomerManager : MonoBehaviour {
	/// <summary>
	/// Queue to order customers.
	/// </summary>
	public ObjectQueue Queue { get { return queue; } private set { } }

	/// <summary>
	/// Maximum customers to spawn in one game session.
	/// </summary>
	private const int maxCustomers = 35;
	private int spawnTime = 0;
	private int lastSpawnTimestamp = 0;

	/// <summary>
	/// The scene's customers.
	/// </summary>
	private LinkedList<GameObject> customers = new LinkedList<GameObject>();
	private LinkedList<GameObject> emptyChairs = new LinkedList<GameObject>();
	private LinkedList<GameObject> occupiedChairs = new LinkedList<GameObject>();

	private Clock clock = null;
	private GameStateManager gameStateManager = null;
	/// <summary>
	/// Queue to order customers.
	/// </summary>
	[SerializeField]
	private ObjectQueue queue = null;
	
	/// <summary>
	/// Spawn point for customers.
	/// </summary>
	[SerializeField]
	private Transform spawn = null;

	[SerializeField]
	private GameObject customerPrefab = null;

	public GameObject FindChair() {
		if (emptyChairs.Count <= 0) {
			return null;
		}

		GameObject emptyChair = emptyChairs.First.Value;
		emptyChairs.Remove(emptyChair);
		occupiedChairs.AddLast(emptyChair);
		return emptyChair;
	}

	/// <summary>
	/// Finds some gameObjects and sets the spawn time.
	/// </summary>
	private void Start() {
		clock = GameObject.FindGameObjectWithTag("Clock").GetComponent<Clock>();
		gameStateManager = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateManager>();
		GameObject[] chairs = GameObject.FindGameObjectsWithTag("Chair");

		foreach (GameObject chair in chairs) {
			emptyChairs.AddLast(chair);
		}

		spawnTime = clock.MaxLength / maxCustomers;
	}

	private void Update() {
		if (gameStateManager.GameState == GameStateManager.GameStates.WaitingToStart) {
			// Wait until player presses button to start game.
			return;
		}

		// Check a customer hasn't already spawned at this time and the spawn time has elapsed.
		if (spawnTime != 0 && lastSpawnTimestamp != (int)clock.CurrentTime && (int)clock.CurrentTime % spawnTime == 0) {
			customers.AddLast(Instantiate(customerPrefab, spawn.position, Quaternion.identity));
			lastSpawnTimestamp = (int)clock.CurrentTime;
		}
	}
}
