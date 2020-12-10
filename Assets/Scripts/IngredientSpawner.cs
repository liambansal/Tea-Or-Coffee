using UnityEngine;

public class IngredientSpawner : MonoBehaviour {
	private float spawnTime = 0.0f;

	[SerializeField]
	private Transform spawn = null;
	[SerializeField]
	private GameObject ingredient = null;
	private GameObject spawnedIngredient = null;
	private GameObject lastSpawnedIngredient = null;

	private void Start() {
		SpawnIngredient();
	}

	private void Update() {
		if (spawnTime >= 0.0f) {
			spawnTime -= Time.deltaTime;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.transform.parent.gameObject == spawnedIngredient) {
			if (spawnTime < 0.0f) {
				SpawnIngredient();
				spawnTime = 1.0f;
			}
		}
	}

	private void SpawnIngredient() {
		spawnedIngredient = Instantiate(ingredient, spawn.position, Quaternion.LookRotation(Vector3.up, Vector3.forward));
	}
}
