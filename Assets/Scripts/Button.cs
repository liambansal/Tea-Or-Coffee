using UnityEngine;

/// <summary>
/// Pressable button for VR hand interaction. Execute command on press.
/// </summary>
public class Button : MonoBehaviour {
	[SerializeField]
	private string itemToSpawn = "";

	/// <summary>
	/// Tube to spawn an item.
	/// </summary>
	[SerializeField]
	private Dispenser itemTube = null;

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Debug.Log("Press");
			itemTube.Dispense(itemToSpawn);
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Hand")) {
			Debug.Log("Press");
			itemTube.Dispense(itemToSpawn);
		}
	}
}
