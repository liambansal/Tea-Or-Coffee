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

	private void Update() {
		if (brewState == BREW_STATES.BREWED) {
			gameObject.tag = "Tea";
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
		foreach (Ingredient ingredient in recipeList) {
			// Checks for a collision with an ingredient gameObject.
			if (collision.gameObject.CompareTag(ingredient.name)) {
				// Only add the ingredient if it's not present within the mug.
				if (!ingredient.added) {
					AddIngredient(ingredient);
					// Destroy the ingredient.
					Destroy(collision.gameObject.transform.parent.gameObject);
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
	}

	public void PlayerPickedUpMug() {
		// Mug inheritence is as follows: Player > Hand > Mug.
		playerHoldingMug = gameObject.transform.parent.parent.gameObject;
	}
}
