using UnityEngine;
using Valve.VR.InteractionSystem;

public class Bin : MonoBehaviour {
	[SerializeField]
	private Player player = null;

	/// <summary>
	/// Destroys the colliding object.
	/// </summary>
	/// <param name="other"> Collider from object to destroy. </param>
	private void OnTriggerEnter(Collider other) {
		// Check player isn't holding the object.
		if (!other.transform.IsChildOf(player.gameObject.transform)) {
			Destroy(other.gameObject);
		}
	}
}
