using System.Collections.Generic;
using UnityEngine;

public class Mug : MonoBehaviour {
	public struct Ingredient {
		public string name;
		public bool added;
	};

	private string[] ingredients = {
		"TeaBag",
		"Sugar",
		"Water",
		"Milk"
	};

	private enum BREW_STATES {
		EMPTY,
		BREWED
	}

	private BREW_STATES brewState = BREW_STATES.EMPTY;

	private LinkedList<Ingredient> recipeList = new LinkedList<Ingredient>();

	[SerializeField]
	private GameObject mugLiquid = null;
	private GameObject playerHoldingMug;

	private void Awake() {
		// Add each ingredient to the mug's recipe list.
		foreach (string name in ingredients) {
			Ingredient newIngredient = new Ingredient();
			newIngredient.name = name;
			newIngredient.added = false;
			recipeList.AddLast(newIngredient);
		}
	}

	private void OnCollisionEnter(Collision collision) {
		foreach (Ingredient ingredient in recipeList) {
			// Checks for a collision with an ingredient gameObject.
			if (collision.gameObject.CompareTag(ingredient.name)) {
				// Only add the ingredient if it's not present within the mug.
				if (!ingredient.added) {
					AddIngredient(ingredient);
					// Destroy the ingredient.
					Destroy(collision.gameObject);
					UpdateBrewState();
				}
			}
		}
	}

	private void AddIngredient(Ingredient ingredient) {
		// TODO: simplify.
		Ingredient localIngredient = ingredient;
		localIngredient.added = true;
		// Checks off the collided ingredient from the recipe list.
		recipeList.Find(ingredient).Value = localIngredient;
	}

	private void UpdateBrewState() {
		foreach (Ingredient ingredient in recipeList) {
			if (!ingredient.added) {
				brewState = BREW_STATES.EMPTY;
				break;
			} else {
				brewState = BREW_STATES.BREWED;
			}
		}

		if (brewState == BREW_STATES.BREWED) {
			gameObject.tag = "Tea";
			const float yScale = 0.55f;
			// Set a new y scale for the mug's liquid now it's brewed.
			mugLiquid.transform.localScale =
				new Vector3(mugLiquid.transform.localScale.x,
				yScale,
				mugLiquid.transform.localScale.z);
			// TODO: In the future we could grow the mug liquid as ingredients are put in.
		}
	}

	public void PlayerPickedUpMug() {
		// Mug inheritence is as follows: Player > Hand > Mug.
		playerHoldingMug = gameObject.transform.parent.parent.gameObject;
	}
}
