using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour, IButton {
	[SerializeField]
	private Transform spawnPoint = null;

	[SerializeField]
	public GameObject[] items = null;

	private Dictionary<string, GameObject> itemsToSpawn = new Dictionary<string, GameObject>();

	private void Start() {
		// Add all spawnable items to dictionary.
		for (int i = 0; i < items.Length; ++i) {
			itemsToSpawn.Add(items[i].tag, items[i]);
		}
	}

	/// <summary>
	/// Spawns an item.
	/// </summary>
	public void ButtonPressed(Button caller) {
		// Check IButton is implemented in object that called here.
		if (caller.gameObject.GetComponent<IButton>() != null) {
			// Make sure the item to spawn exists.
			if (itemsToSpawn.ContainsKey(caller.tag)) {
				Instantiate(itemsToSpawn[caller.tag], spawnPoint.position, Quaternion.LookRotation(Vector3.up, Vector3.forward));
			}
		}
	}
}
