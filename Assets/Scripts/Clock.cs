using UnityEngine;

public class Clock : MonoBehaviour {
	/// <summary>
	/// Amount of time in one level session in seconds.
	/// </summary>
	public int MaxLength { get; private set; } = 300;
	/// <summary>
	/// Returns the length of time the unity scene has been open for in seconds.
	/// </summary>
	public int CurrentTime = 0;

	private GameStateManager gameStateManager = null;

	private void Start() {
		gameStateManager = GameObject.FindGameObjectWithTag("GameStateManager").GetComponent<GameStateManager>();
	}

	// Update is called once per frame
	private void Update() {
		if (gameStateManager.GameState != GameStateManager.GameStates.Gameplay) {
			// Only count if we're in the gameplay state.
			return;
		}

		if (CurrentTime < MaxLength) {
			// Get length of time level has been open.
			CurrentTime = (int)Time.timeSinceLevelLoad;
		}
	}
}
