using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the general state of the game at any given time.
/// </summary>
public class GameStateManager : MonoBehaviour, IButton {
	public GameStates GameState { get; private set; } = GameStates.WaitingToStart;

	public enum GameStates {
		WaitingToStart,
		Gameplay
	};

	private Clock clock = null;

	/// <summary>
	/// Called after a button press.
	/// </summary>
	/// <param name="caller"> Button being pressed. </param>
	public void ButtonPressed(Button caller) {
		// Check IButton is implemented in object that called here.
		if (caller.gameObject.GetComponent<IButton>() != null) {
			GameState = GameStates.Gameplay;
		}
	}

	/// <summary>
	/// Finds the scene's clock gameObject.
	/// </summary>
	private void Start() {
		clock = GameObject.FindGameObjectWithTag("Clock").GetComponent<Clock>();
	}

	/// <summary>
	/// Checks when the game should end.
	/// </summary>
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
		// Delete VR player.
		Destroy(GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}
}
