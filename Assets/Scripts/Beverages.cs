using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for creating/referencing beverages and their ingredients.
/// </summary>
public class Beverages : MonoBehaviour {
	/// <summary>
	/// Names of the beverages that can be made.
	/// </summary>
	public string[] beverageKeys { get; private set; } = {
		"Tea",
		"Coffee",
		"BuildersBrew",
		"Latte",
		"Espresso"
	};

	public struct Ingredient {
		public string name;
		public bool isAdded;
		public int count;
		public Sprite image;
		public RecipeTypes ownerRecipe;

		public Ingredient(string newname, bool newIsAdded, int newCount, Sprite newImage, RecipeTypes newOwnerRecipe) {
			name = newname;
			isAdded = newIsAdded;
			count = newCount;
			image = newImage;
			ownerRecipe = newOwnerRecipe;
		}
	};

	public struct Beverage {
		public string name;
		public RecipeTypes recipeType;
		public Ingredient[] ingredients;
		public Sprite image;
		public Material material;

		public Beverage(string newName, RecipeTypes newRecipeType, Ingredient[] newIngredients, Sprite newImage, Material newMaterial) {
			name = newName;
			recipeType = newRecipeType;
			ingredients = newIngredients;
			image = newImage;
			material = newMaterial;
		}
	};

	/// <summary>
	/// The category that a beverage or ingredient belongs to.
	/// </summary>
	public enum RecipeTypes {
		Tea,
		Coffee,
		Unknown,
		Count
	};

	/// <summary>
	/// An array of all existing ingredients.
	/// </summary>
	public Ingredient[] ingredients { get; private set; }

	/// <summary>
	/// Stores all of the beverages that can be made.
	/// </summary>
	public Dictionary<string, Beverage> recipes { get; private set; } = null;

	private Beverage tea;
	private Beverage coffee;
	private Beverage buildersBrew;
	private Beverage latte;
	private Beverage espresso;

	private Ingredient teaBag = new Ingredient("TeaBag", false, 1, null, RecipeTypes.Tea);
	private Ingredient coffeeTin = new Ingredient("CoffeeTin", false, 1, null, RecipeTypes.Coffee);
	private Ingredient sugar = new Ingredient("Sugar", false, 1, null, RecipeTypes.Unknown);
	private Ingredient water = new Ingredient("Water", false, 1, null, RecipeTypes.Unknown);
	private Ingredient milk = new Ingredient("Milk", false, 1, null, RecipeTypes.Unknown);

	[SerializeField]
	private Sprite teaBagSprite = null;
	[SerializeField]
	private Sprite coffeTinSprite = null;
	[SerializeField]
	private Sprite sugarSprite = null;
	[SerializeField]
	private Sprite waterSprite = null;
	[SerializeField]
	private Sprite milkSprite = null;

	/// <summary>
	/// Initializes everything.
	/// </summary>
	private void Start() {
		// Initialize unset ingredient properties.
		teaBag.image = teaBagSprite;
		coffeeTin.image = coffeTinSprite;
		sugar.image = sugarSprite;
		water.image = waterSprite;
		milk.image = milkSprite;

		ingredients = new Ingredient[] {
			teaBag,
			coffeeTin,
			sugar,
			water,
			milk
		};

		// Create each individual beverage recipe.
		tea = new Beverage("Tea",
			RecipeTypes.Tea,
			new Ingredient[4] {
				teaBag,
				sugar,
				water,
				milk
			},
			tea.image,
			tea.material);
		coffee = new Beverage("Coffee",
			RecipeTypes.Coffee,
			new Ingredient[3] {
				coffeeTin,
				sugar,
				water
			},
			coffee.image,
			coffee.material);
		buildersBrew = new Beverage("BuildersBrew",
			RecipeTypes.Tea,
			new Ingredient[3] {
				teaBag,
				sugar,
				water
			},
			buildersBrew.image,
			buildersBrew.material);
		latte = new Beverage("Latte",
			RecipeTypes.Coffee,
			new Ingredient[4] {
				coffeeTin,
				sugar,
				water,
				milk
			},
			latte.image,
			latte.material);
		espresso = new Beverage("Espresso",
			RecipeTypes.Coffee,
			new Ingredient[2] {
				coffeeTin,
				water,
			},
			espresso.image,
			espresso.material);

		// Initialize unset beverage properties.
		tea.image = GameObject.FindGameObjectWithTag("Tea").GetComponent<Image>().sprite;
		tea.material = GameObject.FindGameObjectWithTag("Tea").GetComponent<MeshRenderer>().material;
		coffee.image = GameObject.FindGameObjectWithTag("Coffee").GetComponent<Image>().sprite;
		coffee.material = GameObject.FindGameObjectWithTag("Coffee").GetComponent<MeshRenderer>().material;
		buildersBrew.image = GameObject.FindGameObjectWithTag("BuildersBrew").GetComponent<Image>().sprite;
		buildersBrew.material = GameObject.FindGameObjectWithTag("BuildersBrew").GetComponent<MeshRenderer>().material;
		latte.image = GameObject.FindGameObjectWithTag("Latte").GetComponent<Image>().sprite;
		latte.material = GameObject.FindGameObjectWithTag("Latte").GetComponent<MeshRenderer>().material;
		espresso.image = GameObject.FindGameObjectWithTag("Espresso").GetComponent<Image>().sprite;
		espresso.material = GameObject.FindGameObjectWithTag("Espresso").GetComponent<MeshRenderer>().material;

		// Add recipes to a dictionary for accessibility from other classes.
		recipes = new Dictionary<string, Beverage>() {
			{ beverageKeys[0], tea },
			{ beverageKeys[1], coffee },
			{ beverageKeys[2], buildersBrew },
			{ beverageKeys[3], latte },
			{ beverageKeys[4], espresso }
		};
	}
}
