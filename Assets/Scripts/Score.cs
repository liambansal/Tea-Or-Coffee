using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	private int score = 0;

	[SerializeField]
	private Text scoreText = null;

	public void IncrementScore() {
		scoreText.text = (score += 1).ToString();
	}

	/// <summary>
	/// Initialises score.
	/// </summary>
	private void Awake() {
		scoreText.text = score.ToString();
	}
}