using UnityEngine;
using UnityEngine.UI;

public class TipJar : MonoBehaviour {
	public int CashAmount { get; private set; } = 0;

	[SerializeField]
	private Text score = null;

	/// <summary>
	/// Consumes cash on trigger enter.
	/// </summary>
	/// <param name="other"></param>
	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Cash")) {
			Destroy(other.gameObject);
			score.text = (++CashAmount).ToString();
		}
	}
}
