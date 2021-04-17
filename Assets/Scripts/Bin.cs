using UnityEngine;
using Valve.VR.InteractionSystem;

public class Bin : MonoBehaviour {
	[SerializeField]
	private Player player = null;

	private void OnTriggerEnter(Collider other) {
		// Check player isn't holding the object.
		if (!other.transform.IsChildOf(player.gameObject.transform)) {
			Destroy(other.gameObject);
		}
	}
}
