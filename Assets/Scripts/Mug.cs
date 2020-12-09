using UnityEngine;

public class Mug : MonoBehaviour {
	private enum BREW_STATES {
		EMPTY,
		BREWED
	} BREW_STATES brewState = BREW_STATES.EMPTY;

	private Beverages.Beverage beverage = new Beverages.Beverage();

	[SerializeField]
	private GameObject mugLiquid = null;
	private GameObject playerHoldingMug = null;

	// TODO: create list to hold currently added ingredients instead of checing them off a premade beverage ingredient array.

	public void PlayerPickedUpMug() {
		playerHoldingMug = gameObject.transform.root.gameObject;
	}

	private void Start() {
		beverage.name = Beverages.beverages["Tea"].name;
		beverage.ingredients = new Beverages.Ingredient[Beverages.beverages["Tea"].ingredients.Length];
		Beverages.beverages["Tea"].ingredients.CopyTo(beverage.ingredients, 0);
		beverage.image = Beverages.beverages["Tea"].image;
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
		// TODO: allow collision with any ingredients
		// TODO: Dont let tea bags be added if coffee if present and vise versa.
		foreach (Beverages.Ingredient ingredient in beverage.ingredients) {
			// Checks for a collision with an ingredient gameObject.
			if (collision.gameObject.CompareTag(ingredient.name)) {
				// Only add the ingredient if it's not present within the mug.
				if (!ingredient.isAdded) {
					AddIngredient(ingredient);
					// Destroy the ingredients parent.
					Destroy(collision.gameObject.transform.parent.gameObject);
					UpdateBrewState();
				}
			}
		}
	}

	private void AddIngredient(Beverages.Ingredient ingredient) {
		Beverages.Ingredient localIngredient = ingredient;
		localIngredient.isAdded = true;

		for (int i = 0; i < beverage.ingredients.Length; ++i) {
			if (beverage.ingredients[i].name == ingredient.name) {
				// Checks off the collided ingredient from the recipe list.
				beverage.ingredients[i] = localIngredient;
			}
		}
	}

	private void UpdateBrewState() {
		// TODO: temp to store possible beverage
		// TODO: check ingredients of all possible beverages, whichever bevergae has all of its ingredients added is the mug's beverage.
		// Beverages with more ingredients will be prefered, as long as they have all been added.
		// Loops through ossible beverages.
		for (int i = 0; i < beverage.ingredients.Length; ++i) {
			// Check if ingredients have been added.
			foreach (Beverages.Ingredient ingredient in beverage.ingredients) {
				if (!ingredient.isAdded) {
					brewState = BREW_STATES.EMPTY;
					break;
				} else {
					brewState = BREW_STATES.BREWED;
				}
			}

			// TODO: check if all ingredients are added.
			// TODO: check which one of two (or more) beverages have more ingredients that are all added.
		}
	}
}
