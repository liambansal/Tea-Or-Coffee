using System.Collections.Generic;
using UnityEngine;

public class Mug : MonoBehaviour {
	private enum BREW_STATES {
		EMPTY,
		BREWED,
		SPOILED,
		COUNT
	} BREW_STATES brewState = BREW_STATES.EMPTY;

	private Beverages.Beverage beverage = new Beverages.Beverage();

	[SerializeField]
	private GameObject mugLiquid = null;

	private void Start() {
		// Create unknown beverage.
		beverage.name = "Unknown";
		beverage.ingredients = new Beverages.Ingredient[Beverages.ingredientList.Length];
		Beverages.ingredientList.CopyTo(beverage.ingredients, 0);
		beverage.image = null;
		beverage.ingredients[0].count = 0;
		beverage.ingredients[1].count = 0;
		beverage.ingredients[2].count = 0;
		beverage.ingredients[3].count = 0;
		beverage.ingredients[4].count = 0;
	}

	private void Update() {
		if (brewState == BREW_STATES.BREWED) {
			gameObject.tag = beverage.name;
			const float zStart = 0.0f;
			const float zEnd = 1.1f;
			// Slowly raises the level of liquid within the mug. Z is pointing up.
			mugLiquid.transform.localPosition =
				new Vector3(mugLiquid.transform.localPosition.x,
				mugLiquid.transform.localPosition.y,
				Mathf.Lerp(zStart, zEnd, mugLiquid.transform.localPosition.z + Time.deltaTime));
		}
	}

	private void OnTriggerEnter(Collider collision) {
		// TODO: Dont let tea bags be added if coffee if present and vise versa.
		foreach (Beverages.Ingredient ingredient in beverage.ingredients) {
			// Checks for a collision with an ingredient gameObject.
			if (collision.gameObject.CompareTag(ingredient.name)) {
				AddIngredient(ingredient);
				// Destroy the ingredients parent.
				Destroy(collision.gameObject.transform.parent.gameObject);
				UpdateBrewState();
			}
		}
	}

	private void AddIngredient(Beverages.Ingredient ingredient) {
		ingredient.isAdded = true;
		++ingredient.count;

		for (int i = 0; i < beverage.ingredients.Length; ++i) {
			if (beverage.ingredients[i].name == ingredient.name) {
				// Checks off the collided ingredient from the recipe list.
				beverage.ingredients[i] = ingredient;
				return;
			}
		}
	}

	private void UpdateBrewState() {
		// If player has added enough ingredients for multiple beverages to be brewed 
		// we need to decide which one to use.
		// Keeps track of possible beverages to be brewed.
		LinkedList<Beverages.Beverage> brewedBeverages = new LinkedList<Beverages.Beverage>();

		// Loops through all beverages that can be made.
		for (int i = 0; i < Beverages.beverageKeys.Length; ++i) {
			// Temporary variable to store possible beverage.
			Beverages.Beverage possibleBeverage = Beverages.beverages[Beverages.beverageKeys[i]];
			CheckAddedInrgedients(ref possibleBeverage);

			if (brewState == BREW_STATES.BREWED) {
				brewedBeverages.AddLast(possibleBeverage);
			}
		}

		if (brewedBeverages.Count > 0) {
			int largestBrew = 0;
			LinkedListNode<Beverages.Beverage> node = brewedBeverages.First;
			Beverages.Beverage brewedBeverage = new Beverages.Beverage();

			// Get the beverage with the most ingredients.
			for (int j = 0; j < brewedBeverages.Count; ++j, node = node.Next) {
				if (node.Value.ingredients.Length > largestBrew) {
					brewedBeverage = node.Value;
					largestBrew = node.Value.ingredients.Length;
				}
			}

			// Identify what the player's making.
			beverage.name = brewedBeverage.name;
			gameObject.tag = brewedBeverage.name;
			beverage.image = brewedBeverage.image;
			mugLiquid.GetComponent<Renderer>().material.color = brewedBeverage.material.color;
			brewState = BREW_STATES.BREWED;
		}
		
		if (brewState == BREW_STATES.SPOILED) {
			beverage.name = "Spoiled";
			gameObject.tag = "Spoiled";
			beverage.image = null;
			mugLiquid.GetComponent<Renderer>().material.color = Color.black;
		}
	}

	private void CheckAddedInrgedients(ref Beverages.Beverage possibleBeverage) {
		foreach (Beverages.Ingredient ingredient in beverage.ingredients) {
			for (int j = 0; j < possibleBeverage.ingredients.Length; ++j) {
				if (ingredient.name == possibleBeverage.ingredients[j].name) {
					if (ingredient.isAdded && (ingredient.count == possibleBeverage.ingredients[j].count)) {
						brewState = BREW_STATES.BREWED;
						break;
					} else if (ingredient.isAdded && (ingredient.count != 0)) {
						brewState = BREW_STATES.SPOILED;
						return;
					} else if (!ingredient.isAdded && (ingredient.count == 0)) {
						brewState = BREW_STATES.EMPTY;
						return;
					}
				}
			}
		}
	}
}
