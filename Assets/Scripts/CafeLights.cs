using UnityEngine;

public class CafeLights : MonoBehaviour, IButton {
	private const int intensity = 2;
	[SerializeField]
	private GameObject[] cafeLights = { null };

	public void ButtonPressed(Button caller) {
		// Check IButton is implemented in object that called here.
		if (caller.gameObject.GetComponent<IButton>() != null) {
			foreach (GameObject light in cafeLights) {
				// Turn the light on.
				light.GetComponent<Light>().intensity = intensity;
			}
		}
	}
}
