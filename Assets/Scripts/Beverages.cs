using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beverages : MonoBehaviour {
	public string[] beverageKeys { get; private set; } = {
		"Tea",
		"Coffee",
		"BuildersBrew"
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

	public enum RecipeTypes {
		Tea,
		Coffee,
		Unknown,
		Count
	}

	public Ingredient[] ingredients { get; private set; }

	/// <summary>
	/// Possible beverages to create.
	/// </summary>
	public Dictionary<string, Beverage> recipes { get; private set; } = null;

	private Beverage tea;
	private Beverage coffee;
	private Beverage buildersBrew ;

	private Ingredient teaBag = new Ingredient("TeaBag", false, 1, null, RecipeTypes.Tea);
	private Ingredient teaBagStrong = new Ingredient("TeaBag", false, 2, null, RecipeTypes.Tea);
	private Ingredient coffeeTin = new Ingredient("CoffeeTin", false, 1, null, RecipeTypes.Coffee);
	private Ingredient sugar = new Ingredient("Sugar", false, 1, null, RecipeTypes.Unknown);
	private Ingredient water = new Ingredient("Water", false, 1, null, RecipeTypes.Unknown);
	private Ingredient milk = new Ingredient("Milk", false, 1, null, RecipeTypes.Unknown);

	private void Start() {
		// Initialize unset beverage and ingredient properties.
		tea.image = GameObject.FindGameObjectWithTag("Tea").GetComponent<Image>().sprite;
		tea.material = GameObject.FindGameObjectWithTag("Tea").GetComponent<MeshRenderer>().material;
		coffee.image = GameObject.FindGameObjectWithTag("Coffee").GetComponent<Image>().sprite;
		coffee.material = GameObject.FindGameObjectWithTag("Coffee").GetComponent<MeshRenderer>().material;
		buildersBrew.image = GameObject.FindGameObjectWithTag("BuildersBrew").GetComponent<Image>().sprite;
		buildersBrew.material = GameObject.FindGameObjectWithTag("BuildersBrew").GetComponent<MeshRenderer>().material;
		teaBag.image = GameObject.FindGameObjectWithTag("TeaBag").GetComponent<Image>().sprite;
		teaBagStrong.image = GameObject.FindGameObjectWithTag("TeaBag").GetComponent<Image>().sprite;
		coffeeTin.image = GameObject.FindGameObjectWithTag("CoffeeTin").GetComponent<Image>().sprite;
		sugar.image = GameObject.FindGameObjectWithTag("Sugar").GetComponent<Image>().sprite;
		water.image = GameObject.FindGameObjectWithTag("Water").GetComponent<Image>().sprite;
		milk.image = GameObject.FindGameObjectWithTag("Milk").GetComponent<Image>().sprite;

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
			new Ingredient[4] {
				coffeeTin,
				sugar,
				water,
				milk
			},
			coffee.image,
			coffee.material);
		buildersBrew = new Beverage("BuildersBrew",
			RecipeTypes.Tea,
			new Ingredient[4] {
				teaBagStrong,
				sugar,
				water,
				milk
			},
			buildersBrew.image,
			buildersBrew.material);

		// Add recipes to a dictionary for accessibility from other classes.
		recipes = new Dictionary<string, Beverage>() {
			{ beverageKeys[0], tea },
			{ beverageKeys[1], coffee },
			{ beverageKeys[2], buildersBrew },
		};
	}
}
