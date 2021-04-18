using UnityEngine;

public interface IButton {
	void ButtonPressed(Button caller);
}

public class Button : MonoBehaviour, IButton {
	/// <summary>
	/// The object(s) receiving a call from this button.
	/// </summary>
	[SerializeField]
	private GameObject[] receivers = { null };

	/// <summary>
	/// Notifies all receivers of a button press.
	/// </summary>
	/// <param name="caller"> Button being pressed. </param>
	public void ButtonPressed(Button caller) {
		foreach (GameObject receiver in receivers) {
			receiver.GetComponent<IButton>().ButtonPressed(this);
		}
	}
}
