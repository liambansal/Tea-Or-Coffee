using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderCard : MonoBehaviour {
	[SerializeField]
	private GameObject ingredientList = null;

	[SerializeField]
	private Image beverageImage = null;

	private Beverages.Beverage beverage;

	public OrderCard(Beverages.Beverage setBeverage) {
		beverage = setBeverage;
		beverageImage.sprite = beverage.image;
	}

	private void Start() {
		for (int i = 0; i < beverage.ingredients.Length; ++i) {
			Image ingredientImage = beverageImage;
			ingredientImage.sprite = beverage.ingredients[i].image;
			Instantiate(ingredientImage, ingredientList.transform);
		}
	}
}
