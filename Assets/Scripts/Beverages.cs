using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Beverages : MonoBehaviour {
	static public string[] beverageKeys { get; private set; } = {
		"Tea",
		"Coffee",
		"BuildersBrew"
	};

	/// <summary>
	/// Possible beverages to create.
	/// </summary>
	static public Dictionary<string, Beverage> beverages { get; private set; } = new Dictionary<string, Beverage>() {
		{ beverageKeys[0], new Beverage(beverageKeys[0],
			new Ingredient[4] {
				new Ingredient("TeaBag", false, 1, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Tea Bag.png", typeof(Sprite))),
				new Ingredient("Sugar", false, 1, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Sugar.png", typeof(Sprite))),
				new Ingredient("Water", false, 1, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Water.png", typeof(Sprite))),
				new Ingredient("Milk", false, 1, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Milk.png", typeof(Sprite)))
			},
			(Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Cup of Tea.png", typeof(Sprite)),
			(Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/Beverages/Tea.mat", typeof(Material)))
		},
		{ beverageKeys[1], new Beverage(beverageKeys[1],
			new Ingredient[4] {
				new Ingredient("CoffeeTin", false, 1, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Coffee.png", typeof(Sprite))),
				new Ingredient("Sugar", false, 1, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Sugar.png", typeof(Sprite))),
				new Ingredient("Water", false, 1, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Water.png", typeof(Sprite))),
				new Ingredient("Milk", false, 1, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Milk.png", typeof(Sprite)))
			},
			(Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Cup of Coffee.png", typeof(Sprite)),
			(Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/Beverages/Coffee.mat", typeof(Material)))
		},
		{ beverageKeys[2], new Beverage(beverageKeys[2],
			new Ingredient[4] {
				new Ingredient("TeaBag", false, 2, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Tea Bag.png", typeof(Sprite))),
				new Ingredient("Sugar", false, 1, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Sugar.png", typeof(Sprite))),
				new Ingredient("Water", false, 1, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Water.png", typeof(Sprite))),
				new Ingredient("Milk", false, 1, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Milk.png", typeof(Sprite)))
			},
			(Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Cup of Tea.png", typeof(Sprite)),
			(Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/Beverages/Builders Brew.mat", typeof(Material)))
		},
	};

	static public Ingredient[] ingredientList { get; private set; } = {
		new Ingredient("TeaBag", false, 0, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Tea Bag.png", typeof(Sprite))),
		new Ingredient("CoffeeTin", false, 0, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Coffee.png", typeof(Sprite))),
		new Ingredient("Sugar", false, 0, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Sugar.png", typeof(Sprite))),
		new Ingredient("Water", false, 0, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Water.png", typeof(Sprite))),
		new Ingredient("Milk", false, 0, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Milk.png", typeof(Sprite)))
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
}
