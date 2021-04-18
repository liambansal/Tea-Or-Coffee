using UnityEngine;

public class CafeLights : MonoBehaviour, IButton {
	private const int intensity = 2;
	[SerializeField]
	private GameObject[] cafeLights = { null };

	/// <summary>
	/// Turns on cafe lights with a button press.
	/// </summary>
	/// <param name="caller"> Button to press. </param>
	public void ButtonPressed(Button caller) {
		// Check IButton is implemented in object that called here.
		if (caller.gameObject.GetComponent<IButton>() != null) {
			foreach (GameObject light in cafeLights) {
				light.GetComponent<Light>().intensity = intensity;
			}
		}
	}
}
