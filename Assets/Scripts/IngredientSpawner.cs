using UnityEngine;

public class IngredientSpawner : MonoBehaviour {
	private float respawnTime = 0.0f;

	[SerializeField]
	private Transform spawn = null;
	[SerializeField]
	private GameObject ingredient = null;
	private GameObject spawnedIngredient = null;

	private void Update() {
		if (respawnTime <= 0.0f) {
			SpawnIngredient();
			respawnTime = Mathf.Infinity;
		} else {
			if (respawnTime == Mathf.Infinity && Vector3.Distance(gameObject.transform.position, spawnedIngredient.transform.position) > 1.0f) {
				const int seconds = 3;
				respawnTime = seconds;
			}

			respawnTime -= Time.deltaTime;
		}
	}

	private void SpawnIngredient() {
		spawnedIngredient = Instantiate(ingredient, spawn.position, Quaternion.LookRotation(Vector3.up, Vector3.forward));
	}
}
