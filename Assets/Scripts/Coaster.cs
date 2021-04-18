using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Recognizes a mug being put down and lifted up.
/// </summary>
public class Coaster : MonoBehaviour {
	/// <summary>
	/// Drink placed on the coaster.
	/// </summary>
	public GameObject Beverage { get; private set; } = null;
	
	private Player player = null;

	/// <summary>
	/// Finds the VR player gameObject.
	/// </summary>
	private void Start() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	/// <summary>
	/// May set the placed beverage.
	/// </summary>
	/// <param name="other"> Colliding collider. </param>
	private void OnTriggerStay(Collider other) {
		if (other.GetComponent<Mug>() && !other.transform.IsChildOf(player.transform)) {
			Beverage = other.gameObject;
		}
	}

	/// <summary>
	/// May remove the placed beverage.
	/// </summary>
	/// <param name="other"> Colliding collider. </param>
	private void OnTriggerExit(Collider other) {
		if (Beverage && other.gameObject == Beverage) {
			Beverage = null;
		}
	}
}
