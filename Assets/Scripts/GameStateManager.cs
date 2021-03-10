using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour, IButton {
	public GameStates GameState { get; private set; } = GameStates.WaitingToStart;

	public enum GameStates {
		WaitingToStart,
		Gameplay
	}

	private Clock clock = null;

	public void ButtonPressed(Button caller) {
		// Check IButton is implemented in object that called here.
		if (caller.gameObject.GetComponent<IButton>() != null) {
			GameState = GameStates.Gameplay;
		}
	}

	private void Start() {
		clock = GameObject.FindGameObjectWithTag("Clock").GetComponent<Clock>();
	}

	private void Update() {
		if (clock.CurrentTime >= clock.MaxLength) {
			GameOver();
			return;
		}
	}

	/// <summary>
	/// Reloads the active scene.
	/// </summary>
	private void GameOver() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
