using UnityEngine;
using Valve.VR.InteractionSystem;

public class Coaster : MonoBehaviour {
	public GameObject Beverage { get; private set; } = null;
	
	private Player player = null;

	private void Start() {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	private void OnTriggerStay(Collider other) {
		if (other.GetComponent<Mug>() && !other.transform.IsChildOf(player.transform)) {
			Beverage = other.gameObject;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (Beverage && other.gameObject == Beverage) {
			Beverage = null;
		}
	}
}
