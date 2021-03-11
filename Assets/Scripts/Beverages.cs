using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Beverages : MonoBehaviour {
	static public string[] beverageKeys { get; private set; } = {
		"Tea",
		"Coffee",
		"BuildersBrew"
	};

	public struct Ingredient {
		public string name;
		public bool isAdded;
		public int count;
		public Sprite image;

		public Ingredient(string newname, bool newIsAdded, int newCount, Sprite newImage) {
			name = newname;
			isAdded = newIsAdded;
			count = newCount;
			image = newImage;
		}
	};

	public struct Beverage {
		public string name;
		public Ingredient[] ingredients;
		public Sprite image;
		public Material material;

		public Beverage(string newName, Ingredient[] newIngredients, Sprite newImage, Material newMaterial) {
			name = newName;
			ingredients = newIngredients;
			image = newImage;
			material = newMaterial;
		}
	};

	public static Ingredient[] ingredientList { get; private set; }

	private static Beverage tea;
	private static Beverage coffee;
	private static Beverage buildersBrew ;

	private static Ingredient teaBag = new Ingredient("TeaBag", false, 1, null);
	private static Ingredient teaBagStrong = new Ingredient("TeaBag", false, 2, null);
	private static Ingredient coffeeTin = new Ingredient("CoffeeTin", false, 1, null);
	private static Ingredient sugar = new Ingredient("Sugar", false, 1, null);
	private static Ingredient water = new Ingredient("Water", false, 1, null);
	private static Ingredient milk = new Ingredient("Milk", false, 1, null);

	/// <summary>
	/// Possible beverages to create.
	/// </summary>
	static public Dictionary<string, Beverage> beverages { get; private set; } = null;

	private void Start() {
		// Gather all the beverage and ingredient sprites and materials.
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

		ingredientList = new Ingredient[] {
			teaBag,
			coffeeTin,
			sugar,
			water,
			milk
		};

		tea = new Beverage("Tea",
			new Ingredient[4] {
				teaBag,
				sugar,
				water,
				milk
			},
			tea.image,
			tea.material);
		coffee = new Beverage("Coffee",
			new Ingredient[4] {
				coffeeTin,
				sugar,
				water,
				milk
			},
			coffee.image,
			coffee.material);
		buildersBrew = new Beverage("BuildersBrew",
			new Ingredient[4] {
				teaBagStrong,
				sugar,
				water,
				milk
			},
			buildersBrew.image,
			buildersBrew.material);

		beverages = new Dictionary<string, Beverage>() {
			{ beverageKeys[0], tea },
			{ beverageKeys[1], coffee },
			{ beverageKeys[2], buildersBrew },
		};
	}
}
