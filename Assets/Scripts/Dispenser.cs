using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates an object selected from a collection.
/// </summary>
public class Dispenser : MonoBehaviour, IButton {
	[SerializeField]
	public GameObject[] items = null;

	[SerializeField]
	private Transform spawnPoint = null;

	private Dictionary<string, GameObject> itemsToSpawn = new Dictionary<string, GameObject>();

	/// <summary>
	/// Spawns an items based on the calling button's tag.
	/// </summary>
	/// <param name="callingButton"> Button calling this. </param>
	public void ButtonPressed(Button callingButton) {
		// Check IButton is implemented in object that called here.
		if (callingButton.GetComponent<IButton>() != null) {
			if (itemsToSpawn.ContainsKey(callingButton.tag)) {
				Instantiate(itemsToSpawn[callingButton.tag], spawnPoint.position, Quaternion.LookRotation(Vector3.up, Vector3.forward));
			}
		}
	}

	/// <summary>
	/// Populate dictionary with items to dispense.
	/// </summary>
	private void Start() {
		for (int i = 0; i < items.Length; ++i) {
			itemsToSpawn.Add(items[i].tag, items[i]);
		}
	}
}
