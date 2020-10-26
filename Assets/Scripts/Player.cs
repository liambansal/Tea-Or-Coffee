using UnityEngine;

public class Player : MonoBehaviour {
	private string[] interactableObjects = {
		"TeaBag",
		"CoffeeBag",
		"Sugar",
		"BoilingWater",
		"Milk",
		"Mug",
		"Tea",
		"Coffee"
	};

	[SerializeField]
	private GameObject rightController = null;
	private GameObject heldObject = null;

	private void Update() {
		if (heldObject) {
			// Check if our held object has been taken out of our hands.
			if (heldObject.transform.parent.gameObject != rightController) {
				DropObject();
				return;
			}

			// Update our held objects rotation and poisition.
			heldObject.transform.rotation = Quaternion.identity;
			heldObject.transform.position = heldObject.transform.parent.position;
		}

		if (Input.GetKeyDown(KeyCode.E) ||
			Input.GetAxis("Right Controller Trigger (7)") > 0) {
			if (!heldObject) {
				Transform cameraTransform = Camera.main.gameObject.transform;
				RaycastHit raycastHitInfo;
				// Casts a ray forward of the main camera's position.
				Physics.Raycast(cameraTransform.position + cameraTransform.forward,
					cameraTransform.forward,
					out raycastHitInfo);

				// Checks if the ray collided with any interactable objects.
				foreach (string item in interactableObjects) {
					// TODO FIX: causes error when there's nothing to cast a ray against.
					if (raycastHitInfo.collider.CompareTag(item)) {
						PickupObject(raycastHitInfo.collider.gameObject);
					}
				}
			} else if (heldObject) {
				DropObject();
			}
		}
	}

	/// <summary>
	/// Places a gameObject in the right hand of the player.
	/// </summary>
	/// <param name="toPickup"> The gameObject to put in the player's right hand. </param>
	private void PickupObject(GameObject toPickup) {
		heldObject = toPickup;
		heldObject.transform.SetParent(rightController.transform, true);

		if (heldObject.CompareTag("Mug") ||
			heldObject.CompareTag("Tea") ||
			heldObject.CompareTag("Coffee")) {
			// Saves which player is holding the mug. For scoring purposes.
			heldObject.GetComponent<Mug>().PlayerPickedUpMug();
		}
	}

	private void DropObject() {
		heldObject.transform.SetParent(null, true);
		heldObject = null;
	}
}
