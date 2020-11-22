using UnityEngine;
using UnityEngine.UI;

public class OrderCard : MonoBehaviour {
	[SerializeField]
	private GameObject ingredientList = null;

	[SerializeField]
	private Image beverageImage = null;
	[SerializeField]
	private Image ingredientImage = null;

	private Beverages.Beverage beverage;

	public void SetOrder(Beverages.Beverage setBeverage) {
		beverage = setBeverage;
		beverageImage.sprite = beverage.image;

		for (int i = 0; i < beverage.ingredients.Length; ++i) {
			ingredientImage.sprite = beverage.ingredients[i].image;
			Instantiate(ingredientImage, ingredientList.transform);
		}
	}
}
