using UnityEngine;

public class Customer : MonoBehaviour {
	public string Order { get; private set; }
	public bool OpenOrder { get; private set; } = true;

	private int orderNumber = 0;

	private string[] possibleOrder = {
		"Tea",
		"Coffee"
	};

	private GameObject heldObject = null;

	private void Awake() {
		orderNumber = Random.Range(0, 2);
	}

	private void Start() {
		switch (orderNumber.ToString()) {
			case "zero": {
				Order = possibleOrder[orderNumber];
				break;
			}
			case "one": {
				Order = possibleOrder[orderNumber];
				break;
			}
		}
	}

	private void Update() {
		if (heldObject) {
			heldObject.transform.rotation = Quaternion.identity;
			heldObject.transform.position = transform.position + transform.forward;
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (OpenOrder &&
			(other.gameObject.CompareTag("Tea") ||
			other.gameObject.CompareTag("Coffee"))) {
			GameObject player = other.gameObject.transform.parent.parent.gameObject;
			// Update the player's score.
			GameObject.FindGameObjectWithTag(player.tag + " Score").GetComponent<Score>().IncrementScore();
			heldObject = other.gameObject;
			// Set the mug's parent to the customer.
			heldObject.transform.SetParent(gameObject.transform, true);
			OpenOrder = false;
			// Change the mug's tag so players can't interact with it after it delivered to a 
			// customer.
			heldObject.tag = "Delivered";
		}
	}
}
