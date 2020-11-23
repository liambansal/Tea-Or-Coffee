using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Beverages : MonoBehaviour {
	public struct Ingredient {
		public string name;
		public bool isAdded;
		public Sprite image;

		public Ingredient(string newname, bool newIsAdded, Sprite newImage) {
			name = newname;
			isAdded = newIsAdded;
			image = newImage;
		}
	};

	public struct Beverage {
		public string name;
		public Ingredient[] ingredients;
		public Sprite image;

		public Beverage(string newName, Ingredient[] newIngredients, Sprite newImage) {
			name = newName;
			ingredients = newIngredients;
			image = newImage;
		}
	};

	/// <summary>
	/// List of possible beverages to create.
	/// </summary>
	static public Dictionary<string, Beverage> beverages { get; private set; } = new Dictionary<string, Beverage>() {
		{ "Tea", new Beverage("Tea",
			new Ingredient[4] {
				new Ingredient("TeaBag", false, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Tea.png", typeof(Sprite))),
				new Ingredient("Sugar", false, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Sugar.png", typeof(Sprite))),
				new Ingredient("Water", false, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Water.png", typeof(Sprite))),
				new Ingredient("Milk", false, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Milk.png", typeof(Sprite)))
			},
			(Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Water.png", typeof(Sprite)))
		},
		{ "Coffee", new Beverage("Coffee",
			new Ingredient[4] {
				new Ingredient("Coffee", false, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Coffee.png", typeof(Sprite))),
				new Ingredient("Sugar", false, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Sugar.png", typeof(Sprite))),
				new Ingredient("Water", false, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Water.png", typeof(Sprite))),
				new Ingredient("Milk", false, (Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Milk.png", typeof(Sprite)))
			},
			(Sprite)AssetDatabase.LoadAssetAtPath("Assets/Textures/Water.png", typeof(Sprite)))
		}
	};
}
