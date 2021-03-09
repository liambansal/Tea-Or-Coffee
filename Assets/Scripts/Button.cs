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
	private Dispenser dispenser = null;

	public void OnButtonDown() {
		dispenser.Dispense(itemToSpawn);
	}
}
