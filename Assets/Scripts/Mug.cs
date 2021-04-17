using System.Collections.Generic;
using UnityEngine;

public class Mug : MonoBehaviour {
	public Beverages.Beverage Beverage { get { return beverage; } private set { } }

	private enum BREW_STATES {
		EMPTY,
		BREWED,
		SPOILED,
		COUNT
	} BREW_STATES brewState = BREW_STATES.EMPTY;

	private Beverages.Beverage beverage = new Beverages.Beverage();

	private Beverages beverageClass = null;

	[SerializeField]
	private GameObject mugLiquid = null;

	private void Start() {
		beverageClass = GameObject.FindGameObjectWithTag("BeverageClass").GetComponent<Beverages>();
		// Create unknown beverage.
		beverage.name = "Unknown";
		beverage.recipeType = Beverages.RecipeTypes.Unknown;
		beverage.ingredients = new Beverages.Ingredient[beverageClass.ingredients.Length];
		beverageClass.ingredients.CopyTo(beverage.ingredients, 0);
		beverage.image = null;
		beverage.ingredients[0].count = 0;
		beverage.ingredients[1].count = 0;
		beverage.ingredients[2].count = 0;
		beverage.ingredients[3].count = 0;
		beverage.ingredients[4].count = 0;
	}

	private void Update() {
		if (brewState == BREW_STATES.BREWED ||
			brewState == BREW_STATES.SPOILED) {
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
		foreach (Beverages.Ingredient ingredient in beverage.ingredients) {
			// Check for a collision with an ingredient.
			if (collision.gameObject.CompareTag(ingredient.name)) {
				AddIngredient(ingredient);
				// Destroy the ingredient through its parent gameObject.
				Destroy(collision.gameObject.transform.parent.gameObject);
				UpdateBrewState();
			}
		}
	}

	/// <summary>
	/// Tries adding an ingredient to the beverage.
	/// </summary>
	/// <param name="ingredient"> Ingredient to add. </param>
	private void AddIngredient(Beverages.Ingredient ingredient) {
		// Accept the first recipe type to contact.
		if (beverage.recipeType == Beverages.RecipeTypes.Unknown) {
			beverage.recipeType = ingredient.ownerRecipe;
		}

		// Find collided ingredient among the beverage's ingredients.
		for (int i = 0; i < beverage.ingredients.Length; ++i) {
			if (beverage.ingredients[i].name == ingredient.name) {
				beverage.ingredients[i].isAdded = true;
				++beverage.ingredients[i].count;
				return;
			}
		}
	}

	private void UpdateBrewState() {
		// Keeps track of possible beverages to be brewed.
		LinkedList<Beverages.Beverage> brewedBeverages = new LinkedList<Beverages.Beverage>();

		// Loops through all beverages that can be made.
		for (int i = 0; i < beverageClass.beverageKeys.Length; ++i) {
			// Stores the beverage recipe the player could be trying to make.
			Beverages.Beverage possibleBeverage = beverageClass.recipes[beverageClass.beverageKeys[i]];

			if (CheckWhatsBrewing(ref possibleBeverage)) {
				brewedBeverages.AddLast(possibleBeverage);
			}
		}

		if (brewedBeverages.Count > 0) {
			// The beverage with the most ingredients added.
			int largestBrew = 0;
			LinkedListNode<Beverages.Beverage> node = brewedBeverages.First;
			Beverages.Beverage brewedBeverage = node.Value;

			// If player has added enough ingredients for multiple beverages to be brewed 
			// we need to decide which one to use.
			for (int j = 0; j < brewedBeverages.Count; ++j, node = node.Next) {
				// Find brewed beverage with the most ingredients.
				if (node.Value.ingredients.Length > largestBrew) {
					brewedBeverage = node.Value;
					largestBrew = node.Value.ingredients.Length;
				}
			}

			// Identify what's in the mug.
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

	/// <summary>
	/// Checks if the mug is holding a certain type of beverage.
	/// </summary>
	/// <param name="possibleBeverage"> Beverage to compare mug's contents to. </param>
	/// <returns> True if the argument matches the mug's beverage type. </returns>
	private bool CheckWhatsBrewing(ref Beverages.Beverage possibleBeverage) {
		// Only check recipe types matching beverage type.
		if (possibleBeverage.recipeType != beverage.recipeType) {
			return false;
		}

		foreach (Beverages.Ingredient ingredient in beverage.ingredients) {
			// Is an ingredient apart of the argument recipe.
			bool inRecipe = false;

			for (int j = 0; j < possibleBeverage.ingredients.Length; ++j) {
				if (ingredient.name == possibleBeverage.ingredients[j].name) {
					inRecipe = true;

					if (ingredient.isAdded && ingredient.count == possibleBeverage.ingredients[j].count) {
						break;
					} else if (ingredient.isAdded && ingredient.count > possibleBeverage.ingredients[j].count) {
						if (brewState == BREW_STATES.BREWED) {
							brewState = BREW_STATES.SPOILED;
						}

						return false;
					} else if (!ingredient.isAdded && ingredient.count == 0) {
						brewState = BREW_STATES.EMPTY;
						return false;
					}
				}
			}

			// Check if an ingredient has been added that wasn't found in the recipe.
			if (!inRecipe && ingredient.count > 0) {
				if (brewState == BREW_STATES.BREWED) {
					brewState = BREW_STATES.SPOILED;
				}

				return false;
			}
		}

		return true;
	}
}
