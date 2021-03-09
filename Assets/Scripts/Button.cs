using UnityEngine;

public interface IButton {
	void ButtonPressed(Button caller);
}

public class Button : MonoBehaviour, IButton {
	/// <summary>
	/// The object receiving a call from the button.
	/// </summary>
	[SerializeField]
	private GameObject[] receivers = { null };

	public void ButtonPressed(Button caller) {
		foreach (GameObject receiver in receivers) {
			receiver.GetComponent<IButton>().ButtonPressed(this);
		}
	}
}
