using System.Collections.Generic;
using UnityEngine;

public class ObjectQueue : MonoBehaviour {
	/// <summary>
	/// Stores a copy of a gameObject with its queue position.
	/// </summary>
	public Dictionary<GameObject, Vector3> Queue { get; private set; } = new Dictionary<GameObject, Vector3>();

	[SerializeField]
	private int maximumQueuePositions = 1;
	[SerializeField]
	private float queueGapDistance = 1.0f;

	private Vector3 queueDirection = Vector3.zero;
	private Vector3 lastQueuePosition = Vector3.zero;

	public bool AddToQueue(GameObject queueObject) {
		if (Queue.Count >= maximumQueuePositions) {
			return false;
		}

		if (Queue.Count > 0) {
			lastQueuePosition = lastQueuePosition + queueDirection * queueGapDistance;
		} else {
			lastQueuePosition = transform.position;
		}

		Queue.Add(queueObject, lastQueuePosition);
		return true;
	}

	public void RemoveFromQueue(GameObject queueObject) {
		if (Queue.ContainsKey(queueObject)) {
			Queue.Remove(queueObject);
			RefreshPositions();
		}
	}

	public Vector3 GetPosition(GameObject queueObject) {
		Vector3 position;
		Queue.TryGetValue(queueObject, out position);
		return position;
	}

	private void RefreshPositions() {
		GameObject[] queueCopy = new GameObject[Queue.Count];
		Queue.Keys.CopyTo(queueCopy, 0);
		int cardCount = 0;

		foreach (GameObject element in queueCopy) {
			lastQueuePosition = transform.position + queueDirection * queueGapDistance * cardCount++;
			Queue[element] = lastQueuePosition;
		}
	}

	private void Start() {
		queueDirection = transform.forward;
	}
}
