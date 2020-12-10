using UnityEngine;

public class Player : MonoBehaviour {
	private float canPickupTimer = 0.0f;
	private bool canPickup = true;
	private string[] interactableObjects = {
		"TeaBag",
		"CoffeeTin",
		"Sugar",
		"Water",
		"Milk",
		"Mug",
		"Tea",
		"Coffee",
		"Order Card"
	};

	[SerializeField]
	private GameObject objectHoldPosition = null;
	private GameObject heldObject = null;
	private GameObject heldObjectParent = null;

	private void Update() {
		if (!canPickup && canPickupTimer > 0.0f) {
			canPickupTimer -= Time.deltaTime;
		} else if (canPickupTimer <= 0.0f) {
			canPickup = true;
			canPickupTimer = 0.2f;
		}

		if (heldObject) {
			// Check if our held object has been taken out of our hands.
			if (heldObject.transform.parent.gameObject != objectHoldPosition) {
				DropObject();
				return;
			}

			// Update our held objects rotation and poisition.
			heldObject.transform.rotation = Quaternion.identity;
			heldObject.transform.position = heldObject.transform.parent.position;
			heldObjectParent.transform.position = heldObject.transform.parent.position;
		}

		if (Input.GetKeyDown(KeyCode.E)) {
			if (canPickup && !heldObject) {
				canPickup = false;
				Transform cameraTransform = Camera.main.gameObject.transform;
				RaycastHit raycastHitInfo;
				// Casts a ray forward of the main camera's position.
				Physics.Raycast(cameraTransform.position + cameraTransform.forward,
					cameraTransform.forward,
					out raycastHitInfo);

				// Checks if the ray collided with any interactable objects.
				foreach (string item in interactableObjects) {
					if (raycastHitInfo.collider && raycastHitInfo.collider.CompareTag(item)) {
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
		heldObjectParent = heldObject.transform.parent.gameObject;
		heldObject.transform.SetParent(objectHoldPosition.transform);
	}

	private void DropObject() {
		heldObjectParent.GetComponent<Rigidbody>().velocity = Vector3.zero;
		heldObjectParent.transform.position = heldObject.transform.position;
		heldObject.transform.SetParent(heldObjectParent.transform);
		heldObject = null;
	}
}
