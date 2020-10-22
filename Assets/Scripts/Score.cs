using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
	private int score = 0;

	[SerializeField]
	private Text scoreObject = null;

	private void Awake() {
		scoreObject.text = score.ToString();
	}

	public void IncrementScore() {
		scoreObject.text = (score += 1).ToString();
	}
}