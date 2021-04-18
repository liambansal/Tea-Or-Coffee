using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents the beverage ordered by a customer.
/// </summary>
public class OrderCard : MonoBehaviour {
	public Vector3 QueuePosition {
		get {
			Vector3 queuePosition = Vector3.zero;
			queue.Queue.TryGetValue(gameObject, out queuePosition);
			return queuePosition;
		}
		private set {
		}
	}

	/// <summary>
	/// Has the layer touched this gameobject?
	/// </summary>
	private bool touchedByPlayer = false;

	[SerializeField]
	private GameObject ingredientList = null;

	[SerializeField]
	private Image beverageImage = null;
	[SerializeField]
	private Image ingredientImage = null;

	private ObjectQueue queue = null;
	private Beverages.Beverage beverage;

	/// <summary>
	/// Chooses a beverage for the card to display.
	/// </summary>
	/// <param name="setBeverage"> Beverage to display. </param>
	public void SetOrder(Beverages.Beverage setBeverage) {
		beverage = setBeverage;
		beverageImage.sprite = beverage.image;

		// Spawn images for beverage ingredients.
		for (int i = 0; i < beverage.ingredients.Length; ++i) {
			ingredientImage.sprite = beverage.ingredients[i].image;
			Instantiate(ingredientImage, ingredientList.transform);
		}
	}

	/// <summary>
	/// Adds the card to the card queue.
	/// </summary>
	private void Start() {
		queue = GameObject.FindGameObjectWithTag("CardQueue").GetComponent<ObjectQueue>();

		if (queue.AddToQueue(gameObject, true)) {
			transform.position = QueuePosition;
		}
	}

	/// <summary>
	/// Handles keeping/removing the card in/from the queue.
	/// </summary>
	private void Update() {
		// Check if player has picked up the card. Statement triggers once.
		if (!touchedByPlayer && gameObject.transform.parent != null) {
			touchedByPlayer = true;
			queue.RemoveFromQueue(gameObject);

			// Update queue positions for remaining queue elements.
			foreach (KeyValuePair<GameObject, Vector3> element in queue.Queue) {
				element.Key.gameObject.transform.position = element.Key.GetComponent<OrderCard>().QueuePosition;
			}
		}

		if (queue.Queue.ContainsKey(gameObject)) {
			transform.position = QueuePosition;
			transform.rotation = Quaternion.identity;
			GetComponent<Rigidbody>().Sleep();
		}
	}
}
