using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour {
	[SerializeField]
	private Transform spawnPoint = null;

	[SerializeField]
	public GameObject[] items = null;

	private Dictionary<string, GameObject> itemsToSpawn = new Dictionary<string, GameObject>();

	private void Start() {
		// Add all spawnable items to dictionary.
		for (int i = 0; i < items.Length; ++i) {
			itemsToSpawn.Add(items[i].name, items[i]);
		}
	}

	/// <summary>
	/// Spawns an item.
	/// </summary>
	public void Dispense(string itemToSpawn) {
		Instantiate(itemsToSpawn[itemToSpawn], spawnPoint.position, Quaternion.LookRotation(Vector3.up, Vector3.forward));
	}
}
