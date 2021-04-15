using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderCard : MonoBehaviour {
	private bool isQueued = false;
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

	public void SetOrder(Beverages.Beverage setBeverage) {
		beverage = setBeverage;
		beverageImage.sprite = beverage.image;

		// Spawn images for beverage ingredients.
		for (int i = 0; i < beverage.ingredients.Length; ++i) {
			ingredientImage.sprite = beverage.ingredients[i].image;
			Instantiate(ingredientImage, ingredientList.transform);
		}
	}

	private void Start() {
		queue = GameObject.FindGameObjectWithTag("CardQueue").GetComponent<ObjectQueue>();

		if (queue.AddToQueue(gameObject)) {
			transform.position = queue.GetPosition(gameObject);
			isQueued = true;
		} else {
			// Place card out of view from player.
			transform.position = new Vector3(13.0f, -5.0f, 0.0f);
		}
	}

	private void Update() {
		// Check if player has picked up the card.
		if (!touchedByPlayer && gameObject.transform.parent != null) {
			touchedByPlayer = true;
			isQueued = false;
			queue.RemoveFromQueue(gameObject);

			// Update queue positions for remaining queue elements.
			foreach (KeyValuePair<GameObject, Vector3> element in queue.Queue) {
				element.Key.gameObject.transform.position = queue.GetPosition(element.Key);
			}
		}

		if (isQueued) {
			transform.position = queue.GetPosition(gameObject);
			GetComponent<Rigidbody>().Sleep();
		}
	}
}
