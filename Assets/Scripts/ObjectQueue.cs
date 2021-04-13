using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectQueue : MonoBehaviour {
	public Dictionary<GameObject, Vector3> Queue { get; private set; } = new Dictionary<GameObject, Vector3>();

	[SerializeField]
	private int maximumQueuePositions = 1;
	[SerializeField]
	private float queueGapDistance = 1.0f;

	private Vector3 queueDirection = Vector3.zero;
	private Vector3 lastQueuePosition = Vector3.zero;

	public bool AddToQueue(GameObject objectTransform) {
		if (Queue.Count >= maximumQueuePositions) {
			return false;
		}

		if (Queue.Count > 0) {
			lastQueuePosition = lastQueuePosition + queueDirection * queueGapDistance;
		} else {
			lastQueuePosition = transform.position;
		}

		Queue.Add(objectTransform, lastQueuePosition);
		return true;
	}

	public void RemoveFromQueue(GameObject objectTransform) {
		if (Queue.ContainsKey(objectTransform)) {
			Queue.Remove(objectTransform);
			RefreshPositions();
		}
	}

	public Vector3 GetPosition(GameObject transform) {
		Vector3 position;
		Queue.TryGetValue(transform, out position);
		return position;
	}

	private void RefreshPositions() {
		lastQueuePosition = transform.position;

		foreach (KeyValuePair<GameObject, Vector3> element in Queue) {
			lastQueuePosition = lastQueuePosition + queueDirection * queueGapDistance;
			Queue[element.Key] = lastQueuePosition;
		}
	}

	private void Start() {
		queueDirection = transform.forward;
	}
}
